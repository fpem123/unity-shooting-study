using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   // UI 관련 모듈, Text 변수

public class HUD : MonoBehaviour
{
    // 필요한 컴포넌트
    [SerializeField]
    private GunController theGunController;
    private Gun currentGun;

    // 필요하면 HUD 호출, 필요 없으면 비활성화
    [SerializeField]
    private GameObject goBulletHUD;

    // 총알 개수 텍스트에 반영
    [SerializeField]
    private Text[] textBullet;


    // Update is called once per frame
    void Update()
    {
        CheckBullet();
    }



    private void CheckBullet()
    {
        currentGun = theGunController.GetGun();

        textBullet[0].text = currentGun.carryBulletCount.ToString();
        textBullet[1].text = currentGun.reloadBulletCount.ToString();
        textBullet[2].text = currentGun.currentBulttetCount.ToString();
    }

}
