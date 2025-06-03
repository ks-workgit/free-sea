using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMining : MonoBehaviour
{
    [SerializeField] Collider m_collider;
	[SerializeField] Transform m_currentOre;
	[SerializeField] float m_rotationSpeed;

	bool m_isRotating;

	Quaternion m_targetRotation;

	private void Start()
	{
		m_currentOre = null;
		m_isRotating = false;
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0)&& m_currentOre != null)
		{
			FaceOre();
			Debug.Log("Œ@‚é");
		}

		// ‰ñ“]’†‚Íí‚ÉŒü‚«‚ğXV
		if (m_isRotating)
		{
			transform.rotation = Quaternion.Slerp(transform.rotation, m_targetRotation, m_rotationSpeed * Time.deltaTime);

			// \•ª‰ñ“]‚µ‚½‚ç~‚ß‚é
			if(Quaternion.Angle(transform.rotation, m_targetRotation) < 1f)
			{
				transform.rotation = m_targetRotation;
				m_isRotating = false;
			}
		}
	}

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

	public void MiningStart()
    {
        m_collider.enabled = true;
    }

	public void MiningEnd()
    {
        m_collider.enabled = false;
    }

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Item"))
		{
			m_currentOre = other.transform;
			Debug.Log("zÎ”­Œ©I");
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if(other.CompareTag("Item"))
		{
			if(m_currentOre == other.transform)
			{
				m_currentOre = null;
				Debug.Log("Œ©¸‚Á‚½");
			}
		}
	}
}
