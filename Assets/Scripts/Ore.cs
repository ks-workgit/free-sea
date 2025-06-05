using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : MonoBehaviour
{
    [SerializeField] int m_health;

    Outline m_outline;

	private void Awake()
	{
		m_outline = GetComponent<Outline>();
	}

	private void Start()
	{
		m_outline.enabled = false;
	}

	public void Mine(int damage)
    {
        m_health -= damage;

        if (m_health <= 0)
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
