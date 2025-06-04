using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMining : MonoBehaviour
{
	[SerializeField] GameObject m_tool;
    [SerializeField] Collider m_collider;
	[SerializeField] float m_rotationSpeed;
	[SerializeField] float m_miningInterval;
	[SerializeField] int m_miningPower;
	[SerializeField] Transform m_currentOre;

	PlayerController m_playerController;
	Animator m_animator;
	[SerializeField] string m_mineTriggerName;
	Quaternion m_targetRotation;
	Ore m_ore;
	Ore m_lockedOre;

	//bool m_canMine;
	bool m_isRotating;
	bool m_isMining;

	Coroutine m_miningCoroutine;

	private void Awake()
	{
		m_playerController = GetComponent<PlayerController>();
		m_animator = GetComponent<Animator>();
	}

	private void Start()
	{
		m_tool.SetActive(false);
		m_currentOre = null;
		//m_canMine = true;
		m_isRotating = false;
		m_isMining = false;
	}

	private void Update()
	{
		// 採掘アニメーションを再生していない時かつ鉱石を検知している時
		if (m_ore != null)
		{
			m_lockedOre = m_ore;    // 採掘対象を固定して保持

			if (Input.GetMouseButtonDown(0))
			{
				StartMining();
			}

			if (Input.GetMouseButtonUp(0))
			{
				StopMining();
			}

			if (Input.GetMouseButton(0))
			{
				//m_playerController.StopMove();
			}
			
			//m_animator.SetTrigger("Mining");
		}
		else
		{
			StopMining();
		}

		// 回転中は常に向きを更新
		if (m_isRotating)
		{
			transform.rotation = Quaternion.Slerp(transform.rotation, m_targetRotation, m_rotationSpeed * Time.deltaTime);

			// 十分回転したら止める
			if (Quaternion.Angle(transform.rotation, m_targetRotation) < 1f)
			{
				transform.rotation = m_targetRotation;
				m_isRotating = false;
			}
		}
	}

	private void FaceOre()
	{
		// 鉱石のベクトルを計算
		Vector3 direction = m_currentOre.position - transform.position;
		direction.y = 0f;   // 上下方向の回転は無視する

		// 鉱石の方向に向く
		if (direction != Vector3.zero)
		{
			m_targetRotation = Quaternion.LookRotation(direction);
			m_isRotating = true;
		}
	}

	// 掘るアニメーションが始まったら呼ばれる
	public void MiningStart()
	{
		// ツールを表示
		m_tool.SetActive(true);
		//m_canMine = false;
	}

	// 鉱石にピッケルが当たった時
	public void MiningHit()
	{
		if (m_lockedOre != null)
		{
			// 保持した鉱石を掘る
			m_lockedOre.Mine(m_miningPower);
		}
	}

	// 掘るアニメーション終わったら呼ばれる
	public void MiningEnd()
	{		
		m_lockedOre = null;
		// ツールを非表示
		m_tool.SetActive(false);
		//m_canMine = true;
		m_playerController.StartMove();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Ore"))
		{
			// 鉱石の情報を取得
			Ore ore = other.GetComponent<Ore>();
			m_currentOre = other.transform;

			// 鉱石が範囲内にある時
			if (ore != null )
			{
				m_ore = ore;
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Ore"))
		{
			Ore ore = other.GetComponent<Ore>();

			// すでに鉱石の位置情報を持っていた場合
			if (m_currentOre == other.transform)
			{
				m_currentOre = null;
			}
			// すでに鉱石の情報を持っていた場合
			if (ore == m_ore)
			{
				m_ore = null;
			}
		}
	}

	private void StartMining()
	{
		if (!m_isMining && m_lockedOre != null)
		{
			m_miningCoroutine = StartCoroutine(MiningLoop());
			m_isMining = true;
		}
	}

	private void StopMining()
	{
		if (m_isMining)
		{
			StopCoroutine(m_miningCoroutine);
			m_isMining = false;
		}
	}

	private IEnumerator MiningLoop()
	{
		while (true)
		{
			if (m_lockedOre == null)
			{
				StopMining();
				yield break;
			}

			FaceOre();
			m_playerController.StopMove();

			if (m_lockedOre != null)
			{
				m_animator.SetTrigger(m_mineTriggerName);
			}

			yield return new WaitForSeconds(m_miningInterval);
		}
	}
}
