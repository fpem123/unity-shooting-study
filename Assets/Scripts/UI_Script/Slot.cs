using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;     // 마우스 처리 담당


public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Item item;   // 획득한 아이템
    public int itemCount; // 아이템의 개수
    public Image itemImage; // 아이템 이미지

    // 필요한 컴퓨넌트
    [SerializeField]
    private Text text_Count;    // 텍스트
    [SerializeField]
    private GameObject go_CountImage;   // 숫자를 띄워주는 이미지

    private ItemEffectDatabase theItemEffectDatabase;
    // 슬롯의 최적화를 위해 weaponmanager를 itemdatabase에 넣음

    void Start()
    {
        // 하이어라키에 있는건 직접 넣어주는게 좋음
        theItemEffectDatabase = FindObjectOfType<ItemEffectDatabase>();
    }


    // 이미지의 투명도 변경
    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }


    // 슬롯에 아이템 추가
    public void AddItem(Item _item, int _count = 1)
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImage;

        if (item.itemType != Item.ItemType.Equipment)
        {
            go_CountImage.SetActive(true);
            text_Count.text = itemCount.ToString();
        }
        else
        {
            text_Count.text = "0";
            go_CountImage.SetActive(false);
        }

        SetColor(1);
    }


    // 슬롯의 아이템 개수를 변경
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        text_Count.text = itemCount.ToString();

        if (itemCount <= 0)
            ClearSlot();
    }


    // 슬롯 초기화
    private void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);

        text_Count.text = "0";
        go_CountImage.SetActive(false);
    }


    // 마우스 클릭 처리 핸들러 인터페이스
    // eventData == 입력된 이벤트
    public void OnPointerClick(PointerEventData eventData)
    {
        // 우클릭 이벤트 처리
        // InputButton == enum 타입
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (item != null)
            {
                theItemEffectDatabase.UseItem(item);

                // 소모품이면 감소시킴
                if (item.itemType == Item.ItemType.Used)
                    SetSlotCount(-1);
            }
        }
    }


    // 마우스 드래그 시작 인터페이스
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.dragSlot = this;  // 자기 자신을 드래그 슬롯에 할당
            DragSlot.instance.DrageSetImage(itemImage); // 자신의 이미지 할당


            // 슬롯의 위치를 마우스 위치로
            DragSlot.instance.transform.position = eventData.position;
        }
    }


    // 마우스 드래그 중 인터페이스
    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            // 슬롯의 위치를 마우스 위치로 계속 따라오게
            DragSlot.instance.transform.position = eventData.position;
        }
    }


    // 마우스 드래그를 땔 때 인터페이스
    public void OnEndDrag(PointerEventData eventData)
    {
        // 드래그 슬롯 초기화
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;
    }


    // 다른 슬롯 위에서 드래그가 끝날 때
    public void OnDrop(PointerEventData eventData)
    {
        // DrageSlot이 null이 아닐 때만
        if (DragSlot.instance.dragSlot != null)
        {
            ChangeSlot();
        }
    }


    // 아이템 바꾸기
    private void ChangeSlot()
    {
        // 대상 슬롯의 임시 사본 생성
        Item _tempItem = item;
        int _temItemCount = itemCount;

        // Drag 슬롯의 아이템으로 변경
        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        if (_tempItem != null)
        {
            DragSlot.instance.dragSlot.AddItem(_tempItem, _temItemCount);
        }
        else
        {
            DragSlot.instance.dragSlot.ClearSlot();
        }
    }


    // 마우스가 슬롯에 들어올 때 발동
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
            theItemEffectDatabase.ShowToolTip(item, transform.position);
    }


    // 마우스가 슬롯에서 빠져나갈 때 발동
    public void OnPointerExit(PointerEventData eventData)
    {
        theItemEffectDatabase.HideToolTip();
    }
}
