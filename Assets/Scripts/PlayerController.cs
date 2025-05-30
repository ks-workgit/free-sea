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
	bool m_isMoving;

	float m_idleTimer;
	float m_idleThreshold;

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
		m_idleTimer = 0f;
		m_idleThreshold = 5f;

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	private void Update()
	{
		ProcessInput();

		// �ҋ@���[�V����
		if (!m_isMoving)
		{
			m_idleTimer += Time.deltaTime;

			if (m_idleTimer >= m_idleThreshold )
			{
				int variant = Random.Range(0, 2);

				if (variant == 0)
				{
					m_animator.SetTrigger("IdleGlove");
				}
				else
				{
					m_animator.SetTrigger("IdleFan");
				}

				m_idleTimer = 0f;
				m_idleThreshold = Random.Range(5f, 10f);	// ���̕ω��̃^�C�~���O
			}
		}
		else
		{
			m_idleTimer = 0f;	// �����Ă��鎞�̓��Z�b�g
		}
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

		float x = Input.GetAxisRaw("Horizontal");
		float y = Input.GetAxisRaw("Vertical");
		m_direction = new Vector3 (x, 0, y);
		m_direction = m_direction.normalized;

		// �ړ��������f
		m_isMoving = m_direction != Vector3.zero;

		// �ړ����Ă��鎞�̃A�j���[�V����
		m_animator.SetBool("Run", m_isMoving && !m_isWalkMode);
		m_animator.SetBool("Walk", m_isMoving && m_isWalkMode);

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
