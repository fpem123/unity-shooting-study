using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;   // A* 알고리즘을 사용하기 위한 라이브러리

public class Animal : MonoBehaviour
{
    [SerializeField] protected string animalName; // 동물의 이름
    [SerializeField] protected int hp;    // 동물의 체력

    [SerializeField] protected float walkSpeed; // 걷기 스피드
    [SerializeField] protected float runSpeed;    // 뛰기 스피드
    //[SerializeField] protected float turningSpeed;  // 회전 속도
    //protected float applySpeed;   // 현재 속도

    protected Vector3 destination;  // 이동 목적지

    // 상태변수
    protected bool isAction;   // 행동 중인지
    protected bool isWalking; // 걷는지
    protected bool isRunning; // 뛰는지
    protected bool isDead;    // 죽었는지

    [SerializeField] protected float walkTime;    // 걷기 시간
    [SerializeField] protected float waitTime;    // 대기 시간
    [SerializeField] protected float runTime;     // 뛰기 시간
    protected float currentTime;      // 시간 카운터

    // 필요한 컴퓨넌트
    [SerializeField] protected Animator anim;
    [SerializeField] protected Rigidbody rigid;
    [SerializeField] protected BoxCollider boxCol;
    protected AudioSource theAudio;   // 오디오 소스
    protected NavMeshAgent nav;

    [SerializeField] protected AudioClip[] sound_Normal;  // 일반 소리
    [SerializeField] protected AudioClip sound_Hurt;      // 피격음
    [SerializeField] protected AudioClip sound_Dead;      // 사망음



    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        theAudio = GetComponent<AudioSource>();
        currentTime = 0.1f;
        isAction = true;
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            Move();
            //Rotation();
            ElapseTime();
        }
    }


    // 이동
    protected void Move()
    {
        if (isWalking || isRunning)
            // 전방으로 이동
            //rigid.MovePosition(transform.position + transform.forward * applySpeed * Time.deltaTime);
            // 어떤 방향으로 뛰는게 아닌 어떤 목적지로 뛰게 변경
            // 정규화되어 작은 값이기에 5를 곱함
            nav.SetDestination(transform.position + destination * 5f);
    }


    /* NavMeshAgent가 활성화 되면 rigidbody가 강제로 잠긴다.
     * Move에 Rotation 기능도 추가
    // 방향 전환
    protected void Rotation()
    {
        if (isWalking || isRunning)
        {
            // 원 방향에서 정해진 방향으로 turningSpeed 속도로 회전
            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles,
                                             new Vector3(0f, direction.y, 0f),
                                             turningSpeed);

            rigid.MoveRotation(Quaternion.Euler(_rotation));
        }
    }
    */


    // 시간 경과 계산
    protected void ElapseTime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0)
                ReSet();   // 다음 행동 개시
        }

    }


    // 행동 리셋
    protected virtual void ReSet()
    {
        isWalking = false;
        isRunning = false;
        isAction = true;

        //applySpeed = walkSpeed;
        nav.speed = walkSpeed;  // 기존 방법은 작동하지 않음

        nav.ResetPath();    // 목적지 초기화, 부들거림 방지

        //destination.Set(0f, Random.Range(0f, 360f), 0f);  // y값만 바꾸면 된다
        destination.Set(Random.Range(-0.2f, 0.2f), 0f, Random.Range(0.5f, 1f)); // 이동할 목적지 설정

        anim.SetBool("Walking", isWalking);
        anim.SetBool("Running", isRunning);
    }



    // 걷기 시도
    protected void TryWalk()
    {
        isWalking = true;
        anim.SetBool("Walking", isWalking);
        currentTime = walkTime;

        //applySpeed = walkSpeed;
        nav.speed = walkSpeed;  // 기존 방법은 작동하지 않음

        //Debug.Log("걷기");
    }


    // 데미지 받기, 가상함수
    public virtual void Damage(int _dmg, Vector3 _targetPos)
    {
        if (!isDead)
        {
            hp -= _dmg;

            // 사망
            if (hp <= 0)
            {
                Dead();
                return;
            }

            PlaySE(sound_Hurt);
            anim.SetTrigger("Hurt");
        }
    }


    // 돼지 사망
    protected void Dead()
    {
        PlaySE(sound_Dead);
        isWalking = false;
        isRunning = false;
        isDead = true;
        anim.SetTrigger("Dead");
    }


    // 랜덤 일반음 출력
    protected void RandomSound()
    {
        int _random = Random.Range(0, 3);   // 일상 사운드 3개
        PlaySE(sound_Normal[_random]);
    }


    // 소리 재생
    protected void PlaySE(AudioClip _clip)
    {
        theAudio.clip = _clip;
        theAudio.Play();
    }
}
