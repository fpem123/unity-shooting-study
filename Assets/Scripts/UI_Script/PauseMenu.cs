using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject go_BaseUi;
    [SerializeField] private SaveNLoad theSaveNLoad;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!GameManager.isPause)
                CallMenu();
            else
                CloseMenu();
        }
    }

    // 메뉴창 열기
    private void CallMenu()
    {
        GameManager.isPause = true;
        go_BaseUi.SetActive(true);
        Time.timeScale = 0f;    // 시간 멈추기
    }

    // 메뉴창 닫기
    private void CloseMenu()
    {
        GameManager.isPause = false;
        go_BaseUi.SetActive(false);
        Time.timeScale = 1f;    // 정상 속도
    }

    public void ClickSave()
    {
        Debug.Log("세이브");
        theSaveNLoad.SaveData();
    }

    public void ClickLoad()
    {
        Debug.Log("로드");
        theSaveNLoad.LoadData();
    }

    public void ClickExit()
    {
        Debug.Log("게임 종료");
        Application.Quit();
    }
}
