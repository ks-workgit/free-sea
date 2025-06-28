using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    [SerializeField] Inventory m_playerInventory;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void TitleButton()
    {
        GameManager.Instance.SaveInventory(m_playerInventory.GetOreList());
        GameManager.Instance.ReturnToTitle();
    }
}
