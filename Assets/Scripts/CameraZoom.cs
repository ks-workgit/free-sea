using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] CinemachineFreeLook m_freeLookCamera;
    [SerializeField] float m_zoomSpeed = 2f;
    [SerializeField] float m_minRadius = 2f;
    [SerializeField] float m_maxRadius = 10f;

    void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput != 0f && m_freeLookCamera != null)
        {
            // FreeLookカメラのTop, Middle, Bottomの3つの軌道を処理する
            for (int i = 0; i < 3; i++)
            {
                // 入力に応じてズーム距離を計算（手前に引く方向なので「-」）
                float newRadius = m_freeLookCamera.m_Orbits[i].m_Radius - scrollInput * m_zoomSpeed;

                // ズーム距離が極端にならないように制限
                newRadius = Mathf.Clamp(newRadius, m_minRadius, m_maxRadius);

                // Orbitの情報を更新して変更を適用
                var orbit = m_freeLookCamera.m_Orbits[i];
                orbit.m_Radius = newRadius;
                m_freeLookCamera.m_Orbits[i] = orbit;
            }
        }
    }
}
