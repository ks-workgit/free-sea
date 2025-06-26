using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    { 
        // 安全に初期化（非アクティブだとスロットが取れないケース対策）
        if (m_playerInventoryPanel != null)
        {
            m_playerInventoryPanel.SetActive(true);
            m_playerInventoryUI.UpdateUI(m_playerInventory);
            m_playerInventoryPanel.SetActive(false);
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
                m_merchantInventoryUI.Initialize(m_playerInventory, m_playerInventoryUI);

                // 商人 UI 用にコールバック付きで表示
                m_playerInventoryUI.UpdateUI(m_playerInventory, m_merchantInventoryUI.SelectOre);
            });
        }
    }

    private void ToggleInventory(GameObject panel, System.Action onOpen)
    {
        if (panel == null) return;

        bool isActive = !panel.activeSelf;
        panel.SetActive(isActive);

        if (isActive)
        {
            onOpen?.Invoke(); // UIが開かれたときに内容を更新
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}

