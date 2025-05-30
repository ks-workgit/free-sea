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

		// 待機モーション
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
				m_idleThreshold = Random.Range(5f, 10f);	// 次の変化のタイミング
			}
		}
		else
		{
			m_idleTimer = 0f;	// 動いている時はリセット
		}
	}

	private void FixedUpdate()
	{
		MovePlayer();
	}

	private void ProcessInput()
	{
		// 歩く/走るの切り替え
		if (Input.GetKeyUp(KeyCode.LeftControl))
		{
			m_isWalkMode = !m_isWalkMode;
		}

		float x = Input.GetAxisRaw("Horizontal");
		float y = Input.GetAxisRaw("Vertical");
		m_direction = new Vector3 (x, 0, y);
		m_direction = m_direction.normalized;

		// 移動中か判断
		m_isMoving = m_direction != Vector3.zero;

		// 移動している時のアニメーション
		m_animator.SetBool("Run", m_isMoving && !m_isWalkMode);
		m_animator.SetBool("Walk", m_isMoving && m_isWalkMode);

		// アニメーションを再生していない時
		if (m_canMine)
		{
			if (Input.GetMouseButtonDown(0))
			{
				m_animator.SetTrigger("Mining");
			}
		}
	}

	// プレイヤーの動き
	private void MovePlayer()
	{
		// カメラの正面ベクトルを作成
		Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

		// カメラの向きに応じた移動量
		m_velocity = cameraForward * m_direction.z + Camera.main.transform.right * m_direction.x;
		// モード切り替えに応じた移動量
		m_velocity *= m_isWalkMode ? m_walkSpeed : m_moveSpeed;

		// 移動
		if (m_canMove)
		{
			// 進行方向に向きを変える
			if (m_velocity != Vector3.zero)
			{
				transform.rotation = Quaternion.Slerp(transform.rotation,
					Quaternion.LookRotation(m_velocity.normalized), 0.3f);
			}

			// 重力による落下を保持
			m_velocity.y = m_rigidBody.velocity.y;

			m_rigidBody.velocity = m_velocity;
		}
	}

	// アニメーションから呼ばれる
	public void MiningStart()
	{
		// ツールを表示
		m_tool.SetActive(true);
		m_canMove = false;
		m_canMine = false;
	}

	// アニメーションから呼ばれる
	public void MiningEnd()
	{
		// ツールを非表示
		m_tool.SetActive(false);
		m_canMove = true;
		m_canMine = true;
	}
}
