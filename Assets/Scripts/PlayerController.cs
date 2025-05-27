using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] float m_moveSpeed;

	Vector3 m_direction;
	Vector3 m_velocity;
	Rigidbody m_rigidBody;

	bool m_canMove;

	private void Awake()
	{
		m_rigidBody = GetComponent<Rigidbody>();
	}

	private void Start()
	{
		m_direction = new Vector3(0, 0, 0);
		m_velocity = new Vector3(0, 0, 0);
		m_canMove = true;
	}

	private void Update()
	{
		ProcessInput();
	}

	private void FixedUpdate()
	{
		MovePlayer();
	}

	private void ProcessInput()
	{
		var x = Input.GetAxis("Horizontal");
		var y = Input.GetAxis("Vertical");
		m_direction = new Vector3 (x, 0, y);
	}

	// プレイヤーの動き
	private void MovePlayer()
	{
		// カメラの正面ベクトルを作成
		Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

		// カメラの向きに応じた移動量
		m_velocity = cameraForward * m_direction.z + Camera.main.transform.right * m_direction.x;
		m_velocity *= m_moveSpeed;

		// 進行方向に向きを変える
		if (m_velocity != Vector3.zero)
		{
			transform.rotation = Quaternion.Slerp(transform.rotation,
				Quaternion.LookRotation(m_velocity.normalized), 0.3f);
		}

		// 移動
		if (m_canMove)
		{
			// 重力による落下を保持
			m_velocity.y = m_rigidBody.velocity.y;

			m_rigidBody.velocity = m_velocity;
		}
	}
}
