using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
	[SerializeField] GameObject m_inventoryPanel;
	private InventoryUI m_inventoryUI;

	// アイテムリスト
	private List<InventoryItem> m_oreList = new List<InventoryItem>();

	private void Start()
	{
		m_inventoryUI = GetComponent<InventoryUI>();
		m_inventoryUI.UpdateUI(this);

		if (m_inventoryPanel != null)
		{
			m_inventoryPanel.SetActive(false);
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.E))
		{
			bool isActive = !m_inventoryPanel.activeSelf;
			m_inventoryPanel.SetActive(isActive);

			Cursor.lockState = isActive ? CursorLockMode.None : CursorLockMode.Locked;
			Cursor.visible = isActive;
		}
	}

	public void Add(OreSetting ore)
	{
		foreach (var item in m_oreList)
		{
			if (item.m_ore == ore)
			{
				item.m_quantity++;
				m_inventoryUI.UpdateUI(this);
				return;
			}
		}

		m_oreList.Add(new InventoryItem(ore, 1));
		m_inventoryUI.UpdateUI(this);
	}

	public void Remove(OreSetting ore)
	{
		for (int i = 0; i < m_oreList.Count; i++)
		{
			if (m_oreList[i].m_ore == ore)
			{
				m_oreList[i].m_quantity--;

				if (m_oreList[i].m_quantity <= 0)
				{
					m_oreList.RemoveAt(i);
				}

				break;
			}
		}

		m_inventoryUI.UpdateUI(this);
	}

	public List<InventoryItem> GetOreList()
	{
		return m_oreList;
	}
}
