using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakAnimal : Animal
{

    // 위협받는 상황에서 달리기
    public void Run(Vector3 _targetPos)
    {
        // _targetPos => 플레이어의 위치
        // 플레이어 반대 방향을 바라보게
        //destination = Quaternion.LookRotation(transform.position - _targetPos).eulerAngles;
        // 방향이 아닌 정점으로
        destination = new Vector3(transform.position.x - _targetPos.x, 0f, transform.position.z - _targetPos.z).normalized;

        currentTime = runTime;
        isWalking = false;
        isRunning = true;
        //applySpeed = runSpeed;
        nav.speed = runSpeed;
        anim.SetBool("Running", isRunning);
    }


    // 약한 동물은 피해를 받으면 도망감
    public override void Damage(int _dmg, Vector3 _targetPos)
    {
        base.Damage(_dmg, _targetPos);

        if (!isDead)
        {
            Run(_targetPos);
        }
    }
}
