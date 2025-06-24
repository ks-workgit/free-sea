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
		m_slots = m_inventoryPanel.GetComponentsInChildren<Slot>();
	}

	// インベントリの情報を受け取りUIスロットに表示を反映する
	public void UpdateUI(Inventory inventory)
	{
		// スロットが見つかっていないか空の場合は警告
		if (m_slots == null || m_slots.Length == 0)
		{
			Debug.LogError("slots が初期化されていません！");
			return;
		}

		// 現在の所持鉱石リストを取得
		var oreList = inventory.GetOreList();

		// スロットの数だけループしてアイテムを表示または空にする
		for (int i = 0; i < m_slots.Length; i++)
		{
			if (i < oreList.Count)
			{
				// アイテムが存在する場合はその情報をUIにセット
				m_slots[i].AddOre(oreList[i], inventory);
			}
			else
			{
				// アイテムがないスロットは空にして非表示にする
				m_slots[i].ClearOre();
			}
		}
	}
}
