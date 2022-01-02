using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeController : CloseWeaponController
{
    // static
    public static bool isActivate = false;   // 활성화 여부


    // Update is called once per frame
    void Update()
    {
        if (isActivate)
            TryAttack();
    }


    // 추상 클래스 구현
    protected override IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                isSwing = false;    // 뭐가 맞았따면 스윙상태를 멈춤
                Debug.Log("도끼 휘두름");
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }


    public override void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        base.CloseWeaponChange(_closeWeapon); // this

        isActivate = true;
    }
}
