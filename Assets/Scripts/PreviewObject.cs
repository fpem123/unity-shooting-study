using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    // 충돌한 오브젝트의 콜라이더를 저장하는 리스트
    private List<Collider> colliderList = new List<Collider>();
    
    [SerializeField]
    private int layerGround;    // 지상 레이어, 부딪히면 안됨
    private const int IGNORE_RAYCAST_RAYER = 2; // ignore raycast

    [SerializeField]
    private Material green;     // 건설 가능 매터리얼
    [SerializeField]
    private Material red;       // 건설 불가 매터리얼


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ChangeColor();
    }

    // 프리뷰 프리팹의 색 변경
    private void ChangeColor()
    {
        if (colliderList.Count > 0)
            SetColor(red);
        else
            SetColor(green);
    }

    // 색 적용
    private void SetColor(Material mat)
    {
        foreach (Transform tf_Child in this.transform)
        {
            var newMaterials = new Material[tf_Child.GetComponent<Renderer>().materials.Length];

            for (int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = mat;
            }

            tf_Child.GetComponent<Renderer>().materials = newMaterials;
        }
    }

    // 트리거에 들어올 시
    private void OnTriggerEnter(Collider other) 
    {
        Debug.Log(other);
        if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_RAYER)
            colliderList.Add(other);
    }

    // 트리거에서 나갈 시
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_RAYER)
            colliderList.Remove(other);
    }

    public bool isBuildable()
    {
        return colliderList.Count == 0;
    }
}
