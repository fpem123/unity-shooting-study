using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// 드래그 되는 슬롯
public class DragSlot : MonoBehaviour
{
    static public DragSlot instance;    // 자기 자신을 인스턴스로

    public Slot dragSlot;

    [SerializeField]
    private Image imageItem;    // 아이템 이미지


    void Start()
    {
        instance = this;
    }


    // 드래그 이미지 할당
    public void DrageSetImage(Image _itemImage)
    {
        imageItem.sprite = _itemImage.sprite;
        SetColor(1);
    }

    
    // 이미지 알파값 변경
    public void SetColor(float _alpha)
    {
        Color color = imageItem.color;
        color.a = _alpha;
        imageItem.color = color;
    }
}
