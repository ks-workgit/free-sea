using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMining : MonoBehaviour
{
	// ƒvƒŒƒCƒ„[ŠÖ˜A
	[SerializeField] float m_rotationSpeed;
	[SerializeField] float m_miningInterval;	// Œ@‚éŠÔŠu
	[SerializeField] int m_miningPower;
	[SerializeField] string m_mineTriggerName;
	PlayerController m_playerController;
	Animator m_animator;
	Quaternion m_targetRotation;	// ÌŒ@‚ÌŒü‚«‰ñ“]
	Coroutine m_miningCoroutine;

	bool m_canMine;
	bool m_isRotating;
	bool m_isMining;
	bool m_doMineAnim;

	// ƒsƒbƒPƒ‹ŠÖ˜A
	[SerializeField] GameObject m_tool;
    [SerializeField] Collider m_collider;

	// zÎŠÖ˜A
	[SerializeField] Transform m_currentOre;
	Ore m_ore;
	Ore m_lockedOre;	// zÎ‚ğ•Û‚·‚é—p

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
		// zÎ‚ğŒŸ’m‚µ‚Ä‚¢‚é
		if (m_ore != null)
		{
			m_lockedOre = m_ore;    // ÌŒ@‘ÎÛ‚ğŒÅ’è‚µ‚Ä•Û

			// ’P‰Ÿ‚µ‚ÅƒAƒjƒ[ƒVƒ‡ƒ“‚ªÄ¶‚³‚ê‚Ä‚¢‚È‚¢
			if (Input.GetMouseButtonDown(0) && !m_doMineAnim)
			{
				StartMining();
			}
			// ‰Ÿ‚µ‚Ä‚¢‚é‚©‚ÂŒ@‚Á‚Ä‚¢‚éŠÔ‚Í“®‚«‚ğ~‚ß‚é
			if (Input.GetMouseButton(0) && m_isMining)
			{
				m_playerController.StopMove();
			}
			// —£‚³‚ê‚½‚çÌŒ@’â~
			if (Input.GetMouseButtonUp(0))
			{
				m_canMine = false;
				StopMining();
			}
		}
		else
		{
			// zÎ‚ª‚È‚­‚È‚Á‚½
			m_canMine = false;
			StopMining();
		}

		// ‰ñ“]’†‚Íí‚ÉŒü‚«‚ğXV
		if (m_isRotating)
		{
			// ‚ä‚Á‚­‚è‰ñ“]‚µ‘±‚¯‚é
			transform.rotation = Quaternion.Slerp(transform.rotation, m_targetRotation, m_rotationSpeed * Time.deltaTime);

			// \•ª‰ñ“]‚µ‚½‚ç~‚ß‚é
			if (Quaternion.Angle(transform.rotation, m_targetRotation) < 1f)
			{
				transform.rotation = m_targetRotation;
				m_isRotating = false;
			}
		}
	}

	// ƒLƒƒƒ‰‚ğzÎ‚Ì•ûŒü‚ÉŒü‚¯‚é
	private void FaceOre()
	{
		// zÎ‚ÌƒxƒNƒgƒ‹‚ğŒvZ
		Vector3 direction = m_currentOre.position - transform.position;
		direction.y = 0f;   // ã‰º•ûŒü‚Ì‰ñ“]‚Í–³‹‚·‚é

		// zÎ‚Ì•ûŒü‚ÉŒü‚­
		if (direction != Vector3.zero)
		{
			m_targetRotation = Quaternion.LookRotation(direction);
			m_isRotating = true;
		}
	}

	// zÎ‚ğŒŸ’m‚µ‚½
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Ore"))
		{
			// zÎ‚Ìî•ñ‚ğæ“¾
			Ore ore = other.GetComponent<Ore>();
			m_currentOre = other.transform;

			// zÎ‚ª”ÍˆÍ“à‚É‚ ‚é
			if (ore != null )
			{
				m_ore = ore;
				m_ore.OutlineAttach();
			}
		}
	}

	// ”ÍˆÍ‚©‚çŠO‚ê‚½
	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Ore"))
		{
			Ore ore = other.GetComponent<Ore>();

			// ‚·‚Å‚ÉzÎ‚ÌˆÊ’uî•ñ‚ğ‚Á‚Ä‚¢‚½ê‡
			if (m_currentOre == other.transform)
			{
				m_currentOre = null;
			}
			// ‚·‚Å‚ÉzÎ‚Ìî•ñ‚ğ‚Á‚Ä‚¢‚½ê‡
			if (ore == m_ore)
			{
				m_ore.OutlineRemove();
				m_ore = null;
			}
		}
	}

	// ÌŒ@ŠJn
	private void StartMining()
	{
		// zÎ‚ª‚ ‚èŒ@‚Á‚Ä‚¢‚È‚¢
		if (!m_isMining && m_lockedOre != null)
		{
			m_miningCoroutine = StartCoroutine(MiningLoop());
			m_isMining = true;
		}
	}

	// ÌŒ@’â~
	private void StopMining()
	{
		// Œ@‚Á‚Ä‚¢‚é
		if (m_isMining)
		{
			StopCoroutine(m_miningCoroutine);
			m_isMining = false;
		}
	}

	// ÌŒ@ƒ‹[ƒv
	private IEnumerator MiningLoop()
	{
		while (true)
		{
			// zÎ‚ª‚È‚­‚È‚Á‚½‚ç’†~
			if (m_lockedOre == null || !m_lockedOre.gameObject.activeInHierarchy)
			{
				StopMining();
				yield break;
			}

			FaceOre();	// zÎ‚Ì•ûŒü‚ÉŒü‚­
			m_playerController.StopMove();	// ˆÚ“®‚ğ~‚ß‚é

			// zÎ‚ª‚ ‚ê‚ÎŒ@‚è‘±‚¯‚é
			if (!m_lockedOre.GetComponent<Ore>().OreDurability() && m_lockedOre.gameObject.activeInHierarchy)
			{
				Debug.Log(1);
				m_animator.SetTrigger(m_mineTriggerName);
			}

			yield return new WaitForSeconds(m_miningInterval);
		}
	}

	// Œ@‚éƒAƒjƒ[ƒVƒ‡ƒ“‚ªn‚Ü‚Á‚½‚çŒÄ‚Î‚ê‚é
	public void MiningStart()
	{
		if (m_lockedOre == null || !m_lockedOre.gameObject.activeInHierarchy)
		{
			StopMining();
			return;
		}

		m_tool.SetActive(true); // ƒc[ƒ‹‚ğ•\¦
		m_canMine = true;
		m_doMineAnim = true;
	}

	// zÎ‚ÉƒsƒbƒPƒ‹‚ª“–‚½‚Á‚½
	public void MiningHit()
	{
		if (m_lockedOre != null)
		{
			// •Û‚µ‚½zÎ‚ğŒ@‚é
			m_lockedOre.Mine(m_miningPower);
		}
	}

	// Œ@‚éƒAƒjƒ[ƒVƒ‡ƒ“‚ªI‚í‚Á‚½‚çŒÄ‚Î‚ê‚é
	public void MiningEnd()
	{
		// zÎ‚ª‚È‚­‚È‚é‚©ƒ{ƒ^ƒ“‚ª—£‚³‚ê‚½
		if (!m_canMine)
		{
			m_lockedOre = null;
			m_tool.SetActive(false);    // ƒc[ƒ‹‚ğ”ñ•\¦
			m_playerController.StartMove();	// ˆÚ“®‚ğn‚ß‚é
		}
		m_doMineAnim = false;
	}
}
