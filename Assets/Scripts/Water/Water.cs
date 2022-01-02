using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Water : MonoBehaviour
{
    //public static bool isWater = false;

    [SerializeField] private float waterDrag;   // 물의 중력
    private float originDrag;   // 원래 물의 중력, 플레이어 리지드바디

    [SerializeField] private Color waterColor;  // 물의 색
    [SerializeField] private float waterFogDensity;     // 물의 탁함 정도
    [SerializeField] private Color waterNightColor;     // 물의 탁함 정도
    [SerializeField] private float waterNightFogDensity;     // 물의 탁함 정도

    private Color originColor;     // 물의 원색
    private float originFogDensity;     // 물의 원 탁함
    
    [SerializeField] private Color originNightColor;    // 원래 밤의 색
    [SerializeField] private float originNightFogDensity;  // 원래 밤의 어두움

    // 사운드
    [SerializeField] private string sound_WaterOut;
    [SerializeField] private string sound_WaterIn;
    [SerializeField] private string sound_WaterBreathe;

    [SerializeField] private float breatheTime;
    private float currentBreatheTime;

    [SerializeField] private float totalOxygen;     // 총 산소량
    private float currentOxygen;        // 현재 상소량
    private float temp;     // 체력 감소 시간 체크

    [SerializeField] private GameObject go_BaseUI;
    [SerializeField] Text text_totalOxygen;
    [SerializeField] private Text text_currentOxygen;
    [SerializeField] private Image image_gauge;
    
    private StatusController thePlayerStat;


    // Start is called before the first frame update
    void Start()
    {
        originColor = RenderSettings.fogColor;
        originFogDensity = RenderSettings.fogDensity;

        originDrag = 0;
        thePlayerStat = FindObjectOfType<StatusController>();
        currentOxygen = totalOxygen;
        text_totalOxygen.text = totalOxygen.ToString();
        temp = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.isWater)
        {
            currentBreatheTime += Time.deltaTime;

            if (currentBreatheTime >= breatheTime)
            {
                SoundManager.instance.PlaySE(sound_WaterBreathe);
                currentBreatheTime = 0;
            }
        }

        DecreaseOxygen();
    }

    // 산소 감소
    private void DecreaseOxygen()
    {
        if (GameManager.isWater)
        {
            currentOxygen = Mathf.Clamp(currentOxygen - Time.deltaTime, 0, totalOxygen);
            text_currentOxygen.text = Mathf.RoundToInt(currentOxygen).ToString();
            image_gauge.fillAmount = currentOxygen / totalOxygen;

            if (currentOxygen <= 0)
            {
                temp += Time.deltaTime;
                if (temp >= 1)
                {
                    thePlayerStat.DecreaseHP(1);
                    temp = 0;
                }
            }
        }
    }

    // 유저가 들어오는 거 감지
    private void OnTriggerEnter(Collider other) 
    {
        if (other.transform.tag == "Player")
        {
            GetWater(other);
        }
    }

    // 유저 나가는 거 감지
    private void OnTriggerExit(Collider other) 
    {
        if (other.transform.tag == "Player")
        {
            GetOutWater(other);
        }
    }

    // 유저가 물에 들어옴
    private void GetWater(Collider _player)
    {
        SoundManager.instance.PlaySE(sound_WaterIn);
        go_BaseUI.SetActive(true);

        GameManager.isWater = true;
        _player.transform.GetComponent<Rigidbody>().drag = waterDrag;   // 플레이어의 저항을 변경

        if (!GameManager.isNight)
        {
            RenderSettings.fogColor = waterColor;
            RenderSettings.fogDensity = waterFogDensity;
        }
        else
        {
            RenderSettings.fogColor = waterNightColor;
            RenderSettings.fogDensity = waterNightFogDensity;
        }
    }

    // 유저가 물에서 나감
    private void GetOutWater(Collider _player)
    {
        if (GameManager.isWater)
        {
            go_BaseUI.SetActive(false);
            currentOxygen = totalOxygen;
            SoundManager.instance.PlaySE(sound_WaterOut);
            GameManager.isWater = false;
            _player.transform.GetComponent<Rigidbody>().drag = originDrag;

            if (!GameManager.isNight)
            {
                RenderSettings.fogColor = originColor;
                RenderSettings.fogDensity = originFogDensity;
            }
            else
            {
                RenderSettings.fogColor = originNightColor;
                RenderSettings.fogDensity = originNightFogDensity;
            }
        }
    }
}
