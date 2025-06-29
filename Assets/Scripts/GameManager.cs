using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // シングルトンインスタンス
    public static GameManager Instance { get; private set; }

    public bool IsUIOpen { get; private set; } = false;

    // 所持金
    public int Money = 0;

    public List<InventoryItem> savedInventory = new List<InventoryItem>();

	private void Awake()
	{
        // すでにインスタンスが存在していたら自分を破棄
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;    // 自分を唯一のインスタンスとして保持
        DontDestroyOnLoad(gameObject);  // シーンを跨いでも破棄されないようにする
    }

    public void SetUIOpen(bool isOpen)
    {
        IsUIOpen = isOpen;

        Cursor.visible = isOpen;
        Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void SaveInventory(List<InventoryItem> inventory)
    {
        savedInventory = new List<InventoryItem>(inventory);
    }

    public List<InventoryItem> LoadInventory()
    {
        return new List<InventoryItem>(savedInventory);
    }

    // お金を増やす
    public void AddMoney(int amount)
    {
        Money += amount;
        Debug.Log($"所持金: {Money}G");
    }

    public int GetMoney()
    {
        return Money;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void ReturnToTitle()
    {
        SceneManager.LoadScene("Title");
    }
}
