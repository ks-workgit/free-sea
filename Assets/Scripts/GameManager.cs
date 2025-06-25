using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // シングルトンインスタンス
    public static GameManager Instance { get; private set; }

    // 所持金
    public int Money { get; private set; } = 0;

    // ピッケルの強化レベル
    public int PickaxeLevel { get; private set; } = 0;

    // 帰還中かどうか
    public bool IsAtBase { get; private set; } = false;

	private void Awake()
	{
		// シングルトンの初期化
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
	}

    // お金を増やす
    public void AddMoney(int amount)
    {
        Money += amount;
        Debug.Log($"所持金: {Money}G");
    }

    // お金を使う
    public bool SpendMoney(int amount)
    {
        if (Money >= amount)
        {
            Money -= amount;
            Debug.Log($"購入成功！残金:{Money}G");
            return true;
        }
        else
        {
            Debug.Log("お金が足りません！");
            return false;
        }
    }

    // ピッケルのレベルを上げる
    public void UpgradePickaxe()
    {
        PickaxeLevel++;
        Debug.Log($"ピッケルがレベルアップ！現在のレベル:{PickaxeLevel}");
    }

    // 帰還状態の切り替え
    public void EnterBase()
    {
        IsAtBase = true;
        Debug.Log("拠点に帰還しました");
    }

    public void LeaveBase()
    {
        IsAtBase = false;
        Debug.Log("探索に出発しました");
    }
}
