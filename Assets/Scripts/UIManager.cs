using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("通常インベントリ")]
    [SerializeField] GameObject m_playerInventoryPanel;
    [SerializeField] InventoryUI m_playerInventoryUI;

    [Header("商人インベントリ")]
    [SerializeField] GameObject m_merchantPanel;
    [SerializeField] MerchantInventoryUI m_merchantInventoryUI;

    [Header("共通")]
    [SerializeField] Inventory m_playerInventory;
    [SerializeField] Merchant m_merchantTrigger;
    [SerializeField] GameObject m_inventoryBack;

    [SerializeField] TMP_Text m_moneyText;
    [SerializeField] Cinemachine.CinemachineFreeLook m_freeLookCamera;

    private void Start()
    { 
        // 安全に初期化（非アクティブだとスロットが取れないケース対策）
        if (m_playerInventoryPanel != null)
        {
            m_playerInventoryPanel.SetActive(true);
            m_playerInventoryUI.UpdateUI(m_playerInventory);
            m_playerInventoryPanel.SetActive(false);
            m_inventoryBack.SetActive(false);
            m_moneyText.gameObject.SetActive(false);
        }

        if (m_merchantPanel != null)
        {
            m_merchantPanel.SetActive(true);
            m_merchantInventoryUI.Initialize(m_playerInventory, m_playerInventoryUI);  // スロット生成など内部のAwakeが必要な場合
            m_merchantPanel.SetActive(false);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // 通常インベントリ（Eキー）
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleInventory(m_playerInventoryPanel, () => {
                m_playerInventoryUI.UpdateUI(m_playerInventory);
            });
        }

        // 商人インベントリ（Fキー）→ 商人の近くでのみ
        if (Input.GetKeyDown(KeyCode.F) && m_merchantTrigger.IsPlayerNear)
        {
            if (m_merchantInventoryUI == null)
            {
                Debug.LogError("m_merchantInventoryUI が null または破棄されています！");
                return;
            }

            ToggleInventory(m_merchantPanel, () => {
				m_playerInventoryPanel.SetActive(true);
				m_merchantInventoryUI.Initialize(m_playerInventory, m_playerInventoryUI);
				m_playerInventoryUI.UpdateUI(m_playerInventory, m_merchantInventoryUI.SelectOre);
			});
        }
    }

    private void ToggleInventory(GameObject panel, System.Action onOpen)
    {
        if (panel == null) return;

        bool isActive = !panel.activeSelf;
        panel.SetActive(isActive);
        m_inventoryBack.SetActive(isActive);
        m_moneyText.gameObject.SetActive(isActive);

        GameManager.Instance.SetUIOpen(isActive);

        if (isActive)
        {
            onOpen?.Invoke(); // UIが開かれたときに内容を更新

            m_freeLookCamera.m_XAxis.m_InputAxisName = "";
            m_freeLookCamera.m_YAxis.m_InputAxisName = "";
        }
        else
        {
            m_freeLookCamera.m_XAxis.m_InputAxisName = "Mouse X";
            m_freeLookCamera.m_YAxis.m_InputAxisName = "Mouse Y";
        }
    }
}

