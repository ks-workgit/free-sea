using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
	public static Inventory m_instance;
	private InventoryUI m_inventoryUI;

	// アイテムリスト
	public List<OreSetting> m_oreList = new List<OreSetting>();

	[SerializeField] GameObject m_inventory;
	private bool m_isOpen;

	private void Awake()
	{
		if (m_instance == null)
		{
			m_instance = this;
		}
	}

	private void Start()
	{
		m_inventoryUI = GetComponent<InventoryUI>();

		m_isOpen = true;
		m_inventory.SetActive(false);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.E))
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;

			m_inventory.SetActive(!m_inventory.activeSelf);

			if (!m_inventory.activeSelf)
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
		}
	}
}
