using System.Collections;
using System.Collections.Generic;
using System;
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
	public void Setup(InventoryItem item, Inventory inventory, Action<InventoryItem> onClick = null)
    {
        m_item = item;
        m_inventoryRef = inventory;

        // アイコンと数量表示
        m_icon.sprite = item.m_ore.m_oreIcon;
        m_icon.enabled = true;

        m_quantityText.text = item.m_quantity.ToString();
        m_quantityText.enabled = true;

        // 削除ボタンを表示（プレイヤーインベントリとして使用する場合）
        m_removeButton?.SetActive(true);

        // スロット自体にクリックイベントを設定（商人UI用など）
        Button btn = GetComponentInChildren<Button>();
        if (btn != null)
        {
            btn.onClick.RemoveAllListeners();

            if (onClick != null)
            {
                btn.onClick.AddListener(() => onClick.Invoke(item));
            }
            else
            {
                btn.onClick.AddListener(() => { }); // デフォルト動作として削除
            }
        }
    }

    // スロットを空にし、UI表示をリセットする
    public void ClearOre()
    {
        m_item = null;
        m_inventoryRef = null;

        m_icon.sprite = null;
        m_icon.enabled = false;

        m_quantityText.text = "";
        m_quantityText.enabled = false;

        m_removeButton?.SetActive(false);

        // クリックイベント解除
        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.RemoveAllListeners();
        }
    }

	// 削除ボタンが押されたときに呼ばれる
	public void OnRemoveButton()
    {
        // アイテム情報とインベントリ参照があれば削除処理を呼ぶ
        if (m_item != null && m_inventoryRef != null)
        {
            m_inventoryRef.Remove(m_item.m_ore, 1);
        }
    }
}
