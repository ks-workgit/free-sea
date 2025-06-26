using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
	[SerializeField] GameObject m_inventoryPanel;
    [SerializeField] private InventoryUI m_inventoryUI;

	// 現在の所持アイテムリスト
	private List<InventoryItem> m_oreList = new List<InventoryItem>();

    

    private void Start()
	{
        // UIコンポーネント取得
        //m_inventoryUI = GetComponent<InventoryUI>();

        // 初期状態でUI更新（空の状態）
        m_inventoryUI.UpdateUI(this);

        // ゲーム開始時にインベントリを非表示に
        if (m_inventoryPanel != null)
        {
            m_inventoryPanel.SetActive(false);
        }
    }

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.E))
		{
			bool isActive = !m_inventoryPanel.activeSelf;
			m_inventoryPanel.SetActive(isActive);

			// カーソルの表示/非表示を切り替え
			Cursor.lockState = isActive ? CursorLockMode.None : CursorLockMode.Locked;
			Cursor.visible = isActive;

            if (isActive)
            {
                m_inventoryUI.UpdateUI(this); // 開くたびにUIを更新
            }
        }
	}

	// 鉱石をインベントリに追加
	public void Add(OreSetting ore)
	{
        // インベントリ内のすべてのアイテムを1つずつチェックする
        foreach (var item in m_oreList)
		{
            // すでに同じ種類の鉱石があるかどうかをチェック
            if (item.m_ore == ore)
			{
                // 同じ鉱石が見つかったのでそのスタック数を1増やす
                item.m_quantity++;

                // UIを更新して最新状態にする
                m_inventoryUI.UpdateUI(this);
				return; // 追加処理は終わったのでメソッドを終了
            }
		}

        // 同じ鉱石が見つからなかった場合、新しくリストに追加する
        m_oreList.Add(new InventoryItem(ore, 1));
		m_inventoryUI.UpdateUI(this);   // UIを更新
    }

	// 鉱石をインベントリから1つ削除
	public void Remove(OreSetting ore, int count = 1)
	{
        // インベントリ内をインデックス付きで走査する
        for (int i = 0; i < m_oreList.Count; i++)
		{
            // i番目の鉱石が、削除対象の鉱石と一致するかどうかをチェック
            if (m_oreList[i].m_ore == ore)
			{
				// 一致したらスタック数を1減らす
				m_oreList[i].m_quantity -= count;

				// 数が0以下になったらインベントリから削除
				if (m_oreList[i].m_quantity <= 0)
				{
					m_oreList.RemoveAt(i);
				}

                // 処理が完了したのでループを抜ける
                break;
			}
		}

        // UIを更新して最新状態に反映
        m_inventoryUI.UpdateUI(this);
	}

	// 現在の所持鉱石の一覧を返す
	public List<InventoryItem> GetOreList()
	{
		return m_oreList;
	}

    public int GetQuantity(OreSetting ore)
    {
        foreach (var item in m_oreList)
        {
            if (item.m_ore == ore)
            {
                return item.m_quantity;
            }
        }
        return 0;
    }

    public bool HasEnough(OreSetting ore, int amount)
    {
        return GetQuantity(ore) >= amount;
    }
}
