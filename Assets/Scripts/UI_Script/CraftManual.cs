using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 건설 아이템 정보
[System.Serializable]
public class Craft
{
    public string craftName;    // 이름
    public GameObject go_Prefab;    // 실제 설치될 프리팹
    public GameObject go_PreviewPrefab; // 미리보기 프리팹
}

public class CraftManual : MonoBehaviour
{
    // status var
    private bool isActivated = false;   // 창이 켜졌는지
    private bool isPreviewActivated = false;    // 프리뷰 프리팹이 작동중인지

    [SerializeField]
    private GameObject go_BaseUI;   // 기본 베이스 UI

    [SerializeField]
    private Craft[] craft_fire; // 모닥불용 탭

    private GameObject go_Preview; // 미리보기 프리팹을 담을 변수
    private GameObject go_Prefab;  // 실제 생성될 프리팹을 담을 변수

    [SerializeField]
    private Transform tf_Player;    // 플레이어 위치

    // Raycast 필요 변수 선언, 프리뷰 프리팹이 따라다니게
    private RaycastHit hitInfo;     // 충돌 정보
    [SerializeField]
    private LayerMask layerMask;    // 레이어 마스크
    [SerializeField]
    private float range;        // 띄울 거리
    

    // 슬롯이 클릭될 때
    public void SlotClick(int _slotNumber)
    {
        go_Preview = Instantiate(craft_fire[_slotNumber].go_PreviewPrefab,
                                    tf_Player.position + tf_Player.forward,
                                    Quaternion.identity);
        go_Prefab = craft_fire[_slotNumber].go_Prefab;
        isPreviewActivated = true;
        GameManager.isOpenCraftManual = false;
        go_BaseUI.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isPreviewActivated)
            Window();

        if (isPreviewActivated)
            PreviewPositionUpdate();

        if (Input.GetButtonDown("Fire1"))
            Build();

        if (Input.GetKeyDown(KeyCode.Escape))
            Cancel();
    }

    // 프리뷰 위치에 프리팹 건설하기
    private void Build()
    {
        if (isPreviewActivated && go_Preview.GetComponent<PreviewObject>().isBuildable())
        {
            Instantiate(go_Prefab, hitInfo.point, Quaternion.identity);
            Destroy(go_Preview);
            isActivated = false;
            isPreviewActivated = false;
            go_Preview = null;
            go_Prefab = null;
        }
    }

    // 프리뷰 프리팹 위치시키기
    private void PreviewPositionUpdate()
    {
        if (Physics.Raycast(tf_Player.position, tf_Player.forward, out hitInfo, range, layerMask))
        {
            if (hitInfo.transform != null)
            {
                Vector3 _location = hitInfo.point;
                go_Preview.transform.position = _location;
            }
        }
    }

    // 건설 취소
    private void Cancel()
    {
        if (isPreviewActivated)
            Destroy(go_Preview);
        
        CloseWindow();
        isPreviewActivated = false;
        go_Preview = null;
        go_Prefab = null;
    }

    // 건설 메뉴 키고 끄기
    private void Window()
    {
        if (!isActivated)
            OpenWindow();
        else
            CloseWindow();
    }

    // 건설 메뉴창 열기
    private void OpenWindow()
    {
        isActivated = true;
        go_BaseUI.SetActive(true);
        GameManager.isOpenCraftManual = true;
    }

    // 건설 메뉴창 끄기
    private void CloseWindow()
    {
        isActivated = false;
        go_BaseUI.SetActive(false);
        GameManager.isOpenCraftManual = false;
    }
}
