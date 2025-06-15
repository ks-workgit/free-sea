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
	public string m_oreName;	// 鉱石の名前
	public int m_durability;	// 耐久値
	public int m_value;			// 価値
	[Range(0.01f, 1f)]
	public float m_spawnProbability;	// 生成確率
	public GameObject m_orePrefab;		// 生成するプレハブ
	public Sprite m_oreIcon;            // UI用のアイコン
	public OreType m_oreType;			// 鉱石の種類
}
