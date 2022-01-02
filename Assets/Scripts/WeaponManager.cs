using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[RequireComponent(typeof(GunController))]     // 실수 방지용
public class WeaponManager : MonoBehaviour
{
    // static var
    public static bool isChangeWeapon = false;      // 무기 중복 교체 실행 방지
    public static Transform currentWeapon;          // 현재 무기, 모든 객체의 기본 컴포넌트는 Transform
    public static Animator currentWeaponAnim;       // 현재 무기 애니메이션


    [SerializeField]
    private string currentWeaponType;       // 현재 무기 타입

    [SerializeField]
    private float changeWeaponDelayTime;    // 무기 교체 딜레이
    [SerializeField]
    private float changeWeaponEndDelayTime;    // 무기 교체 끝난 시점 딜레이


    // 무기 종류들 관리
    [SerializeField]
    private Gun[] guns;     // 총 배열
    [SerializeField]
    private CloseWeapon[] hands;    // 주먹 배열
    [SerializeField]
    private CloseWeapon[] axes;    // 도끼 배열
    [SerializeField]
    private CloseWeapon[] pickaxes;    // 곡괭이 배열


    // 관리를 쉽게하기 위한 딕셔너리
    private Dictionary<string, Gun> gunDict = new Dictionary<string, Gun>();
    private Dictionary<string, CloseWeapon> handDict = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> axeDict = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> pickaxeDict = new Dictionary<string, CloseWeapon>();


    // 필요한 컴포넌트
    [SerializeField]
    private GunController theGunController;
    [SerializeField]
    private HandController theHandController;
    [SerializeField]
    private AxeController theAxeController;
    [SerializeField]
    private PickaxeController thePickaxeController;



    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            gunDict.Add(guns[i].gunName, guns[i]);
        }

        for (int i = 0; i < hands.Length; i++)
        {
            handDict.Add(hands[i].closeWeaponName, hands[i]);
        }

        for (int i = 0; i < axes.Length; i++)
        {
            axeDict.Add(axes[i].closeWeaponName, axes[i]);
        }

        for (int i = 0; i < pickaxes.Length; i++)
        {
            pickaxeDict.Add(pickaxes[i].closeWeaponName, pickaxes[i]);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (!isChangeWeapon)
        {
            // 임시로 하드코딩하여 넣음
            if (Input.GetKeyDown(KeyCode.Alpha1))
                StartCoroutine(ChangeWeaponCoroutine("HAND", "맨손")); // 서브머신건으로 교체
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                StartCoroutine(ChangeWeaponCoroutine("GUN", "SubMachineGun1")); // 맨손으로 교체
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                StartCoroutine(ChangeWeaponCoroutine("AXE", "Axe")); // 도끼로 교체
            else if (Input.GetKeyDown(KeyCode.Alpha4))
                StartCoroutine(ChangeWeaponCoroutine("PICKAXE", "Pickaxe")); // 곡괭이로 교체
        }
    }

    
    // 무기를 바꾸는 코루틴
    public IEnumerator ChangeWeaponCoroutine(string _type, string _name)
    {
        isChangeWeapon = true;
        currentWeaponAnim.SetTrigger("Weapon_Out");

        yield return new WaitForSeconds(changeWeaponDelayTime);

        CancelPreWeaponAction();

        WeaponChange(_type, _name);

        yield return new WaitForSeconds(changeWeaponEndDelayTime);

        currentWeaponType = _type;

        isChangeWeapon = false;
    }


    // 현재 실행중인 행동을 끔
    private void CancelPreWeaponAction()
    {
        switch (currentWeaponType)
        {
            case "GUN":
                // 코루틴 스탑
                theGunController.CancelFineSight();
                theGunController.CancelReload();

                // 무기 비활성화
                GunController.isActivate = false;

                break;

            case "HAND":
                // 무기 비활성화 
                HandController.isActivate = false;

                break;

            case "AXE":
                // 무기 비활성화 
                AxeController.isActivate = false;

                break;

            case "PICKAXE":
                // 무기 비활성화 
                PickaxeController.isActivate = false;

                break;
        }
    }


    // 무기 변경
    private void WeaponChange(string _type, string _name)
    {
        if (_type == "GUN")
            theGunController.GunChange(gunDict[_name]);
        else if (_type == "HAND")
            theHandController.CloseWeaponChange(handDict[_name]);
        else if (_type == "AXE")
            theAxeController.CloseWeaponChange(axeDict[_name]);
        else if (_type == "PICKAXE")
            thePickaxeController.CloseWeaponChange(pickaxeDict[_name]);
    }

    // 물 속에선 무기 집어넣기
    public IEnumerator WeaponInCoroutine()
    {
        isChangeWeapon = true;
        currentWeaponAnim.SetTrigger("Weapon_Out");

        yield return new WaitForSeconds(changeWeaponDelayTime);

        currentWeapon.gameObject.SetActive(false);
    }

    // 물 밖에선 무기 꺼내기
    public void WeaponOut()
    {
        isChangeWeapon = false;

        currentWeapon.gameObject.SetActive(true);
    }
}
