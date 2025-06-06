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

	private void SpawnOre()
	{
		for (int i = 0; i < m_spawnCount; i++)
		{
			float x = Random.Range(-m_spawnSize.x / 2f, m_spawnSize.x / 2f);
			float z = Random.Range(-m_spawnSize.z / 2f, m_spawnSize.z / 2f);
			Vector3 origin = new Vector3(x, 100f, z);

			if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, 200f, m_groundLayer))
			{
				// 地形の表面位置にスポーン
				Vector3 spawnPos = hit.point;

				int index = Random.Range(0, m_oreData.m_oreList.Count);
				OreSetting setting = m_oreData.m_oreList[index];

				var oreGo = Instantiate(setting.m_orePrefab, spawnPos, Quaternion.identity);
				if (oreGo.TryGetComponent<Ore>(out var ore))
				{
					ore.Initialize(setting);
				}
			}
		}
	}
}
