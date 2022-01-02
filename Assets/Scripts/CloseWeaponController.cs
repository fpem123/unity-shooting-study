using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 추상 클래스
// Update가 호출될 필요가 없음
public abstract class CloseWeaponController : MonoBehaviour
{
    // 현재 장착된 close weapon 타입 무기
    [SerializeField]
    protected CloseWeapon currentCloseWeapon;

    // 상태 변수
    protected bool isAttack = false;      // 공격중?
    protected bool isSwing = false;       // 팔을 휘두르는 중?

    protected RaycastHit hitInfo;         // 상호작용이 닿은 것의 정보

    [SerializeField]
    protected LayerMask layerMask;      // 충돌할 것들


    // 공격 시도
    protected void TryAttack()
    {
        if (!Inventory.inventoryActivated)
        {
            // 좌클릭, 프로젝트 설정에서 컨트롤 키를 제거해야함
            if (Input.GetButton("Fire1"))
            {
                if (!isAttack)
                {
                    // 코루틴 실행
                    StartCoroutine(AttackCoroutine());
                }
            }
        }
    }


    // 공격하는 코루틴
    protected IEnumerator AttackCoroutine()
    {
        isAttack = true;

        currentCloseWeapon.anim.SetTrigger("Attack");

        yield return new WaitForSeconds(currentCloseWeapon.attackDelayA);
        isSwing = true;     // 휘두름

        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(currentCloseWeapon.attackDelayB);
        isSwing = false;    // 휘두름이 끝남

        // 남은 딜레이
        yield return new WaitForSeconds(currentCloseWeapon.attackDelay - currentCloseWeapon.attackDelayA - currentCloseWeapon.attackDelayB);
        isAttack = false;
    }


    // 뭐가 맞았는지 추상 코루틴, 상호작용
    protected abstract IEnumerator HitCoroutine() ;


    // 뭐가 맞았는지 체크
    protected bool CheckObject()
    {
        // Raycast(지금 위치, 대상위치, 대상정보, 거리)
        // transform.forward == transform.TransformDirection(Vector3.forward)
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentCloseWeapon.range, layerMask))
        {
            return true;
        }
        return false;
    }


    // 무기 변경, 추가 편집 가능한 가상함수
    public virtual void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        // 먼저 있는 총 제거
        if (WeaponManager.currentWeapon != null)
            WeaponManager.currentWeapon.gameObject.SetActive(false);

        currentCloseWeapon = _closeWeapon;

        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>(); // 모든 객체는 Transform을 가짐
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;

        currentCloseWeapon.transform.localPosition = Vector3.zero;      // 총 위치 초기화
        currentCloseWeapon.gameObject.SetActive(true);
    }
}
