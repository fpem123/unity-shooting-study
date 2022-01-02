using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool canPlayerMove = true;        // 플레이어 움직임 제어

    public static bool isOpenInventory = false;     // 인벤토리 활성화 여부
    public static bool isOpenCraftManual = false;   // 건설 메뉴창 활성화 여부

    public static bool isNight = false;             // 낯과 밤
    public static bool isWater = false;             // 물 속에 있는지
    
    public static bool isPause = false;     // 메뉴가 켜졌는지

    private WeaponManager theWM;
    private bool flag = false;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;   // 커서 잠금
        Cursor.visible = false;     // 커서 안보이게, 커서를 잠그면 자동으로 실행됨
        theWM = FindObjectOfType<WeaponManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpenInventory || isOpenCraftManual || isPause)
        {
            Cursor.lockState = CursorLockMode.None;   // 커서 잠금 해제
            Cursor.visible = true;     // 커서 보이게
            canPlayerMove = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;   // 커서 잠금
            Cursor.visible = false;     // 커서 안보이게, 커서를 잠그면 자동으로 실행됨
            canPlayerMove = true;
        }

        if (isWater && !flag)
        {
            StopAllCoroutines();
            StartCoroutine(theWM.WeaponInCoroutine());
            flag = true;
        }
        else if (!isWater && flag)
        {
            theWM.WeaponOut();
            flag = false;
        }
    }
}
