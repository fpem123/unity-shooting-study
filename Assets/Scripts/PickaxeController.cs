using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeController : CloseWeaponController
{
    // static
    public static bool isActivate = false;   // 활성화 여부

    /*
    private void Start()
    {
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;
        isActivate = true;
    }
    */
    

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
                Debug.Log("곡괭이 휘두름");
                // 돌 태그 오브젝트와 상호작용
                if (hitInfo.transform.tag == "Rock")
                    hitInfo.transform.GetComponent<Rock>().Mining();
                else if (hitInfo.transform.tag == "WeakAnimal")
                {
                    // 도망가는 약한 동물
                    SoundManager.instance.PlaySE("Animal_Hit");
                    hitInfo.transform.GetComponent<WeakAnimal>().Damage(1, transform.position);
                }
                else if (hitInfo.transform.tag == "StrongAnimal")
                {
                    // 반격하는 강한 동물
                    SoundManager.instance.PlaySE("Animal_Hit");
                    hitInfo.transform.GetComponent<WeakAnimal>().Damage(1, transform.position);
                }

                isSwing = false;    // 뭐가 맞았따면 스윙상태를 멈춤
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
