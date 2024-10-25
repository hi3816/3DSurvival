using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;

    public GameObject inventoryWindow;
    public Transform slotPanel;
    public Transform dropPosition;

    [Header("Select Item")]
    public TextMeshProUGUI selectedItemName;        //선택된 아이템 이름
    public TextMeshProUGUI selectedItemDescription; //..설명
    public TextMeshProUGUI selectedStatName;        //..스탯이름
    public TextMeshProUGUI selectedStatValue;       //..스탯 값
    public GameObject useButton;                    //사용버튼
    public GameObject equipButton;                  //장착버튼
    public GameObject unequipeButton;               //해제버튼
    public GameObject dropButton;                   //버리기버튼

    private PlayerController controller;
    private PlayerCondition condition;

    // Start is called before the first frame update
    void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
        dropPosition = CharacterManager.Instance.Player.dropPosition;

        controller.inventory += Toggle;
        CharacterManager.Instance.Player.addItem += AddItem;

        inventoryWindow.SetActive(false);
        slots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ClearSelectedItemWindow()
    { 
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        useButton.SetActive(false);
        equipButton.SetActive(false);
        unequipeButton.SetActive(false);
        dropButton.SetActive(false);
    }

    public void Toggle()
    {
        if (IsOpen())
        {
            inventoryWindow.SetActive(false);
        }
        else 
        {
            inventoryWindow.SetActive(true);
        }
    }

    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData;

        // 아이템이 중복 가능한지 canStack
        if (data.canStack)
        {
            ItemSlot slot = GetItemStack(data);
            if (slot != null)
            {
                slot.quantity++;
                UpdateUI();
                CharacterManager.Instance.Player.itemData = null;
                return;
            }
        }
        // 비어있는 슬롯 가져온다.
        ItemSlot emptySlot = GetEmptySlot();

        //있다면
        if (emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
            return;
        }

        //없다면
        ThrowItem(data);
        CharacterManager.Instance.Player.itemData = null;
    }
    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null)
            {
                slots[i].Set();
            }
            else
            {
                slots[i].Clear();
            }
        }
    }

    ItemSlot GetItemStack(ItemData data) 
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmout)
            {
                return slots[i];
            }
        }
        return null;
    }
    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }
        return null;
    }

    void ThrowItem(ItemData data)
    {
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }
}
