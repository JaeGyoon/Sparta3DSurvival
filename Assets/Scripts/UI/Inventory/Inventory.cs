using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public ItemSlot[] slots;

    public GameObject inventoryPanel;
    public Transform slotGridLayout;

    [Header("Selected Item")]
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;
    public TextMeshProUGUI statName;
    public TextMeshProUGUI statValue;

    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unequipButton;
    public GameObject dropButton;

    private PlayerController playerController;
    private PlayerCondition playerCondition;

    public Transform itemDropPoint;

    private ItemData seletedItemData;
    private int seletedItemIndex;
    // Start is called before the first frame update
    void Start()
    {
        playerController = CharacterManager.Instance.Player.controller;
        playerCondition = CharacterManager.Instance.Player.playerCondition;

        inventoryPanel.SetActive(false);
        slots = new ItemSlot[slotGridLayout.childCount];

        itemDropPoint = CharacterManager.Instance.Player.itemDropPoint;

        for ( int i = 0; i < slots.Length; i++ )
        {
            slots[i] = slotGridLayout.GetChild(i).GetComponent<ItemSlot>();
            slots[i].slotIndex = i;
            slots[i].inventory = this;

        }

        ClearDescription();

        playerController.inventory += Toggle;
        CharacterManager.Instance.Player.addItem += AddItem;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ClearDescription()
    {
        itemName.text = string.Empty;
        itemDescription.text = string.Empty;
        statName.text = string.Empty;
        statValue.text = string.Empty;

        useButton.SetActive(false);
        dropButton.SetActive(false);
    }

    public bool isOpen()
    {
        return inventoryPanel.activeInHierarchy;
    }

    public void Toggle()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeInHierarchy);
    }

    public void AddItem()
    {
        // ��ȣ�ۿ����� ������ ������ ������ ������
        ItemData itemData = CharacterManager.Instance.Player.itemData;

        // �������� �ߺ� �������� Ȯ��
        if ( itemData.canStack)
        {
            // �������� �ߺ� �����ϰ�, �ִ� �ߺ��� �ƴ϶�� �ش� ���Կ� ��ƼƼ �߰�
            ItemSlot slot = GetMaxStackCehck(itemData);

            if (slot != null)
            {
                slot.quantity++;

                InventoryRefresh();
                CharacterManager.Instance.Player.itemData = null;
                return;
            }
        }

        // �ߺ��� �ȵǰų�, �ִ� �����̶�� ����ִ� ���� Ȯ��
        ItemSlot emptySlot = EmptySlotCheck();

        // �ִٸ� �־���
        if (emptySlot != null)
        {
            emptySlot.itemData = itemData;
            emptySlot.quantity = 1;

            InventoryRefresh();
            CharacterManager.Instance.Player.itemData = null;
            return;
        }
        

        // ���ٸ� �����
        DropItem(itemData);

        // ������ �ʱ�ȭ
        CharacterManager.Instance.Player.itemData = null;
    }

    public ItemSlot GetMaxStackCehck(ItemData itemData)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == itemData && slots[i].quantity < itemData.maxStack)
            {
                return slots[i];
            }
        }
                
        return null;
    }

    public ItemSlot EmptySlotCheck()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].itemData == null)
            {
                return slots[i];
            }
        }

        return null;
    }

    public void InventoryRefresh()
    {
        for ( int i = 0; i< slots.Length; i++)
        {
            if (slots[i].itemData != null )
            {
                slots[i].Set();
            }
            else
            {
                slots[i].Clear();
            }
        }
    }

    void DropItem(ItemData itemData)
    {
        Instantiate(itemData.itemObject, itemDropPoint.position, Quaternion.identity);
    }

    public void OnClickSlot(int slotIndex)
    {
        if (slots[slotIndex].itemData == null)
        {
            return;
        }

        seletedItemData = slots[slotIndex].itemData;
        seletedItemIndex = slotIndex;

        itemName.text = seletedItemData.itemName;
        itemDescription.text = seletedItemData.itemDescription;

        statName.text = string.Empty;
        statValue.text = string.Empty;

        for ( int i = 0; i < seletedItemData.consumables.Length; i++)
        {
            statName.text += seletedItemData.consumables[i].type.ToString() + "\n";
            statValue.text += seletedItemData.consumables[i].value.ToString() + "\n";
        }

        useButton.SetActive(seletedItemData.itemType == ItemType.Consumable);
        equipButton.SetActive(seletedItemData.itemType == ItemType.Equipable && slots[seletedItemIndex].equipped == false);
        unequipButton.SetActive(seletedItemData.itemType == ItemType.Equipable && slots[seletedItemIndex].equipped == true);


        dropButton.SetActive(true);
    }

    public void OnUseButton()
    {
        if ( seletedItemData.itemType == ItemType.Consumable)
        {
            for ( int i = 0; i < seletedItemData.consumables.Length; i++)
            {
                switch(seletedItemData.consumables[i].type)
                {
                    case ConsumableType.Health:
                        playerCondition.Heal(seletedItemData.consumables[i].value);
                        break;
                    case ConsumableType.Hunger:
                        playerCondition.Eat(seletedItemData.consumables[i].value);
                        break;
                }
            }
            RemoveSeletedItemData();
        }
    }

    public void OnDropButton()
    {
        DropItem(seletedItemData);
        RemoveSeletedItemData();
    }

    void RemoveSeletedItemData()
    {
        slots[seletedItemIndex].quantity--;

        if (slots[seletedItemIndex].quantity <= 0)
        {
            seletedItemData = null;
            slots[seletedItemIndex].itemData = null;
            seletedItemIndex = -1;
            
            
            ClearDescription();
        }

        InventoryRefresh();
    }
}
