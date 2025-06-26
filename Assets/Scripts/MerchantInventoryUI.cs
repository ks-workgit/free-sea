using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MerchantInventoryUI : MonoBehaviour
{
    [SerializeField] Transform m_slotParent;
    [SerializeField] GameObject m_slotPrefab;

    //[SerializeField] Image m_selectedIcon;
    //[SerializeField] TMP_Text m_ownedText;
    [SerializeField] TMP_InputField m_inputField;
    [SerializeField] Button m_sellButton;
    [SerializeField] TMP_Text m_oreNameText;
    [SerializeField] TMP_Text m_oreCountText;
    [SerializeField] TMP_Text m_moneyText;

    private Inventory m_playerInventory;
    private InventoryItem m_selectedItem;
    [SerializeField] private InventoryUI m_inventoryUI;

    public void Initialize(Inventory inventory, InventoryUI inventoryUI)
	{
        m_playerInventory = inventory;
        m_inventoryUI = inventoryUI;

        //UpdateUI();

        //m_selectedItem = null;
        //m_inputField.gameObject.SetActive(false);
        //m_sellButton.gameObject.SetActive(false);
        //m_oreNameText.text = "";
        //m_oreCountText.text = "";
        //ClearSelection();
        m_sellButton.onClick.RemoveAllListeners();
        m_sellButton.onClick.AddListener(SellOre);

        ClearSelection();
        UpdateUI();
        UpdateMoneyText();
    }

    private void UpdateUI()
    {
        // 1. 既存のスロットを全て破棄
        foreach (Transform child in m_slotParent)
        {
            Destroy(child.gameObject);
        }

        // 2. プレイヤーのインベントリから鉱石情報を取得し、
        //    各アイテムごとにスロットを生成してUIに表示
        foreach (var item in m_playerInventory.GetOreList())
        {
            var obj = Instantiate(m_slotPrefab, m_slotParent);      // スロット生成
            var slot = obj.GetComponent<Slot>();                    // Slotスクリプト取得
            slot.Setup(item, m_playerInventory, SelectOre);         // スロットに情報渡す＋クリック時SelectOreを呼ぶ
        }
    }

    public void SelectOre(InventoryItem item)
    {
        m_selectedItem = item;

        m_oreNameText.text = item.m_ore.m_oreName;
        m_oreCountText.text = $"所持数: {item.m_quantity}";

        m_inputField.text = "1";
        m_inputField.gameObject.SetActive(true);
        m_sellButton.gameObject.SetActive(true);
    }

    private void SellOre()
    {
        if (m_selectedItem == null || m_playerInventory == null) return;

        int sellCount;
        if (!int.TryParse(m_inputField.text, out sellCount) || sellCount <= 0)
        {
            Debug.LogWarning("売却数が正しくありません");
            return;
        }

        sellCount = Mathf.Min(sellCount, m_selectedItem.m_quantity); // 上限を現在所持数に合わせる
        int gain = m_selectedItem.m_ore.m_value * sellCount;

        // インベントリから売却分を削除
        m_playerInventory.Remove(m_selectedItem.m_ore, sellCount);

        // 所持金に加算
        GameManager.Instance.AddMoney(gain);

        Debug.Log($"{m_selectedItem.m_ore.m_oreName} を {sellCount}個 売却（+{gain}G）");

        UpdateMoneyText();
        // UIを更新
        m_inputField.text = "";
        m_inputField.gameObject.SetActive(false);
        m_sellButton.gameObject.SetActive(false);
        m_oreNameText.text = "";
        m_oreCountText.text = "";
        m_selectedItem = null;

        // ここでインベントリUIも更新して再表示させる（重要）
        m_inventoryUI.UpdateUI(m_playerInventory, SelectOre);
    }

    private void UpdateMoneyText()
    {
        if (m_moneyText != null)
        {
            m_moneyText.text = $"所持金: {GameManager.Instance.GetMoney()} G";
        }
    }

    //   private void OnSlotSelected(InventoryItem item)
    //   {
    //       //m_selectedItem = item;
    //       //m_selectedIcon.sprite = item.m_ore.m_oreIcon;
    //       //m_selectedIcon.enabled = true;

    //       //m_ownedText.text = $"所持数: {item.m_quantity}";
    //       //m_sellCountInput.text = "1";
    //       //m_sellCountInput.interactable = true;
    //       //m_sellButton.interactable = true;
    //}

    //  public void OnClickSell()
    //  {
    ////      if (m_selectedItem == null) return;
    ////      if (!int.TryParse(m_sellCountInput.text, out int count)) return;

    ////count = Mathf.Clamp(count, 1, m_selectedItem.m_quantity);

    ////      int totalValue = m_selectedItem.m_ore.m_value * count;
    ////      GameManager.Instance.AddMoney(totalValue);

    ////      for (int i = 0; i < count; i++)
    ////      {
    ////          m_inventoryRef.Remove(m_selectedItem.m_ore);
    ////      }

    ////      Initialize(m_inventoryRef);
    //      //      m_inventory.Remove(m_selectedItem.m_ore, count);
    //      //UpdateUI();
    //      //ClearSelection();
    //  }

    private void ClearSelection()
    {
        m_selectedItem = null;
        m_inputField.text = "";
        m_inputField.gameObject.SetActive(false);

        m_sellButton.gameObject.SetActive(false);

        m_oreNameText.text = "";
        m_oreCountText.text = "";
    }
}
