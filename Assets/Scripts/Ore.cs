using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : MonoBehaviour
{
    //[SerializeField] int m_health;
	private OreSetting m_oreSetting;
	private int m_currentDurability;

    Outline m_outline;

	private void Awake()
	{
		m_outline = GetComponent<Outline>();
	}

	private void Start()
	{
		m_outline.enabled = false;
	}

	public void Initialize(OreSetting setting)
	{
		m_oreSetting = setting;
		m_currentDurability = m_oreSetting.m_durability;
		Debug.Log($"{m_oreSetting.m_oreName} 耐久: {m_currentDurability}/{m_oreSetting.m_durability}");
	}

	public void Mine(int damage)
    {
		m_currentDurability -= damage;
		Debug.Log($"{m_oreSetting.m_oreName} に {damage} ダメージ！ 残り耐久: {m_currentDurability}");

		if (m_currentDurability <= 0)
		{
			Destroy(gameObject);
		}
    }

	public void OutlineAttach()
	{
		m_outline.enabled = true;
	}

	public void OutlineRemove()
	{
		m_outline.enabled = false;
	}
}
