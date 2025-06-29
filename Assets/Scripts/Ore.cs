using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ore : MonoBehaviour
{
	private OreSetting m_oreSetting;	// 鉱石の設定データ
	private int m_currentDurability;    // 現在の耐久値
    [SerializeField] AudioClip m_clip;
    [SerializeField] Inventory m_inventory;

    [SerializeField] float m_respawnDelay = 30f; // 復活までの秒数
    private OreSpawnPoint m_originSpawnPoint;
    private List<OreSetting> m_candidateList;

    // 耐久値を表示する用
    [SerializeField] GameObject m_healthBarPrefab;
	private Slider m_healthBarSlider;
	private Canvas m_healthCanvas;

    private Outline m_outline;

	private void Start()
	{
        m_outline = GetComponent<Outline>();
		m_outline.enabled = false;
	}

    // 鉱石に必要な情報を渡して初期化する
    public void Initialize(OreSetting setting, OreSpawnPoint spawnPoint, List<OreSetting> candidates, Inventory inventory)
	{
		m_oreSetting = setting;
        m_originSpawnPoint = spawnPoint;
        m_candidateList = candidates;
        m_inventory = inventory;
		m_currentDurability = m_oreSetting.m_durability;

		// HPバー生成
		if (m_healthBarPrefab != null)
		{
			GameObject barObj = Instantiate(m_healthBarPrefab, transform);
			m_healthCanvas = barObj.GetComponentInChildren<Canvas>();
			m_healthBarSlider = barObj.GetComponentInChildren<Slider>();
			m_healthCanvas.enabled = false;

            // スライダー初期化
            m_healthBarSlider.maxValue = m_oreSetting.m_durability;
			m_healthBarSlider.value = m_currentDurability;

			// HPバーの位置を調整
			barObj.transform.localPosition = new Vector3(0, 1.4f, 0);	// 鉱石の上に表示
		}

		Debug.Log($"{m_oreSetting.m_oreName} 耐久: {m_currentDurability}/{m_oreSetting.m_durability}");
    }

	// 指定されたダメージ分だけ耐久値を減らす
	public void Mine(int damage)
    {
		m_currentDurability -= damage;
		m_currentDurability = Mathf.Max(0, m_currentDurability);

		// UI更新
		if (m_healthBarSlider != null)
		{
			m_healthBarSlider.value = m_currentDurability;
		}

		Debug.Log($"{m_oreSetting.m_oreName} に {damage} ダメージ！ 残り耐久: {m_currentDurability}");

		if (m_currentDurability <= 0)
		{
			// ランダムにドロップ数を決定
			int dropCount = Random.Range(m_oreSetting.m_minDrop, m_oreSetting.m_maxDrop + 1);

			// 決定した個数分のアイテムをインベントリに追加
			for (int i = 0; i < dropCount; i++)
			{
				m_inventory.Add(m_oreSetting);
			}

            // Spawner に再スポーンを依頼
            OreSpawner spawner = FindObjectOfType<OreSpawner>();
            spawner.RespawnOreDelayed(m_originSpawnPoint, m_candidateList, m_respawnDelay);

            Debug.Log($"{m_oreSetting.m_oreName} を {dropCount}個ドロップ");
            AudioSource.PlayClipAtPoint(m_clip, transform.position);
            Destroy(gameObject);
        }
    }

    private void LateUpdate()
	{
		if (m_healthCanvas != null)
		{
			// 耐久値バーをカメラの方向に向ける
			m_healthCanvas.transform.rotation = Camera.main.transform.rotation;
		}
	}

	public void SetInventory(Inventory inventory)
	{
		m_inventory = inventory;
	}

	// 耐久値バーを表示
	public void CanvasAttach()
	{
		m_healthCanvas.enabled = true;
	}

	// 耐久値バーを非表示
	public void CanvasRemove()
	{
		m_healthCanvas.enabled = false;
	}

	// アウトラインを表示
    public void OutlineAttach()
	{
		m_outline.enabled = true;
	}

	// アウトラインを非表示
	public void OutlineRemove()
	{
		m_outline.enabled = false;
	}

	public bool OreDurability()
	{
		return (m_currentDurability - 1) < 0;
	}
}
