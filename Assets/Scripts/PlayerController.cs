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

	// �v���C���[�̓���
	private void MovePlayer()
	{
		// �J�����̐��ʃx�N�g�����쐬
		Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

		// �J�����̌����ɉ������ړ���
		m_velocity = cameraForward * m_direction.z + Camera.main.transform.right * m_direction.x;
		m_velocity *= m_moveSpeed;

		// �i�s�����Ɍ�����ς���
		if (m_velocity != Vector3.zero)
		{
			transform.rotation = Quaternion.Slerp(transform.rotation,
				Quaternion.LookRotation(m_velocity.normalized), 0.3f);
		}

		// �ړ�
		if (m_canMove)
		{
			// �d�͂ɂ�闎����ێ�
			m_velocity.y = m_rigidBody.velocity.y;

			m_rigidBody.velocity = m_velocity;
		}
	}
}
