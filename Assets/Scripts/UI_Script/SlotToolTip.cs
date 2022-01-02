using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SlotToolTip : MonoBehaviour
{
    [SerializeField]
    private GameObject go_Base; // 툴팁박스

    // 필요한 컴포넌트
    [SerializeField]
    private Text txt_ItemName;
    [SerializeField]
    private Text txt_ItemDesc;
    [SerializeField]
    private Text txt_ItemHowToUsed;


    // 툴팁을 보여줌
    public void ShowToolTip(Item _item, Vector3 _pos)
    {
        go_Base.SetActive(true);

        // 툴팁 박스 좌우길이의 반 만큼 위치를 이동시킴
        _pos += new Vector3(go_Base.GetComponent<RectTransform>().rect.width * 0.5f,
                            -go_Base.GetComponent<RectTransform>().rect.height * 0.5f,
                            0f);

        go_Base.transform.position = _pos;

        txt_ItemName.text = _item.itemName;
        txt_ItemDesc.text = _item.itemDesc;

        if (_item.itemType == Item.ItemType.Equipment)
            txt_ItemHowToUsed.text = "우클릭 - 장착";
        else if (_item.itemType == Item.ItemType.Used)
            txt_ItemHowToUsed.text = "우클릭 - 먹기";
        else
            txt_ItemHowToUsed.text = "";
    }

    
    // 툴팁을 가림
    public void HideToolTip()
    {
        go_Base.SetActive(false);
    }
}
