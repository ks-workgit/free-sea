using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OreSpawner : MonoBehaviour
{
    [SerializeField] OreData m_oreData;
	[SerializeField] LayerMask m_groundLayer;
    [SerializeField] LayerMask m_oreLayer;
	[SerializeField] Inventory m_inventory;

	private void Start()
	{
        // シーン上にあるすべてのOreSpawnPointを取得
        var spawnPoints = FindObjectsOfType<OreSpawnPoint>();

        // 各スポーンポイントに対して処理
        foreach (var point in spawnPoints)
		{
            // そのスポーンポイントで許可された鉱石タイプのみを抽出
            var filtered = FilterOreByType(m_oreData.m_oreList, point.m_allowedTypes);

            // 対応する鉱石が1つもない場合はスキップ
            if (filtered.Count == 0)
            {
                Debug.LogWarning($"{point.name} に一致する鉱石タイプがありませんでした。");
                continue;
            }

            // 抽出した鉱石候補から指定された数だけ生成する
            SpawnOre(point.transform, point.m_spawnRadius, point.m_spawnCount, filtered);
        }
	}

    // 許可された鉱石タイプだけを抽出する
    private List<OreSetting> FilterOreByType(List<OreSetting> all, OreType[] types)
	{
        List<OreSetting> filteredList = new List<OreSetting>();

        // すべての OreSetting を1つずつチェック
        for (int i = 0; i < all.Count; i++)
        {
            OreSetting ore = all[i];

            // 許可されたタイプに含まれていれば追加
            for (int j = 0; j < types.Length; j++)
            {
                if (ore.m_oreType == types[j])
                {
                    filteredList.Add(ore);
                    break; // 一致したらこれ以上見る必要はないので終了
                }
            }
        }

        return filteredList;
    }

	// 指定された数の鉱石を生成
	private void SpawnOre(Transform centerObject, float radius, int count, List<OreSetting> candidates)
	{
		for (int i = 0; i < count; i++)
		{
            bool spawned = false;
            int attempts = 0;

            // 最大10回までリトライしてスポーンが成功するか試す
            while (!spawned && attempts < 10)
            {
                attempts++;
                
                // XZ平面上でランダムなオフセットを取得（ランダム距離を均等に分布させるためSqrtを使用）
                float angle = Random.Range(0f, 360f);
                float distance = Mathf.Sqrt(Random.Range(0f, 1f)) * radius;

                // 円周上のランダムな方向にオフセット
                Vector3 offset = new Vector3(
                    Mathf.Cos(angle * Mathf.Deg2Rad),
                    0f,
                    Mathf.Sin(angle * Mathf.Deg2Rad)
                ) * distance;

                // 高さ100から真下にRaycastを飛ばして地面を検出
                Vector3 origin = centerObject.position + offset + Vector3.up * 100f;

			    // Raycastで地面に当たった位置を取得
			    if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, 200f, m_groundLayer))
			    {
				    // 地形の表面位置にスポーン
				    Vector3 spawnPos = hit.point;

                    // 既に他の鉱石があるならスキップ
                    if (Physics.CheckSphere(spawnPos, 1.0f, m_oreLayer)) continue;

                    // 鉱石データからランダムに1つ選択
                    var setting = PickRandomOreSetting(candidates);
				    if (setting == null) continue;

				    // プレハブを生成し、Oreスクリプトがあるなら初期化
				    var oreSpawn = Instantiate(setting.m_orePrefab, spawnPos, Quaternion.identity);
				    if (oreSpawn.TryGetComponent<Ore>(out var ore))
				    {
					    ore.Initialize(setting);	// Oreにデータを渡す
                        ore.SetInventory(m_inventory);
				    }

                    spawned = true; // スポーン成功
                }
            }
		}
	}

    // 鉱石候補リストの中から、設定された出現確率に基づいてランダムに1つ選ぶ
    private OreSetting PickRandomOreSetting(List<OreSetting> candidates)
	{
        // 全候補の出現確率を合計
        float totalProbability = 0f;

		for (int i = 0; i < candidates.Count; i++)
		{
			totalProbability += candidates[i].m_spawnProbability;
		}
		
		// 候補が無い、または確率合計が0以下なら処理をスキップ
		if (totalProbability <= 0f || candidates.Count == 0) return null;

		// 0～合計確率の範囲内でランダムな値を生成
		float randomPoint = Random.Range(0f, totalProbability);
		float currentSum = 0f;

        // 各鉱石の確率を順に足していき、ランダム値が入る範囲を探す
        foreach (var setting in candidates)
		{
			currentSum += setting.m_spawnProbability;

            // ランダム値がこの鉱石の範囲に入ったら、それを選ぶ
            if (randomPoint <= currentSum)
			{
				return setting;
			}
		}

		// 万が一見つからなかった場合は最後の候補を返す
		return candidates[candidates.Count - 1];
	}
}
