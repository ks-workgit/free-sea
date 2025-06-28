using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class GameMenu : MonoBehaviour
{
    [SerializeField] GameObject m_menuUI;
    [SerializeField] Inventory m_playerInventory;

    private void Start()
    {
        m_menuUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool isActive = !m_menuUI.activeSelf;
            m_menuUI.SetActive(isActive);
            Cursor.visible = isActive;
            Cursor.lockState = isActive ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }

    public void TitleButton()
    {
        GameManager.Instance.SaveInventory(m_playerInventory.GetOreList());
        GameManager.Instance.ReturnToTitle();
    }
}
