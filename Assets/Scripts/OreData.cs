using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Ore Setting", fileName = "OreData")]
public class OreData : ScriptableObject
{
	public List<OreSetting> m_oreList;
}

[Serializable]
public class OreSetting
{
	public string m_oreName;
	public int m_durability;
	public int m_value;
	[Range(0f, 1f)]
	public float m_spawnProbability;
	public GameObject m_orePrefab;
	public Sprite m_oreIcon;
}
