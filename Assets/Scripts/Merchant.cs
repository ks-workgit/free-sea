using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : MonoBehaviour
{
    public static Merchant Instance { get; private set; }

    private bool m_playerInRange = false;
	private bool m_isTalking = false;

	public delegate void OnTalkStateChanged(bool isTalking);
	public static event OnTalkStateChanged OnTalkChanged;

	[SerializeField] GameObject m_merchantPanel;
	[SerializeField] MerchantInventoryUI m_merchantUI;

	[SerializeField] Inventory m_playerInventory;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			m_playerInRange = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			m_playerInRange = false;
			SetTalkState(false);
		}
	}

	private void Update()
	{
		if (m_playerInRange && Input.GetKeyDown(KeyCode.F))
		{
			SetTalkState(!m_isTalking);

			m_merchantPanel.SetActive(true);
			m_merchantUI.Initialize(m_playerInventory);
		}
	}

	private void SetTalkState(bool isTalking)
	{
		m_isTalking = isTalking;
		OnTalkChanged?.Invoke(m_isTalking);
		Debug.Log($"è§êlÇ∆ÇÃâÔòbèÛë‘: {m_isTalking}");
	}

	public bool IsTalking()
	{
		return m_isTalking;
	}

	public bool CanSell()
	{
		return m_playerInRange;
	}
}
