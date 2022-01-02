using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 스피드 조정 변수
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float swimSpeed;
    [SerializeField] private float swimFastSpeed;
    [SerializeField] private float UpSwimSpeed;


    private float applySpeed;   // 현재 속도


    [SerializeField]
    private float jumpForce;    // 점프의 크기


    // 상태 변수
    private bool isWalk = false;
    private bool isRun = false; // 달리기 상태 체크 변수
    private bool isCrouch = false; // 앉기 상태인지 체크 변수
    private bool isGround = true; // 땅위에 있는지 체크 변수


    // 움직임 체크 변수
    private Vector3 lastPos;    // 이전 프레임의 위치정보


    // 앉았을 때 얼마나 앉을지
    [SerializeField]
    private float crouchPosY;
    private float originPosY;   // 초기 높이
    private float applyCrounchPosY;


    // 땅과의 충돌을 체크하기 위해, 땅 착지 여부
    private CapsuleCollider capsuleCollider;


    // 카메라 민감도
    [SerializeField]
    private float loockSensitivity;


    // 카메라 한계
    [SerializeField]
    private float cameraRotationLimit;  // 카메라 회전 제한, 제한 안 하면 무한 회전함.
    private float currentCameraRotationX = 0f; // 정면


    // 컴포넌트
    [SerializeField]
    private Camera theCamera;
    private Rigidbody myRigid;
    [SerializeField]
    private GunController theGunController;
    [SerializeField]
    private HandController handController;      // 현재 장착된 Hand형 타입 무기
    private Crosshair theCrosshair;
    private StatusController theStatusController;



    // Start is called before the first frame update
    void Start()
    {
        // theCamera = FindObjectOfType<Camera>();     // 계층구조 내의 카메라 객체를 가져옴
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigid = GetComponent<Rigidbody>();
        theGunController = FindObjectOfType<GunController>();
        theCrosshair = FindObjectOfType<Crosshair>();
        theStatusController = FindObjectOfType<StatusController>();


        // 초기화
        applySpeed = walkSpeed;     // 걷는 것이 초기 상태
        originPosY = theCamera.transform.localPosition.y; // 카메라는 플레이어 기준이기 때문에 localPosition
        applyCrounchPosY = originPosY;

    }


    // Update is called once per frame
    void Update()
    {
        if (GameManager.canPlayerMove)
        {
            WaterCheck();
            IsGround();

            if (!GameManager.isWater)
            {
                TryRun();   // 달리는 상태인지 먼저 체크
            }

            TryCrouch();
            TryJump();
            Move();
            CameraRotation();
            CharacterRotation();
            /*
            // 인벤토리가 열려있으면 카메라 이동 X
            if (!Inventory.inventoryActivated)
            {
                CameraRotation();
                CharacterRotation();
            }
            */
        }
    }

    // 물 속일 때 스피드
    private void WaterCheck()
    {
        if (GameManager.isWater)
        {
            if (Input.GetKey(KeyCode.LeftShift))
                applySpeed = swimFastSpeed;
            else
                applySpeed = swimSpeed;
        }
    }

    private void FixedUpdate()
    {
        MoveCheck();
    }


    /*
     *  앉기 시도, 땅에서만 앉을 수 있음
     */
    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && isGround)
        {
            Crouch();
        }
    }


    /*
     *  앉기 실행
     */
    private void Crouch()
    {
        if (isWalk)
        {
            isWalk = false;
            theCrosshair.WalkingAnimation(isWalk);
        }

        isCrouch = !isCrouch;
        theCrosshair.CrouchingAnimation(isCrouch);
        RunningCancel();

        if (isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrounchPosY = crouchPosY;
        }
        else
        {
            applySpeed = walkSpeed;
            applyCrounchPosY = originPosY;
        }

        // 코루틴 할당
        StartCoroutine(CrouchCoroutine());
    }


    /*
     *  자연스러운 앉기를 위해 코루틴 선언
     *  동시성 처리
     */
    IEnumerator CrouchCoroutine()
    {
        float _posY = theCamera.transform.localPosition.y;
        int count = 0;

        while (_posY != applyCrounchPosY)
        {
            count++;

            // 선형이 아닌 보간으로 증가시킴, 30%씩 증가
            _posY = Mathf.Lerp(_posY, applyCrounchPosY, 0.3f);
            // 한개만 수정할 수 없기 때문에 백터를 만들어서 넣어줌
            theCamera.transform.localPosition = new Vector3(0, _posY, 0);

            // 15번만 실행, 무한 루프를 방지
            if (count > 15)
                break;

            yield return null; // 1프레임 대기
        }

        // 15번 실행 후 바로 apply로
        theCamera.transform.localPosition = new Vector3(0, applyCrounchPosY, 0);
    }


    // 지면 유저가 키 인풋을 발생시켰는지 체크
    private void IsGround()
    {
        // 객체 인식, 플래이어 객체의 중심에서 바닥으로 y만큼의 거리로
        // bounds = 캡슐의 크기, extents = 크기의 반, y값 사용, 0.1f 만큼의 여유
        // Vector3.down은 고정된 방향

        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.2f);
        theCrosshair.JumpingAnimation(!isGround);
    }


    /*
     *  점프가 가능한 상태인지 체크
     */
    private void TryJump()
    {
        // GetKeyDown = 한번만
        if (Input.GetKeyDown(KeyCode.Space) 
            && isGround 
            && theStatusController.GetCurrentSP() > 0 
            && !GameManager.isWater)
            Jump();
        else if (Input.GetKey(KeyCode.Space) 
            && GameManager.isWater)
            UpSwim();

    }

    // 물속에선 상승
    private void UpSwim()
    {
        myRigid.velocity = transform.up * UpSwimSpeed;
    }


    /*
     *  jumpForce 만큼 위치를 이동시킴, 점프 실행
     */
    private void Jump()
    {
        // 점프했을 때 앉아있다면 일어나게
        if (isCrouch)
            Crouch();

        theStatusController.DecreaseSP(100);

        myRigid.velocity = transform.up * jumpForce;
    }


    /*
     *  유저가 키 인풋을 발생시켰는지 체크
     */
    private void TryRun()
    {
        // GetKey = 키를 계속 누르고 있는 상태
        if (Input.GetKey(KeyCode.LeftShift) && !isCrouch && theStatusController.GetCurrentSP() > 0)
        {
            Running();
        }
        // GetkeyUp 키를 땐 상태
        if (Input.GetKeyUp(KeyCode.LeftShift) || theStatusController.GetCurrentSP() <= 0)
        {
            RunningCancel();
        }
    }


    /*
     *  플레이어 상태를 러닝 상태로
     */
    private void Running()
    {
        // 달리면 일어나게.
        if (isCrouch)
            Crouch();

        // 뛸 때 정조준 해제
        theGunController.CancelFineSight();

        isRun = true;
        theCrosshair.RunningAnimation(isRun);
        theStatusController.DecreaseSP(1);
        applySpeed = runSpeed;

    }


    /*
     *  플레이어 상태를 워킹 상태로
     */
    private void RunningCancel()
    {
        isRun = false;
        theCrosshair.RunningAnimation(isRun);
        applySpeed = walkSpeed;
    }


    /*
     *  플레이어의 움직임을 조절하는 함수
     */
    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");


        // 유니티에선 z축이 앞뒤, 방향 선언
        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        // 합이 1이 나오도록 정규화, 계산의 용이성을 위해
        // 방향 * 속도
        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;

        // 자연스러운 이동을 위해 delta time(0.016)을 곱함. 아니면 순간이동
        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);

    }


    // 이동이 있었는지 체크
    private void MoveCheck()
    {
        if (!isRun && !isCrouch && isGround)
        {
            // 경사로에 대한 여유를 줌
            if (Vector3.Distance(lastPos, transform.position) >= 0.02f)
                isWalk = true;
            else
                isWalk = false;


            theCrosshair.WalkingAnimation(isWalk);
            lastPos = transform.position;
        }

    }


    /*
     *  플레이어의 좌우 회전을 담당하는 함수
     */
    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * loockSensitivity;

        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));

    }


    /*
     *  카메라 상하 회전을 담당하는 함수
     */
    private void CameraRotation()
    {
        // 마우스는 2차원, X랑 Y만 있음
        float _xRotiation = Input.GetAxis("Mouse Y");   // 마우스 위 아래 == 유니티 x축
        float _cameraRotationX = _xRotiation * loockSensitivity;

        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        // 유니티는 회전을 쿼터니언으로 내부적으로 처리한다.
        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f); // Rotation x, y, z
    }

}
