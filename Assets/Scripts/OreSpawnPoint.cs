using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreSpawnPoint : MonoBehaviour
{
    [Tooltip("このポイントの周囲に生成する鉱石の数")]
    public int m_spawnCount = 10;

    [Tooltip("鉱石を密集させる範囲の半径")]
    public float m_spawnRadius = 5f;

    [Tooltip("この地点で生成するOreTypeを指定")]
    public OreType[] m_allowedTypes;

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, m_spawnRadius);
    }
#endif
}
