using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField] Image m_icon;
    [SerializeField] GameObject m_removeButton;
    private OreSetting m_oreSetting;

    public void AddOre(OreSetting newOre)
    {
        m_oreSetting = newOre;
        m_icon.sprite = newOre.m_oreIcon;
        m_icon.enabled = true;
        m_removeButton.SetActive(true);
    }

    public void ClearOre()
    {
        m_oreSetting = null;
        m_icon.sprite = null;
        m_icon.enabled = false;
        m_removeButton.SetActive(false);
    }

    public void OnRemoveButton()
    {
        Inventory.m_instance.Remove(m_oreSetting);
    }
}
