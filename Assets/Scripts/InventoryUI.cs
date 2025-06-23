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

	public void UpdateUI(Inventory inventory)
	{
		if (m_slots == null || m_slots.Length == 0)
		{
			Debug.LogError("slots ‚ª‰Šú‰»‚³‚ê‚Ä‚¢‚Ü‚¹‚ñI");
			return;
		}

		var oreList = inventory.GetOreList();

		for (int i = 0; i < m_slots.Length; i++)
		{
			if (i < oreList.Count)
			{
				m_slots[i].AddOre(oreList[i], inventory);
			}
			else
			{
				m_slots[i].ClearOre();
			}
		}
	}
}
