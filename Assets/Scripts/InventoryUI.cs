using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] Transform m_inventoryPanel;
    private Slot[] slots;

	private void Awake()
	{
		slots = m_inventoryPanel.GetComponentsInChildren<Slot>();
	}

	public void UpdateUI()
	{
		if (slots == null || slots.Length == 0)
		{
			Debug.LogError("slots ‚ª‰Šú‰»‚³‚ê‚Ä‚¢‚Ü‚¹‚ñI");
			return;
		}

		for (int i = 0; i < slots.Length; i++)
		{
			if (i < Inventory.m_instance.m_oreList.Count)
			{
				slots[i].AddOre(Inventory.m_instance.m_oreList[i]);
			}
			else
			{
				slots[i].ClearOre();
			}
		}
	}
}
