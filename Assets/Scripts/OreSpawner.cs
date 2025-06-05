using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreSpawner : MonoBehaviour
{
    public OreData[] m_availableOres;

	private void Start()
	{
		SpawnOre();
	}

	private void SpawnOre()
	{
		float roll = Random.Range(0f, 1f);
		float cumulative = 0f;

		foreach (OreData ore in m_availableOres)
		{
			cumulative += ore.m_spawnProbability;

			if (roll <= cumulative)
			{
				Instantiate(ore.m_orePrefab, transform.position, Quaternion.identity);
				break;
			}
		}
	}
}
