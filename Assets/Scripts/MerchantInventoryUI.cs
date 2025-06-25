using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MerchantInventoryUI : MonoBehaviour
{
    [SerializeField] Transform m_slotParent;
    [SerializeField] GameObject m_slotPrefab;

    [SerializeField] Image m_selectedIcon;
    [SerializeField] TMP_Text m_ownedText;
    [SerializeField] TMP_InputField m_sellCountInput;
    [SerializeField] Button m_sellButton;

    private Inventory m_inventory;
    private InventoryItem m_selectedItem;

	public void Initialize(Inventory inventory)
	{
		m_inventory = inventory;
        UpdateUI();
        ClearSelection();
	}

    public void UpdateUI()
    {
        foreach (Transform child in m_slotParent)
        {
            Destroy(child.gameObject);
		}

        foreach (var item in m_inventory.GetOreList())
        {
			var obj = Instantiate(m_slotPrefab, m_slotParent);
			var slot = obj.GetComponent<Slot>();

            slot.AddOre(item, m_inventory, OnSlotSelected);
		}
    }

    private void OnSlotSelected(InventoryItem item)
    {
        m_selectedItem = item;
        m_selectedIcon.sprite = item.m_ore.m_oreIcon;
        m_selectedIcon.enabled = true;

        m_ownedText.text = $"èäéùêî: {item.m_quantity}";
        m_sellCountInput.text = "1";
        m_sellCountInput.interactable = true;
        m_sellButton.interactable = true;
	}

    public void OnClickSell()
    {
        if (m_selectedItem == null) return;
        if (!int.TryParse(m_sellCountInput.text, out int count)) return;

		count = Mathf.Clamp(count, 1, m_selectedItem.m_quantity);
		GameManager.Instance.AddMoney(count * m_selectedItem.m_ore.m_value);

        m_inventory.Remove(m_selectedItem.m_ore, count);
		UpdateUI();
		ClearSelection();
	}

    private void ClearSelection()
    {
        m_selectedItem = null;
        m_selectedIcon.enabled = false;
        m_ownedText.text = "";
        m_sellCountInput.text = "";
        m_sellCountInput.interactable= false;
        m_sellButton.interactable = false;
    }
}
