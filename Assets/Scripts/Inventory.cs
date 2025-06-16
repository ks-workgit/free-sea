using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
	[SerializeField] GameObject m_inventory;
	private bool m_isOpen;

	private void Start()
	{
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
