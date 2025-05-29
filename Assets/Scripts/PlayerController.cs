using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] float m_moveSpeed;
	[SerializeField] float m_walkSpeed;
	[SerializeField] GameObject m_tool;

	Vector3 m_direction;
	Vector3 m_velocity;
	Rigidbody m_rigidBody;
	Animator m_animator;

	bool m_canMove;
	bool m_canMine;
	bool m_isWalkMode;

	private void Awake()
	{
		m_rigidBody = GetComponent<Rigidbody>();
		m_animator = GetComponent<Animator>();
	}

	private void Start()
	{
		m_direction = new Vector3(0, 0, 0);
		m_velocity = new Vector3(0, 0, 0);
		m_canMove = true;
		m_canMine = true;
		m_isWalkMode = false;
		m_tool.SetActive(false);

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
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
		// ����/����̐؂�ւ�
		if (Input.GetKeyUp(KeyCode.LeftControl))
		{
			m_isWalkMode = !m_isWalkMode;
		}

		var x = Input.GetAxis("Horizontal");
		var y = Input.GetAxis("Vertical");
		m_direction = new Vector3 (x, 0, y);

		// �ړ����Ă��鎞
		if (m_direction != Vector3.zero)
		{
			m_animator.SetBool("Run", true);
		}
		else
		{
			m_animator.SetBool("Run", false);
		}

		// �A�j���[�V�������Đ����Ă��Ȃ���
		if (m_canMine)
		{
			if (Input.GetMouseButtonDown(0))
			{
				m_animator.SetTrigger("Mining");
			}
		}
	}

	// �v���C���[�̓���
	private void MovePlayer()
	{
		// �J�����̐��ʃx�N�g�����쐬
		Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

		// �J�����̌����ɉ������ړ���
		m_velocity = cameraForward * m_direction.z + Camera.main.transform.right * m_direction.x;
		// ���[�h�؂�ւ��ɉ������ړ���
		m_velocity *= m_isWalkMode ? m_walkSpeed : m_moveSpeed;

		// �ړ�
		if (m_canMove)
		{
			// �i�s�����Ɍ�����ς���
			if (m_velocity != Vector3.zero)
			{
				transform.rotation = Quaternion.Slerp(transform.rotation,
					Quaternion.LookRotation(m_velocity.normalized), 0.3f);
			}

			// �d�͂ɂ�闎����ێ�
			m_velocity.y = m_rigidBody.velocity.y;

			m_rigidBody.velocity = m_velocity;
		}
	}

	// �A�j���[�V��������Ă΂��
	public void MiningStart()
	{
		// �c�[����\��
		m_tool.SetActive(true);
		m_canMove = false;
		m_canMine = false;
	}

	// �A�j���[�V��������Ă΂��
	public void MiningEnd()
	{
		// �c�[�����\��
		m_tool.SetActive(false);
		m_canMove = true;
		m_canMine = true;
	}
}
