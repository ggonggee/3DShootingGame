using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class PlayerFire : MonoBehaviour
{
    public GameObject FirePosition;
    public GameObject BombPrefab;
    public int BombCount;
    public int MaxBombCount = 10;

    public int BulletCount;
    public int MaxBulletCount = 50;
    public float ReloadInterval = 2f;
    public bool isReLoading;

    public float ShotInterval = 0.3f;
    public float ShotTimer = 0;


    public float MinThrowPower = 10f;  //최소 던지는 힘(예: 10f)
    public float MaxThrowPower = 25f;  //최대 던지는 힘(예: 25f)
    public float MaxHoldTime = 2f;    //최대 충전 시간(예: 2초) — 이 이상은 더 안 늘어남
    private float _holdStartTime; //마우스를 누른 시간(Time.time)을 저장
    private bool _isHolding;      //마우스를 누르고 있는 중인지 여부

    // - 던지는 힘
    public float ThrowPower = 15f;

    // 목표: 마우스 왼쪽 버튼을 누르면 카메라가 바라보는 방향으로 총을 발사하고 싶다. 

    public ParticleSystem BulletEffect;


    // 과제 1. 폭탄 개수 3개로 제한하기
    // - 하단에('현재 개수'/'최대 개수') 형태로 UI Text표시(TMP, 한글지원되는 폰트)

    // 과제 2. 마우스 오른쪽 누르고 있다가 때면 폭탄 더 멀리 날라가기
    // - 누르고 있는 시간에 비례(최대치가 있다.)

    // 과제 4. 마우스 왼쪽 버튼 누르고 있으면 연사하기
    // - 쿨타임 존재

    private void Start()
    {
        BombCount = MaxBombCount;
        BulletCount = MaxBulletCount;
        UIManager.Instance.SetBomb(BombCount, MaxBombCount);
        UIManager.Instance.SetBullet(BulletCount, MaxBulletCount);
        Cursor.lockState = CursorLockMode.Locked;
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _isHolding = true;
            _holdStartTime = Time.time;
        }

        // 2. 오른쪽 버튼 입력 받기
        // - 0: 왼쪽, 1: 오른쪽, 2: 휠
        if (Input.GetMouseButtonUp(1))
        {
            _isHolding = false;
            ThrowPower = MinThrowPower* Mathf.Clamp(Time.time - _holdStartTime, 0, MaxHoldTime) * 1.5f;
            ThrowPower = Mathf.Clamp(ThrowPower, MinThrowPower, MaxThrowPower);

            if(BombCount > 0)
            {
                GameObject bomb = PollingManager.Instance.GetBombPrefab();
                bomb.transform.position = FirePosition.transform.position;

                // 4. 생성된 슈류탄을 카메라 방향으로 물리적인 힘 가하기
                Rigidbody bombRigidbody = bomb.GetComponent<Rigidbody>();
                // 반드시 초기화!
                bombRigidbody.angularVelocity = Vector3.zero;
                bombRigidbody.linearVelocity = Vector3.zero; 
                bombRigidbody.AddForce(Camera.main.transform.forward * ThrowPower, ForceMode.Impulse);
                BombCount -= 1;
                UIManager.Instance.SetBomb(BombCount, MaxBombCount);
                }            // 3. 발사 위치에 수류탄 생성하기
        }

        if (!isReLoading)
        {
            if(Input.GetKeyDown(KeyCode.R)){
                BulletCount = MaxBulletCount;
                UIManager.Instance.SetBullet(BulletCount, MaxBulletCount);
            }
        }

        // 1. 왼쪽 버튼 입력 받기
        if (Input.GetMouseButton(0) && BulletCount > 0)
        {
            ShotTimer -= Time.deltaTime;
            if(ShotTimer < 0)
            {
                BulletCount--;
                UIManager.Instance.SetBullet(BulletCount, MaxBulletCount);
                ShotTimer = ShotInterval;

            // 2. 레이를 생성하고 발사 위치와 진행 방향을 설정
            Ray ray = new Ray(FirePosition.transform.position, Camera.main.transform.forward);
                
            // 3. 레이와 부딛힌 물체의 정보를 저장할 변수를 생성
            RaycastHit hitInfo = new RaycastHit();

            // 4. 레이저를 발사한 다음,                 -에 데이터가 있다면(부딧혔다면) 피격 이펙트 생성(표시)
            bool isHit = Physics.Raycast(ray, out hitInfo);
            if (isHit) //데이터가 있다면 (부딛혔다면
            {
                // 피격 이펙트 생성(표시)
                BulletEffect.transform.position = hitInfo.point;
                BulletEffect.transform.forward = hitInfo.normal; //법선 벡터: 직선에 대하여 수직인 벡터
                BulletEffect.Play();

                // 게임 수학: 선형대수학(스칼라, 벡터, 행렬), 기하학(삼각함수..)
                    
            }
            // Ray: 레이저( 시작위치, 방향)
            // RayCast : 레이저를 발사
            // RayCastHit: 레이저가 물체와 부딛혔다면 그 정보를 저장하는 구조체
            Debug.DrawRay(FirePosition.transform.position, Camera.main.transform.forward * 10f, Color.red, 2f);
            }
        }
    }
}
