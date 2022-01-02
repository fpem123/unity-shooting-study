using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public string gunName;      // 총 구분
    public float range;         // 사정 거리
    public float accuracy;      // 정확도
    public float fireRate;      // 연사 속도
    public float reloadTime;    // 재장전 속도

    public int damage;          // 공격력

    public int reloadBulletCount;   // 총알 재장전 개수
    public int currentBulttetCount; // 현재 탄창에 남아있는 탄환 수
    public int maxBulletCount;      // 최대 소유 가능 탄환 수
    public int carryBulletCount;    // 현재 소유하고 있는 탄환 수

    public float retroActionForce;          // 반동 세기
    public float retroActionFineSightForce; // 정조준시 반동 세기

    public Vector3 fineSightOriginPos;      // 조준시 위치
    public Animator anim;                   // 애니메이션
    public ParticleSystem muzzleFlash;      // 빛
    public AudioClip fireSound;             // 발사 효과음
    
}
