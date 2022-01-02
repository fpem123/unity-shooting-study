using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : CloseWeaponController
{
    // static
    public static bool isActivate = false;   // 활성화 여부

    
    private void Start()
    {
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;
        isActivate = true;
    }
    

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
                Debug.Log(hitInfo.transform.name);
                Debug.Log("손 휘두름");
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
