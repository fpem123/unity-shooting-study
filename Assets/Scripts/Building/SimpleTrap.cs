using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTrap : MonoBehaviour
{
    private Rigidbody[] rigid;  // 자식들의 리지드
    [SerializeField] private GameObject go_Meat; // 고기
    [SerializeField] private int damage;    // 함정의데미지
    private bool isActivated = false;   // 1회성 함정

    private AudioSource theAudio;
    [SerializeField] private AudioClip sound_Activate;  // 작동 소리

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponentsInChildren<Rigidbody>();
        theAudio = GetComponent<AudioSource>();
    }

    // 콜라이더에 닿으면
    private void OnTriggerEnter(Collider other) 
    {  
        if (!isActivated)
        {
            // Untag가 아닌 것들이 닿으면
            if (other.transform.tag != "Untagged")
            {
                Debug.Log(other);
                isActivated = true;
                theAudio.clip = sound_Activate;
                theAudio.Play();
                Destroy(go_Meat);   // 고기 제거

                for (int i = 0; i <rigid.Length; i++)
                {
                    rigid[i].useGravity = true;     // 자연스러운 중력
                    rigid[i].isKinematic = false;
                }

                if (other.transform.name == "Player")
                {
                    //other.transform.GetComponent<StatusController>().DecreaseHP(damage);
                    FindObjectOfType<StatusController>().DecreaseHP(damage);
                }
            }
        }
    }
}
