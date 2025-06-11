using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreSpawner : MonoBehaviour
{
    [SerializeField] OreData m_oreData;
	[SerializeField] int m_spawnCount;
	[SerializeField] Vector3 m_spawnSize;
	[SerializeField] LayerMask m_groundLayer;

	private void Start()
	{
		SpawnOre();
	}

	// 指定された数の鉱石を生成
	private void SpawnOre()
	{
		for (int i = 0; i < m_spawnCount; i++)
		{
			// 生成範囲のランダムなXZ座標を取得
			float x = Random.Range(-m_spawnSize.x / 2f, m_spawnSize.x / 2f);
			float z = Random.Range(-m_spawnSize.z / 2f, m_spawnSize.z / 2f);
			Vector3 origin = new Vector3(x, 100f, z);	// 高さ100から真下にRayを飛ばす

			// Raycastで地面に当たった位置を取得
			if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, 200f, m_groundLayer))
			{
				// 地形の表面位置にスポーン
				Vector3 spawnPos = hit.point;

				// 鉱石データからランダムに1つ選択
				OreSetting setting = PickRandomOreSetting();

				// プレハブを生成し、Oreスクリプトがあるなら初期化
				var oreSpawn = Instantiate(setting.m_orePrefab, spawnPos, Quaternion.identity);
				if (oreSpawn.TryGetComponent<Ore>(out var ore))
				{
					ore.Initialize(setting);	// 耐久や名前などを設定
				}
			}
		}
	}

	// 生成確率に基づいてランダムに1つのOreSettingを選ぶ
	private OreSetting PickRandomOreSetting()
	{
		float totalProbability = 0f;

		// 確率の合計を計算
		foreach (var setting in m_oreData.m_oreList)
		{
			totalProbability += setting.m_spawnProbability;
		}

		// 合計確率の範囲内でランダムな値を生成
		float randomPoint = Random.Range(0f, totalProbability);
		float currentSum = 0f;

        // 各鉱石の確率を順に足していき、ランダム値がその範囲内にあるかをチェック
        foreach (var setting in m_oreData.m_oreList)
		{
			currentSum += setting.m_spawnProbability;

            // ランダム値がこの鉱石の範囲に入ったら、それを選ぶ
            if (randomPoint <= currentSum)
			{
				return setting;
			}
		}

		// 万が一見つからなかった場合は最後の鉱石を返す
		return m_oreData.m_oreList[m_oreData.m_oreList.Count - 1];
	}
}
