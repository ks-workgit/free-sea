using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    public OreSetting m_ore;    // 鉱石の設定データ
    public int m_quantity;      // 所持している個数

    // 鉱石データと初期所持数を渡して初期化するコンストラクタ
    public InventoryItem(OreSetting ore, int quantity)
    {
        m_ore = ore;
        m_quantity = quantity;
    }
}
