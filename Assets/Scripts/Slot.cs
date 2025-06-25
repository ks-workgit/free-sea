using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField] Image m_icon;
    [SerializeField] TMP_Text m_quantityText;   // 所持数を表示するテキスト
    //[SerializeField] TMP_InputField m_sellCountInput;
    //[SerializeField] Button m_sellButton;
    [SerializeField] GameObject m_removeButton;

    private InventoryItem m_item;       // このスロットに表示されているアイテム情報
    private Inventory m_inventoryRef;   // アイテム削除などのためのインベントリ参照
    private System.Action<InventoryItem> m_onClickCallback;

	//private void OnEnable()
	//{
 //       Merchant.OnTalkChanged += UpdateSellUIVisibility;
	//}

	//private void OnDisable()
	//{
	//	Merchant.OnTalkChanged -= UpdateSellUIVisibility;
	//}

	// 鉱石情報とインベントリ参照を受け取り、スロットUIを更新する
	public void AddOre(InventoryItem item, Inventory inventory, System.Action<InventoryItem> onClick = null)
    {
        m_item = item;      // 表示するアイテムを保持
        m_inventoryRef = inventory;     // インベントリ操作用に参照を保持
        m_onClickCallback = onClick;

        m_icon.sprite = item.m_ore.m_oreIcon;   // 鉱石のアイコンを設定
        m_icon.enabled = true;  // アイコン画像を表示

        m_quantityText.text = item.m_quantity.ToString();   // 所持数を表示
        m_quantityText.enabled = true;

        //m_sellCountInput.text = "";
        //m_sellCountInput.gameObject.SetActive(true);
        //m_sellButton.gameObject.SetActive(true);

        //UpdateSellUIVisibility(Merchant.Instance.IsTalking());

        m_removeButton.SetActive(true); // 削除ボタンを表示
    }

    public void OnClick()
    {
        if (m_onClickCallback != null && m_item != null)
        {
            m_onClickCallback.Invoke(m_item);
        }
    }

    // スロットを空にし、UI表示をリセットする
    public void ClearOre()
    {
        m_item = null;
        m_inventoryRef = null;
        m_onClickCallback = null;

        m_icon.sprite = null;   
        m_icon.enabled = false;

        m_quantityText.text = "";
        m_quantityText.enabled = false; // 所持数テキストを消す

        //m_sellCountInput.text = "";
        //m_sellCountInput.gameObject.SetActive(false);
        //m_sellButton.gameObject.SetActive(false);
        m_removeButton.SetActive(false);    // 削除ボタンを非表示
    }

 //   public void OnSellAmount()
 //   {
 //       if (m_item == null || m_inventoryRef == null) return;

 //       // 商人の範囲内でなければ売れない
 //       if (!Merchant.Instance.CanSell())
 //       {
 //           Debug.Log("商人の近くでしか売れません！");
 //           return;
 //       }

 //       if (!int.TryParse(m_sellCountInput.text, out int count)) return;

 //       count = Mathf.Clamp(count, 1, m_item.m_quantity);
 //       int total = count * m_item.m_ore.m_value;

 //       GameManager.Instance.AddMoney(total);
 //       m_inventoryRef.Remove(m_item.m_ore, count);
 //   }

	//private void UpdateSellUIVisibility(bool canSell)
	//{
	//	bool visible = (m_item != null && canSell);
	//	m_sellCountInput.gameObject.SetActive(visible);
	//	m_sellButton.gameObject.SetActive(visible);
	//}

	// 削除ボタンが押されたときに呼ばれる
	public void OnRemoveButton()
    {
        // アイテム情報とインベントリ参照があれば削除処理を呼ぶ
        if (m_item != null && m_inventoryRef != null)
        {
            m_inventoryRef.Remove(m_item.m_ore);
        }
    }
}
