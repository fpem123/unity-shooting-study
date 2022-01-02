using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    private Vector3 originPos;      // 기존 위치

    private Vector3 currentPos;     // 계산에 필요한 현재 위치

    [SerializeField]
    private Vector3 limitPos;       // Sway 한계, 움직임 한계, 화면을 벗어나지 않도록

    [SerializeField]
    private Vector3 fineSightLimitPos;  // 정조준 sway  한계

    [SerializeField]
    private Vector3 smoothSway;          // 부드러운 움직임 정도

    // 필요한 컴퓨넌트
    [SerializeField]
    private GunController theGunController;



    // Start is called before the first frame update
    void Start()
    {
        originPos = this.transform.localPosition;   // 현재 자신의 위치 값    
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.canPlayerMove)
        {
            TrySway();
        }
    }



    private void TrySway()
    {
        // 마우스 상하 좌우 움직임
        if (Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0)
            Swaying();
        else
            BackToOriginPos();
    }



    // 마우스 흔들림, 마우스 가두기
    private void Swaying()
    {
        float _moveX = Input.GetAxisRaw("Mouse X");
        float _moveY = Input.GetAxisRaw("Mouse Y");

        if (!theGunController.isFineSightMode)
        {
            // - 를 안붙히면 마우스가 빠르게 사라짐
            currentPos.Set(Mathf.Clamp(Mathf.Lerp(currentPos.x, -_moveX, smoothSway.x), -limitPos.x, limitPos.x),
                           Mathf.Clamp(Mathf.Lerp(currentPos.y, -_moveY, smoothSway.x), -limitPos.y, limitPos.y),
                           originPos.z);
        }
        else
        {
            currentPos.Set(Mathf.Clamp(Mathf.Lerp(currentPos.x, -_moveX, smoothSway.y), -fineSightLimitPos.x, fineSightLimitPos.x),
                           Mathf.Clamp(Mathf.Lerp(currentPos.y, -_moveY, smoothSway.y), -fineSightLimitPos.y, fineSightLimitPos.y),
                           originPos.z);
        }

        transform.localPosition = currentPos;
    }


    // 다시 원위치
    private void BackToOriginPos()
    {
        currentPos = Vector3.Lerp(currentPos, originPos, smoothSway.x);

        transform.localPosition = currentPos;
    }
}
