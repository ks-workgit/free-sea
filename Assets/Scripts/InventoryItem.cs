using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    public OreSetting m_ore;
    public int m_quantity;

    public InventoryItem(OreSetting ore, int quantity)
    {
        m_ore = ore;
        m_quantity = quantity;
    }
}
