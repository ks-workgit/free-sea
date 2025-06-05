using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Ore Setting", fileName = "OreData")]
public class OreData : ScriptableObject
{
    public string m_oreName;
    public int m_durability;
	[Range(0f, 1f)]
	public float m_spawnProbability;
	public GameObject m_orePrefab;
}
