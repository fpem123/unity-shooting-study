using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    [SerializeField] private float secondPerRealTimeSecound;    // 게임 세계의 1000초 = 현실의 1초

    //private bool isNight = false;   // 밤 체크

    [SerializeField] private float fogDensityCalc;  // Fog 증감량 비율

    [SerializeField] private float nightFogDensity;     // 밤 상태의 Fog 밀도
    private float dayFogDensity;     // 낯 상태의 Fog 밀도
    private float currentFogDensity;    // 현재 fog 밀도

    // Start is called before the first frame update
    void Start()
    {
        dayFogDensity = RenderSettings.fogDensity;      // 렌더러 세팅의 기본 세팅
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.right, 0.1f * secondPerRealTimeSecound * Time.deltaTime);
        
        // 현재 태양의 각도
        if (transform.eulerAngles.x >= 170) // 밤
            GameManager.isNight = true;
        else if (transform.eulerAngles.x <= 10) // 해가 뜨는 중
            GameManager.isNight = false;

        if (GameManager.isNight)
        {   
            // 밤엔 점점 어두워지게
            if (currentFogDensity <= nightFogDensity)
            {
                currentFogDensity += 0.1f * fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
        }
        else
        {
            // 낯엔 점점 밝아지게
            if (currentFogDensity >= dayFogDensity)
            {
                currentFogDensity -= 0.1f * fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
        }

    }
}
