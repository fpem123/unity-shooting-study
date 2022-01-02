using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false;  // 아이템 활성화여부

    // 필요한 컴포넌트
    [SerializeField]
    private GameObject go_InventoryBase;    // 
    [SerializeField]
    private GameObject go_SlotsParent;  // 슬롯들의 부모 그리드
    [SerializeField]
    private GameObject go_SlotToolTip;  // 슬롯 툴팁

    // 슬롯들
    private Slot[] slots;

    // 저장할 슬롯들 정보
    public Slot[] GetSlots() { return slots; }

    [SerializeField] private Item[] items;

    // 인벤토리 로드
    public void LoadToInven(int _arrayNum, string _itemName, int _itemNum)
    {
        for (int i = 0; i < items.Length; i++)
            if (items[i].itemName == _itemName)
                slots[_arrayNum].AddItem(items[i], _itemNum);
    }


    // Start is called before the first frame update
    void Start()
    {
        slots = go_SlotsParent.GetComponentsInChildren<Slot>(); // 그리드 내의 슬롯들을 가져옴
    }

    // Update is called once per frame
    void Update()
    {
        TryOpenInventory();
    }


    // 인벤토리 열기 시도
    private void TryOpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryActivated = !inventoryActivated;

            if (inventoryActivated)
                OpenInventory();
            else
                CloseInventory();
        }
    }


    // 인벤토리 오픈
    private void OpenInventory()
    {
        GameManager.isOpenInventory = true;
        go_InventoryBase.SetActive(true);
    }    
    
    
    // 인벤토리 닫기
    private void CloseInventory()
    {
        GameManager.isOpenInventory = false;
        go_InventoryBase.SetActive(false);
        go_SlotToolTip.SetActive(false);
    }


    // 아이템 습득
    public void AcquireItem(Item _item, int _count = 1)
    {
        // 아무것도 없다면 리턴
        if (_item == null)
        {
            Debug.Log("습득한 아이템이 없습니다!");

            return;
        }

        // 장비는 무조건 1칸 차지
        if (Item.ItemType.Equipment != _item.itemType)
        {
            // 이미 있다면 개수 추가
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)
                {
                    if (slots[i].item.itemName == _item.itemName)
                    {
                        slots[i].SetSlotCount(_count);
                        return;
                    }
                }
            }
        }

        // 없다면 추가
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(_item, _count);
                return;
            }
        }


    }
}
