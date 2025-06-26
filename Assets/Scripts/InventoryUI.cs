using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] Transform m_inventoryPanel;
    private Slot[] m_slots;

	private void Awake()
	{
		m_slots = m_inventoryPanel.GetComponentsInChildren<Slot>(true);
	}

	// インベントリの情報を受け取りUIスロットに表示を反映する
	public void UpdateUI(Inventory inventory, Action<InventoryItem> onClick = null)
	{
		// スロットが見つかっていないか空の場合は警告
		if (m_slots == null || m_slots.Length == 0)
		{
			Debug.LogError("slots が初期化されていません！");
			return;
		}

        if (inventory == null)
        {
            Debug.LogError("InventoryUI: 渡された Inventory が null です");
            return;
        }

        // 現在の所持鉱石リストを取得
        var oreList = inventory.GetOreList();

		// スロットの数だけループしてアイテムを表示または空にする
		for (int i = 0; i < m_slots.Length; i++)
		{
			if (i < oreList.Count)
			{
                //m_slots[i].gameObject.SetActive(true);
                // アイテムが存在する場合はその情報をUIにセット
                m_slots[i].Setup(oreList[i], inventory, onClick);
			}
			else
			{
				// アイテムがないスロットは空にして非表示にする
				m_slots[i].ClearOre();
                //m_slots[i].gameObject.SetActive(false);
            }
		}
	}
}
