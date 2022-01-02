using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    private float range;        // 습득 가능한 최대 거리

    private bool pickupActivated = false;   // 습득 가능할 시 true

    private RaycastHit hitInfo; // 충돌체 정보 저장

    [SerializeField]
    private LayerMask layerMask;    // 아이템 레이어에만 반응하도록 item만 획득해야함

    // 필요한 컴포넌트
    [SerializeField]
    private Text actionText;    // 화면에 보여줄 텍스트
    [SerializeField]
    private Inventory theInventory;     // 인벤토리


    // Update is called once per frame
    void Update()
    {
        CheckItem();
        TryAction();
    }
    

    // 액션 시도
    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckItem();
            CanPickUp();
        }
    }

     
    // 아이템 획득
    private void CanPickUp()
    {
        if (pickupActivated)
        {
            if (hitInfo.transform != null)
            {
                Debug.Log(hitInfo.transform.GetComponent<ItemPickUp>().item.itemName
                          + " 획득했습니다.");

                theInventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickUp>().item);

                // 획득 후 파괴
                Destroy(hitInfo.transform.gameObject);
                InfoDisappear();
            }
        }
    }


    // 아이템 체크
    private void CheckItem()
    {
        // transform.forward == transform.TransformDirection(Vector3.forward)
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, layerMask))
        {
            if (hitInfo.transform.tag == "Item")
            {
                ItemInfoAppear();
            }
        }
        else
            InfoDisappear();

    }


    // 아이템 발견
    private void ItemInfoAppear()
    {
        pickupActivated = true; // 이제 상호작용 가능
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName 
                          + " 획득 " + "<color=yellow>" + "(E)" + "</color>";
    }


    // 아이템이 없음
    private void InfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }
}
