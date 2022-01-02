using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StatusController : MonoBehaviour
{
    // 체력
    [SerializeField]
    private int hp;     // 최대 체력
    private int currentHp;  // 현재 체력

    // 스테미나
    [SerializeField]
    private int sp;         // 최대 스테미나
    private int currentSp;  // 현재 스테미나

    [SerializeField]
    private int spIncreaseSpeed;    // 스테미나 증가량

    [SerializeField]
    private int spRechargeTime;     // 스테미나 재회복 딜레이
    private int currentSpRechargeTime; // 타이머

    private bool spUsed;    // 스테미나 감소 여부

    // 방어력
    [SerializeField]
    private int dp;             // 최대 방여력
    private int currentDp;      // 현재 방어력

    // 배고픔
    [SerializeField]
    private int hungry;
    private int currentHungry;

    [SerializeField]
    private int hungryDecreaseTime;
    private int currentHungryDecreaseTime;

    // 목마름
    [SerializeField]
    private int thirsty;
    private int currentThirsty;

    [SerializeField]
    private int thirstyDecreaseTime;
    private int currentThirstyDecreaseTime;

    // 만족도
    [SerializeField]
    private int satisfy;
    private int currentSatisfy;

    // 필요한 이미지
    [SerializeField]
    private Image[] images_Gauge;    // 이미지 배열

    // 배열 관리를 위한 상수
    private const int HP = 0, DP = 1, SP = 2, HUNGRY = 3, THIRSTY = 4, SATISFY = 5;



    // Start is called before the first frame update
    void Start()
    {
        currentHp = hp;
        currentDp = dp;
        currentSp = sp;
        currentHungry = hungry;
        currentThirsty = thirsty;
        currentSatisfy = satisfy;
    }

    // Update is called once per frame
    void Update()
    {
        Hungry();
        Thirsty();
        SPRechargeTime();
        SPRecover();
        GaugeUpdate();
    }

    
    // 스테미나 회복 시간 체크
    private void SPRechargeTime()
    {
        if (spUsed)
        {
            if (currentSpRechargeTime < spRechargeTime)
                currentSpRechargeTime++;
            else
                spUsed = false;
        }
    }


    // 스테미나 회복
    private void SPRecover()
    {
        if(!spUsed && currentSp < sp)
        {
            currentSp += spIncreaseSpeed;
        }
    }


    // 일정 시간마다 배고픔 감소
    private void Hungry()
    {
        if (currentHungry > 0)
        {
            // 타이머
            if (currentHungryDecreaseTime <= hungryDecreaseTime)
                currentHungryDecreaseTime++;
            else
            {
                currentHungry--;
                currentHungryDecreaseTime = 0;
            }
        }
        else
            Debug.Log("배고픔 수치가 0이 됨");
    }


    // 일정 시간마다 목마름 감소
    private void Thirsty()
    {
        if (currentThirsty > 0)
        {
            // 타이머
            if (currentThirstyDecreaseTime <= thirstyDecreaseTime)
                currentThirstyDecreaseTime++;
            else
            {
                currentThirsty--;
                currentThirstyDecreaseTime = 0;
            }
        }
        else
            Debug.Log("목마름 수치가 0이 됨");
    }


    // 상태를 이미지에 반영
    private void GaugeUpdate()
    {
        // fillAmount로 이미지 조절
        images_Gauge[HP].fillAmount = (float)currentHp / hp;
        images_Gauge[SP].fillAmount = (float)currentSp / sp;
        images_Gauge[DP].fillAmount = (float)currentDp / dp;
        images_Gauge[HUNGRY].fillAmount = (float)currentHungry / hungry;
        images_Gauge[THIRSTY].fillAmount = (float)currentThirsty / thirsty;
        images_Gauge[SATISFY].fillAmount = (float)currentSatisfy / satisfy;
    }


    // 체력 증가
    public void IncreaseHP(int _count)
    {
        if (currentHp + _count < hp)
            currentHp += +_count;
        else
            currentHp = hp;
    }


    // 체력 감소
    public void DecreaseHP(int _count)
    {
        if (currentDp > 0)
        {
            DecreaseDP(_count);
            return;
        }

        currentHp -= _count;

        if (currentHp <= 0)
            Debug.Log("게임 오버");
    }


    // 방어력 증가
    public void IncreaseDP(int _count)
    {
        if (currentDp + _count < dp)
            currentDp += +_count;
        else
            currentDp = dp;
    }


    // 방어력 감소
    public void DecreaseDP(int _count)
    {
        if (currentDp - _count > 0)
            currentDp -= _count;
        else
            currentDp = 0;
    }


    // 배고픔 증가
    public void IncreaseHungry(int _count)
    {
        if (currentHungry + _count < hungry)
            currentHungry += +_count;
        else
            currentHungry = hungry;
    }


    // 배고픔 감소
    public void DecreaseHungry(int _count)
    {
        if (currentHungry - _count < 0)
            currentHungry = 0;
        else
            currentHungry -= _count;
    }


    // 목마름 증가
    public void IncreaseThirsty(int _count)
    {
        if (currentThirsty + _count < thirsty)
            currentThirsty += +_count;
        else
            currentThirsty = thirsty;
    }


    // 목마름 감소
    public void DecreaseThirsty(int _count)
    {
        if (currentThirsty - _count < 0)
            currentThirsty = 0;
        else
            currentThirsty -= _count;
    }


    // 스테미나 증가
    public void IncreaseSP(int _count)
    {
        if (currentSp + _count < sp)
            currentSp += +_count;
        else
            currentSp = sp;
    }


    // 스테미나 감소
    public void DecreaseSP(int _count)
    {
        spUsed = true;
        currentSpRechargeTime = 0;

        if (currentSp - _count > 0)
            currentSp -= _count;
        else
            currentSp = 0;
    }


    // sp getter
    public int GetCurrentSP()
    {
        return currentSp;
    }

}
