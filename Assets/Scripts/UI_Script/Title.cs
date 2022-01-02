using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public string sceneName = "GameStage";        // 이동할 씬 네임

    public static Title instance;
    private SaveNLoad theSaveNLoad;

    // 싱글턴
    private void Awake() 
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this.gameObject);
    }

    public void ClickStart()
    {
        Debug.Log("로딩");
        gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;   // 커서 잠금
        Cursor.visible = false;     // 커서 안보이게, 커서를 잠그면 자동으로 실행됨
        SceneManager.LoadScene(sceneName);
    }

    public void ClickLoad()
    {
        Debug.Log("로드");
        
        StartCoroutine(LoadCoroutine());

    }

    IEnumerator LoadCoroutine()
    {
        //SceneManager.LoadScene(sceneName);  // 먼저 씬을 이동시켜야함
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);   
        Cursor.lockState = CursorLockMode.Locked;   // 커서 잠금
        Cursor.visible = false;     // 커서 안보이게, 커서를 잠그면 자동으로 실행됨
        
        // 로딩 대기
        while (!operation.isDone)
        {
            // operation.process 를 이용해 로딩화면을 만들 수 있음
            yield return null;
        }
        theSaveNLoad = FindObjectOfType<SaveNLoad>();      // 다음 씬의 savenload를 찾게됨
        theSaveNLoad.LoadData();        // 싱글턴을 적용해 파괴되지 않고 실행됨
        gameObject.SetActive(false);
    }

    public void ClickExit()
    {
        Debug.Log("게임 종료");
        Application.Quit();
    }
}
