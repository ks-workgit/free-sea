using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMining : MonoBehaviour
{
	// プレイヤー関連
	[SerializeField] float m_rotationSpeed;
	[SerializeField] float m_miningInterval;	// 掘る間隔
	[SerializeField] int m_miningPower;
	[SerializeField] string m_mineTriggerName;
	private PlayerController m_playerController;
	private Animator m_animator;
	private Quaternion m_targetRotation;	// 採掘時の向き回転
	private Coroutine m_miningCoroutine;

	private bool m_canMine;
	private bool m_isRotating;
	private bool m_isMining;
	private bool m_doMineAnim;

	// ピッケル関連
	[SerializeField] GameObject m_tool;
    [SerializeField] Collider m_collider;

	// 鉱石関連
	[SerializeField] Transform m_currentOre;
	private Ore m_ore;
	private Ore m_lockedOre;	// 鉱石を保持する用

	private void Awake()
	{
		m_playerController = GetComponent<PlayerController>();
		m_animator = GetComponent<Animator>();
	}

	private void Start()
	{
		m_canMine = true;
		m_isRotating = false;
		m_isMining = false;
		m_doMineAnim = false;
		m_tool.SetActive(false);
		m_currentOre = null;
		m_ore = null;
		m_lockedOre = null;
	}

	private void Update()
	{
		// 鉱石を検知している時
		if (m_ore != null)
		{
			m_lockedOre = m_ore;    // 採掘対象を固定して保持

			// 単押しでアニメーションが再生されていない時
			if (Input.GetMouseButtonDown(0) && !m_doMineAnim)
			{
				StartMining();
			}
			// 押しているかつ掘っている間は動きを止める
			if (Input.GetMouseButton(0) && m_isMining)
			{
				m_playerController.StopMove();
			}
			// 離されたら採掘停止
			if (Input.GetMouseButtonUp(0))
			{
				m_canMine = false;
				StopMining();
			}
		}
		else
		{
			// 鉱石がなくなった時
			m_canMine = false;
			StopMining();
		}

		// 回転中は常に向きを更新
		if (m_isRotating)
		{
			// ゆっくり回転し続ける
			transform.rotation = Quaternion.Slerp(transform.rotation, m_targetRotation, m_rotationSpeed * Time.deltaTime);

			// 十分回転したら止める
			if (Quaternion.Angle(transform.rotation, m_targetRotation) < 1f)
			{
				transform.rotation = m_targetRotation;
				m_isRotating = false;
			}
		}
	}

	// キャラを鉱石の方向に向ける
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

	// 鉱石を検知した時
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Ore"))
		{
			// すでに他の鉱石に触れている場合は無視
			if (m_currentOre != null) return;

			// 鉱石の情報を取得
			Ore ore = other.GetComponent<Ore>();
			
			// 鉱石が範囲内にある時
			if (ore != null )
			{
                m_currentOre = other.transform;
                m_ore = ore;
				m_ore.CanvasAttach();	// 耐久値バーを表示
				m_ore.OutlineAttach();	// アウトラインを表示
			}
		}
	}

	// 範囲から外れた時
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
				m_ore.CanvasRemove();	// 耐久値バーを非表示
				m_ore.OutlineRemove();	// アウトラインを非表示
				m_ore = null;
			}
		}
	}

	// 採掘開始
	private void StartMining()
	{
		// 鉱石があり掘っていない時
		if (!m_isMining && m_lockedOre != null)
		{
			m_miningCoroutine = StartCoroutine(MiningLoop());
			m_isMining = true;
		}
	}

	// 採掘停止
	private void StopMining()
	{
		// 掘っている時
		if (m_isMining)
		{
			StopCoroutine(m_miningCoroutine);
			m_isMining = false;
		}
	}

	// 採掘ループ
	private IEnumerator MiningLoop()
	{
		while (true)
		{
			// 鉱石がなくなったら中止
			if (m_lockedOre == null || !m_lockedOre.gameObject.activeInHierarchy)
			{
				StopMining();
				yield break;
			}

			FaceOre();	// 鉱石の方向に向く
			m_playerController.StopMove();	// 移動を止める

			// 鉱石があれば掘り続ける
			if (!m_lockedOre.OreDurability() && m_lockedOre.gameObject.activeInHierarchy)
			{
				m_animator.SetTrigger(m_mineTriggerName);
			}

			yield return new WaitForSeconds(m_miningInterval);
		}
	}

	// 掘るアニメーションが始まったら呼ばれる
	public void MiningStart()
	{
		if (m_lockedOre == null || !m_lockedOre.gameObject.activeInHierarchy)
		{
			StopMining();
			return;
		}

		m_tool.SetActive(true); // ツールを表示
		m_canMine = true;
		m_doMineAnim = true;
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

	// 掘るアニメーションが終わったら呼ばれる
	public void MiningEnd()
	{
		// 鉱石がなくなるかボタンが離された時
		if (!m_canMine)
		{
			m_lockedOre = null;
			m_tool.SetActive(false);    // ツールを非表示
			m_playerController.StartMove();	// 移動を始める
		}
		m_doMineAnim = false;
	}
}
