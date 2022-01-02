using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    // static
    public static bool isActivate = false;   // 활성화 여부

    // 현재 장착된 Hand형 타입 무기
    [SerializeField]
    private Gun currentGun;

    private float currentFireRate;      // 연사 속도

    // 상태 변수
    private bool isReload = false;          // 리로드 상태인지
    public bool isAttack = false;
    [HideInInspector]
    public bool isFineSightMode = false;   // 정조준 상태인지

    private Vector3 originPos;              // 본래 포지션 값
    private Vector3 recoilBack;             // 발사 시 반동
    private Vector3 retroActionRecoilBack;  // 정조준 시 반동

    private AudioSource audioSource;    // 발사 효과음

    private RaycastHit hitInfo;         // 상호작용이 닿은 것의 정보
    [SerializeField]
    private LayerMask layerMask;        // 충돌할 것들


    // 필요한 컴포넌트
    [SerializeField]
    private Camera theCam;           // 크로스 헤어 중앙을 표현하기 위함
    private Crosshair theCrosshair;

    [SerializeField]
    private GameObject hitEffectPrefab; // 피격 이펙트


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        theCrosshair = FindObjectOfType<Crosshair>();
        originPos = Vector3.zero;

        recoilBack = new Vector3(currentGun.retroActionForce, originPos.y, originPos.z);
        retroActionRecoilBack = new Vector3(currentGun.retroActionFineSightForce, currentGun.fineSightOriginPos.y, currentGun.fineSightOriginPos.z);

        /*
        WeaponManager.currentWeapon = currentGun.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentGun.anim;
        */
    }

    // Update is called once per frame
    void Update()
    {
        if (isActivate)
        {
            GunFireRateCalc();

            if (!Inventory.inventoryActivated)
            {
                TryFire();
                TryFineSight();
            }

            TryReload();
        }
    }


    // 발사 딜레이 계산
    private void GunFireRateCalc()
    {
        if (currentFireRate > 0)
            currentFireRate -= Time.deltaTime;  // 1초의 역수, 1/60
    }


    // 발사 시도
    private void TryFire()
    {
        // 키 홀딩 가능
        if (Input.GetButton("Fire1") && currentFireRate <= 0)
        {
            Fire();
        }
    }
     

    // 발사 가능한지 체크
    private void Fire()
    {
        if (!isReload)
        {
            if (currentGun.currentBulttetCount > 0)
                Shoot();
            else
            {
                CancelFineSight();
                StartCoroutine(ReloadCoroutine());
            }
        }
    }

    
    //  발사 후 계산
    private void Shoot()
    {
        isAttack = true;

        theCrosshair.FireAnimation();

        PlaySE(currentGun.fireSound);
        currentGun.currentBulttetCount--;
        currentFireRate = currentGun.fireRate;  // 연사 속도 재계산
        currentGun.muzzleFlash.Play();

        Hit();

        // 총기 반동 코루틴
        StopAllCoroutines();
        StartCoroutine(RetroActionCoroutine());

        isAttack = false;
    }


    
    // 적중되는 위치 계산
    private void Hit()
    {
        // 상대 위치가 아닌 절대 포지션으로
        // 앞방향 + 정확도(크로스헤어의 정확도와 총의 정확도)
        if (Physics.Raycast(theCam.transform.position, 
                            theCam.transform.forward + new Vector3(Random.Range(-theCrosshair.GetAccuracy() - currentGun.accuracy, theCrosshair.GetAccuracy() + currentGun.accuracy),
                                                                   Random.Range(-theCrosshair.GetAccuracy() - currentGun.accuracy, theCrosshair.GetAccuracy() + currentGun.accuracy),
                                                                   0),
                            out hitInfo, currentGun.range, layerMask))
        {
            // NPC 태그 오브젝트와 상호작용
            if (hitInfo.transform.tag == "WeakAnimal")
            {
                SoundManager.instance.PlaySE("Animal_Hit");
                hitInfo.transform.GetComponent<WeakAnimal>().Damage(currentGun.damage, transform.position);
            }


            // 프리팹을 위치에 생성
            // 프래팹, 위치, 각도
            // Quaternion.LookRotation() -> 특정 객체를 바라보는 각도
            // normal -> 충돌한 위치의 표면
            Debug.Log(hitInfo.transform.name);
            GameObject clone = Instantiate(hitEffectPrefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));

           
            
            Destroy(clone, 2f); // 2초 후에 오브젝트 파괴
        }
    }


    // 재장전 시도
    private void TryReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReload && currentGun.currentBulttetCount < currentGun.reloadBulletCount)
        {
            CancelFineSight();
            StartCoroutine(ReloadCoroutine());
        }
    }
    
    public void CancelReload()
    {
        if (isReload)
        {
            StopAllCoroutines();
            isReload = false;
        }
    }

    // 리로드 코루틴
    IEnumerator ReloadCoroutine()
    {
        if (currentGun.carryBulletCount > 0)
        {
            isReload = true;
            currentGun.anim.SetTrigger("Reload");

            // 기존에 남아있던 총알이 사라지지 않게
            currentGun.carryBulletCount += currentGun.currentBulttetCount;
            currentGun.currentBulttetCount = 0;

            yield return new WaitForSeconds(currentGun.reloadTime);

            // 남은 총알 수 체크
            if (currentGun.carryBulletCount >= currentGun.reloadBulletCount)
            {
                currentGun.currentBulttetCount = currentGun.reloadBulletCount;
                currentGun.carryBulletCount -= currentGun.reloadBulletCount;
            }
            else
            {
                currentGun.currentBulttetCount = currentGun.carryBulletCount;
                currentGun.carryBulletCount = 0;
            }

            isReload = false;
        }
        else
        {
            Debug.Log("소유한 총알이 없습니다.");
        }
    }


    // 정조준 상태 전환 시도
    private void TryFineSight()
    {
        if (Input.GetButtonDown("Fire2") && !isReload)
        {
            FineSight();
        }
    }


    // 정조준 강제 해제
    public void CancelFineSight()
    {
        if (isFineSightMode)
        {
            FineSight();
        }
    }


    // 정조준 상태 전환
    private void FineSight()
    {
        isFineSightMode = !isFineSightMode;
        currentGun.anim.SetBool("FineSightMode", isFineSightMode);

        theCrosshair.FineSightAnimation(isFineSightMode);

        if (isFineSightMode)
        {
            StopAllCoroutines();
            StartCoroutine(FineSightActivateCoroutine());
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(FineSightDeactivateCoroutine());
        }
    }


    // 정조준 상태를 해제하는 코루틴
    IEnumerator FineSightDeactivateCoroutine()
    {
        while (currentGun.transform.localPosition != originPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.2f);

            yield return null;
        }
    }


    // 정조준 상태로 들어가는 코루틴
    IEnumerator FineSightActivateCoroutine()
    {
        while (currentGun.transform.localPosition != currentGun.fineSightOriginPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.2f);

            yield return null;
        }
    }


    // 총기 반동 코루틴
    IEnumerator RetroActionCoroutine()
    {
        // 건홀더를 90도 돌려 서브머신건도 돌아간 상태
        // z 축이 앞뒤가 아닌 좌우가 되고
        // x 축이 앞뒤가 됨
        // x 값으로 반동, 앞으로가면 마이너스 뒤로가면 플러스

        if (!isFineSightMode)
        {
            // 반동이 이어지는 상태에서 또 반동이 들어오지 않게
            currentGun.transform.localPosition = originPos;

            // 반동 시작, 중간에 대충 일치하면 멈추게
            while (currentGun.transform.localPosition.x <= currentGun.retroActionForce - 0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition,recoilBack, 0.4f);
                
                yield return null;
            }

            // 원위치
            while (currentGun.transform.localPosition != originPos)
            {
                // 빠르게 돌아옴
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.1f);

                yield return null;
            }

        }
        else
        {
            // 반동이 이어지는 상태에서 또 반동이 들어오지 않게
            currentGun.transform.localPosition = currentGun.fineSightOriginPos;

            // 반동 시작, 중간에 대충 일치하면 멈추게
            while (currentGun.transform.localPosition.x <= currentGun.retroActionFineSightForce - 0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, retroActionRecoilBack, 0.4f);

                yield return null;
            }

            // 원위치
            while (currentGun.transform.localPosition != currentGun.fineSightOriginPos)
            {
                // 빠르게 돌아옴
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.1f);

                yield return null;
            }

        }
    }


    // 효과음 재생
    private void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }


    public Gun GetGun()
    {
        return currentGun;
    }

    
    public bool GetFineSightMode()
    {
        return isFineSightMode;
    }


    public void GunChange(Gun _gun)
    {
        // 먼저 있는 총 제거
        if (WeaponManager.currentWeapon != null)
            WeaponManager.currentWeapon.gameObject.SetActive(false);

        currentGun = _gun;

        WeaponManager.currentWeapon = currentGun.GetComponent<Transform>(); // 모든 객체는 Transform을 가짐
        WeaponManager.currentWeaponAnim = currentGun.anim;

        currentGun.transform.localPosition = Vector3.zero;      // 총 위치 초기화
        currentGun.gameObject.SetActive(true);

        isActivate = true;
    }
}
