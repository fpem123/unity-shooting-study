using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfNewAngle : MonoBehaviour
{
    [SerializeField] private float viewAngle;   // 시야각
    [SerializeField] private float viewDistance;    // 시야거리
    [SerializeField] private LayerMask targetMask;  // 타겟 마스트, 상호작용할 타겟

    private Pig thePig;


    private void Start()
    {
        thePig = GetComponent<Pig>();
    }


    // Update is called once per frame
    void Update()
    {
        View();
    }


    // 각도 계산
    private Vector3 BoundaryAngle(float _angle)
    {  
        // z축을 기준으로 좌우 회전하면 y축이 움직임
        _angle += transform.eulerAngles.y;

        // 삼각 함수를 이용해 P 백터값 계산
        // Mathf.Deg2Rad -> 각도로 변환
        return new Vector3(Mathf.Sin(_angle * Mathf.Deg2Rad), 0f, Mathf.Cos(_angle * Mathf.Deg2Rad));   
    }


    private void View()
    {
        // 좌우 시야
        Vector3 _leftBoundary = BoundaryAngle(-viewAngle * 0.5f);
        Vector3 _rightBoundary = BoundaryAngle(viewAngle * 0.5f);

        // 씬에 시야각 출력
        Debug.DrawRay(transform.position + transform.up, _leftBoundary, Color.red);
        Debug.DrawRay(transform.position + transform.up, _rightBoundary, Color.red);

        // 콜라이더로 시야각 안에 있는 모든 객체를 저장
        // OverlapSphere 로 일정 거리안에 있는 컬라이더들을 저장
        // targetMask로 필터링
        Collider[] _target = Physics.OverlapSphere(transform.position, viewDistance, targetMask);

        for (int i = 0; i < _target.Length; i++)
        {
            Transform _targetTf = _target[i].transform;

            // 하이어라키의 이름이 Player라면
            if (_targetTf.name == "Player")
            {
                Vector3 _direction = (_targetTf.position - transform.position).normalized;  // 방향 계산
                // 두 객체간 거리 계산, 되지의 앞 방향과 플레이어 위치와의 각도
                float _angle = Vector3.Angle(_direction, transform.forward);    

                if (_angle < viewAngle * 0.5f)
                {
                    RaycastHit _hit;

                    // 땅에 충돌하지 않게 위에서 발사
                    if (Physics.Raycast(transform.position + transform.up, _direction, out _hit, viewDistance))
                    {
                        if (_hit.transform.name == "Player")
                        {
                            //Debug.Log("플레이어가 시야 내에 있습니다.");
                            Debug.DrawRay(transform.position + transform.up, _direction, Color.blue);

                            // 돼지 도망
                            thePig.Run(_hit.transform.position);
                        }
                    }
                }
            }
        }
    }
}
