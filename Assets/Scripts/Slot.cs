using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField] Image m_icon;
    [SerializeField] TMP_Text m_quantityText;
    [SerializeField] GameObject m_removeButton;
    //private OreSetting m_oreSetting;

    private InventoryItem m_item;
    private Inventory m_inventoryRef;

    public void AddOre(InventoryItem item, Inventory inventory)
    {
        m_item = item;
        m_inventoryRef = inventory;

        m_icon.sprite = item.m_ore.m_oreIcon;
        m_icon.enabled = true;

        m_quantityText.text = item.m_quantity.ToString();
        m_quantityText.enabled = true;

        m_removeButton.SetActive(true);
    }

    public void ClearOre()
    {
        m_item = null;
        m_inventoryRef = null;

        m_icon.sprite = null;
        m_icon.enabled = false;

        m_quantityText.text = "";
        m_quantityText.enabled = false;

        m_removeButton.SetActive(false);
    }

    public void OnRemoveButton()
    {
        if (m_item != null && m_inventoryRef != null)
        {
            m_inventoryRef.Remove(m_item.m_ore);
        }
    }
}
