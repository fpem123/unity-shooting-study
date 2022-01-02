using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private float gunAccuracy;  // 크로스헤어 상태에 따른 총의 정확도

    [SerializeField]
    private GameObject goCrosshairHUD;  // 크로스헤어 비활성화를 위한 부모객체, 필요할 때만 나오게
    [SerializeField]
    private GunController theGunController;


    // 걷기 애니메이션
    public void WalkingAnimation(bool _flag)
    {
        if (!GameManager.isWater)
        {
            WeaponManager.currentWeaponAnim.SetBool("Walk", _flag);
            animator.SetBool("Walking", _flag);
        }
    }


    // 달리기 애니메이션
    public void RunningAnimation(bool _flag)
    {
        if (!GameManager.isWater)
        {
            WeaponManager.currentWeaponAnim.SetBool("Run", _flag);
            animator.SetBool("Running", _flag);
        }
    }

    // 점프 애니메이션
    public void JumpingAnimation(bool _flag)
    {
        if (!GameManager.isWater)
        {
            animator.SetBool("Running", _flag);
        }
    }


    // 앉기 애니메이션
    public void CrouchingAnimation(bool _flag)
    {
        if (!GameManager.isWater)
        {
            animator.SetBool("Crouching", _flag);
        }
    }


    // 앉기 애니메이션
    public void FineSightAnimation(bool _flag)
    {
        if (!GameManager.isWater)
        {
            animator.SetBool("FineSight", _flag);
        }
    }


    // 발사 애니메이션
    public void FireAnimation()
    {
        if (!GameManager.isWater)
        {
            if (animator.GetBool("Walking"))
                animator.SetTrigger("Walk_Fire");
            else if (animator.GetBool("Crouching"))
                animator.SetTrigger("Crouch_Fire");
            else
                animator.SetTrigger("Idle_Fire");
        }
    }


    // 총 정확도 설정
    public float GetAccuracy()
    {
        if (animator.GetBool("Walking"))
            gunAccuracy = 0.06f;
        else if (animator.GetBool("Crouching"))
            gunAccuracy = 0.015f;
        else if(theGunController.GetFineSightMode())
            gunAccuracy = 0.001f;
        else
            gunAccuracy = 0.035f;

        return gunAccuracy;
    }

}
