using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MerchantInventoryUI : MonoBehaviour
{
    [SerializeField] TMP_InputField m_inputField;
    [SerializeField] Button m_sellButton;
    [SerializeField] TMP_Text m_oreNameText;
    [SerializeField] TMP_Text m_moneyText;

    private Inventory m_playerInventory;
    private InventoryItem m_selectedItem;
    [SerializeField] InventoryUI m_inventoryUI;

    private AudioSource m_audioSource;
    [SerializeField] AudioClip m_clip;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    public void Initialize(Inventory inventory, InventoryUI inventoryUI)
	{
        m_playerInventory = inventory;
        m_inventoryUI = inventoryUI;

        m_sellButton.onClick.RemoveAllListeners();
        m_sellButton.onClick.AddListener(SellOre);

        ClearSelection();
        
        UpdateMoneyText();

		m_inventoryUI.UpdateUI(m_playerInventory, SelectOre);
	}

    public void SelectOre(InventoryItem item)
    {
        m_selectedItem = item;

        m_oreNameText.text = item.m_ore.m_oreName;

        m_inputField.text = "";
        m_inputField.interactable = true;
        m_sellButton.interactable = true;
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
        m_audioSource.PlayOneShot(m_clip);

        Debug.Log($"{m_selectedItem.m_ore.m_oreName} を {sellCount}個 売却（+{gain}G）");

        UpdateMoneyText();

        // ここでインベントリUIも更新して再表示させる（重要）
        m_inventoryUI.UpdateUI(m_playerInventory, SelectOre);

        ClearSelection();
    }

    private void UpdateMoneyText()
    {
        if (m_moneyText != null)
        {
            m_moneyText.text = $"所持金: {GameManager.Instance.GetMoney()} G";
        }
    }

	private void ClearSelection()
    {
        m_selectedItem = null;
        m_inputField.text = "";
        m_inputField.interactable = false;

        m_sellButton.interactable = false;

        m_oreNameText.text = "";
    }
}
