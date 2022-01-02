using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField]
    private int hp;     // 바위의 체력

    [SerializeField]
    private float destroyTime;  // 파편 제거 시간

    [SerializeField]
    private SphereCollider col; // 구체 콜라이더


    // 필요한 오브젝트
    [SerializeField]
    private GameObject go_rock; // 일반 바위
    [SerializeField]
    private GameObject go_debris;   // 꺠진 바위
    [SerializeField]
    private GameObject go_effect_prefabs;   // 채굴 이펙트에 사용할 프리팹
    [SerializeField]
    private GameObject go_rock_item_prefab; // 돌맹이 아이템


    [SerializeField]
    private int minCount;  // 아이템 등장 개수
    [SerializeField]
    private int maxCount;  // 아이템 등장 개수


    // 사운드 이름들 
    [SerializeField]
    private string strikeSound;
    [SerializeField]
    private string destroySound;



    public void Mining()
    {
        SoundManager.instance.PlaySE(strikeSound);

        // 게임 오브젝트 생성
        // 오브젝트, 위치, 방향
        var clone = Instantiate(go_effect_prefabs, col.bounds.center, Quaternion.identity);
        Destroy(clone, destroyTime);

        hp--;

        if (hp <= 0)
            Destruction();
    }


    private void Destruction()
    {
        SoundManager.instance.PlaySE(destroySound);

        // 콜라이더 비활성화
        col.enabled = false;

        for (int i = 0; i < Random.Range(minCount, maxCount); i++)
        {
            Instantiate(go_rock_item_prefab, go_rock.transform.position, Quaternion.identity);
        }

        Destroy(go_rock);

        // 데브리 활성화
        go_debris.SetActive(true);
        Destroy(go_debris, destroyTime);
    }
}
