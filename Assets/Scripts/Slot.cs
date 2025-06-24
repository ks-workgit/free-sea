using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField] Image m_icon;
    [SerializeField] TMP_Text m_quantityText;   // 所持数を表示するテキスト
    [SerializeField] GameObject m_removeButton;

    private InventoryItem m_item;       // このスロットに表示されているアイテム情報
    private Inventory m_inventoryRef;   // アイテム削除などのためのインベントリ参照

    // 鉱石情報とインベントリ参照を受け取り、スロットUIを更新する
    public void AddOre(InventoryItem item, Inventory inventory)
    {
        m_item = item;      // 表示するアイテムを保持
        m_inventoryRef = inventory;     // インベントリ操作用に参照を保持

        m_icon.sprite = item.m_ore.m_oreIcon;   // 鉱石のアイコンを設定
        m_icon.enabled = true;  // アイコン画像を表示

        m_quantityText.text = item.m_quantity.ToString();   // 所持数を表示
        m_quantityText.enabled = true;

        m_removeButton.SetActive(true); // 削除ボタンを表示
    }

    // スロットを空にし、UI表示をリセットする
    public void ClearOre()
    {
        m_item = null;
        m_inventoryRef = null;

        m_icon.sprite = null;   
        m_icon.enabled = false;

        m_quantityText.text = "";
        m_quantityText.enabled = false; // 所持数テキストを消す

        m_removeButton.SetActive(false);    // 削除ボタンを非表示
    }

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
