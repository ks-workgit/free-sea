using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : MonoBehaviour
{
    [SerializeField] int m_health;

    public void Mine(int damage)
    {
        m_health -= damage;

        if (m_health <= 0)
		{
			Destroy(gameObject);
		}
    }
}
