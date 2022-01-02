using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]   // 클래스 직렬화
public class Sound
{
    public string name;     // 소리의 이름
    public AudioClip clip;   // 곡
}


// 싱글턴화 (Singleton) 1개를 계속 유지
// 씬이 바뀌어도 유지되게
public class SoundManager : MonoBehaviour
{
    static public SoundManager instance;    // 자기자신
    #region singletone
    // 객체 생성시 최초 실행
    private void Awake()
    {
        // 최초 실행시 자신을 할당시킴
        // 이미 있으면 파괴시킴
        // 1개만 유지
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // 파괴 불가
        }
        else
            Destroy(this.gameObject);
        
    }
    #endregion singletone


    public AudioSource[] audioSourceEffects;    // 효과음 소스 배열
    public AudioSource audioSourceBGM;      // 배경음 소스 

    public string[] playSoundName;      // 플레이중인 사운드

    public Sound[] effectSounds;        // 효과음 배열
    public Sound[] bgmSounds;           // 배경음 배열



    private void Start()
    {
        playSoundName = new string[audioSourceEffects.Length];
    }


    public void PlaySE(string _name)
    {
        for (int i = 0; i < effectSounds.Length; i++)
        {
            if (_name == effectSounds[i].name)
            {
                for (int j = 0; j < audioSourceEffects.Length; j++)
                {
                    if (!audioSourceEffects[j].isPlaying)
                    {
                        playSoundName[j] = effectSounds[i].name;

                        // 이미 재생중이면 바꿈
                        audioSourceEffects[j].clip = effectSounds[i].clip;
                        audioSourceEffects[j].Play();

                        return;
                    }
                }

                Debug.Log("모든 가용 Effect AudioSource가 사용중");
                return;
            }
        }

        Debug.Log(_name + " 사운드가 SoundManager에 없습니다.");
    }


    // 모든 효과음 정지
    public void StopALLSE()
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            audioSourceEffects[i].Stop();
        }
    }


    public void StopSE(string _name)
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            if (playSoundName[i] == _name)
            {
                audioSourceEffects[i].Stop();
                
                return;
            }
        }

        Debug.Log(_name + "이 재생중이지 않습니다.");
    }



    public void PlayBGM(string _name)
    {
        for (int i = 0; i < bgmSounds.Length; i++)
        {
            if (_name == bgmSounds[i].name)
            {
                if (!audioSourceBGM.isPlaying)
                {
                    // 이미 재생중이면 바꿈
                    audioSourceBGM.clip = bgmSounds[i].clip;
                    audioSourceBGM.Play();

                    return;
                }

                Debug.Log("모든 가용 BGM AudioSource가 사용중");
                return;
            }
        }

        Debug.Log(_name + " 사운드가 SoundManager에 없습니다.");
    }


    // 모든 효과음 정지
    public void StopBGM()
    {
        audioSourceBGM.Stop();
    }

}
