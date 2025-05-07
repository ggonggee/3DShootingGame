using System.Runtime.CompilerServices;

using UnityEngine;

using static Unity.Burst.Intrinsics.X86.Avx;

public enum WeaponMode
{
    Gun,
    Nife,
    Grenade,
    Count
}

public class PlayerFire : MonoBehaviour
{

    public WeaponMode CurrentWeaponMode;
    public GameObject FirePosition;
    public GameObject[] Weapon;
    private float _knifeDestance = 5f;
    private float _knifeAngle = 90f;

    [Header("총알")]
    public Bullet BulletPrefab;
    public Transform Muzzle;
    private int _damage = 10;
    public GameObject MuzzleFlash;

    [Header("탄약 장전")]
    public int BulletCount;
    public int MaxBulletCount = 50;
    public float ReloadInterval = 2f;
    public float ReloadTimer;
    public bool isReLoading;

    [Header("총발사 간격")]
    public float ShotInterval = 0.3f;
    public float ShotTimer = 0;
    private const float DebugRayLength = 10f;

    [Header("슈류탄 투척")]
    public GameObject BombPrefab;
    public int BombCount;
    public int MaxBombCount = 10;
    public float MinThrowPower = 10f;  //최소 던지는 힘(예: 10f)
    public float MaxThrowPower = 25f;  //최대 던지는 힘(예: 25f)
    public float MaxHoldTime = 2f;    //최대 충전 시간(예: 2초) — 이 이상은 더 안 늘어남
    public float ThrowPower = 15f; // 던지는 힘
    private float _holdStartTime; //마우스를 누른 시간(Time.time)을 저장
    private const float ThrowPowerMultiplier = 1.5f;

    public GameObject UI_CrosseHair;
    public GameObject UI_SniperZoom;
    private bool _zoomMode = false;
    public float ZoomInSize = 15f;
    public float ZoomOutSize = 60f;

    // 목표: 마우스 왼쪽 버튼을 누르면 카메라가 바라보는 방향으로 총을 발사하고 싶다. 

    public Animator _animator;

    // 과제 1. 폭탄 개수 3개로 제한하기
    // - 하단에('현재 개수'/'최대 개수') 형태로 UI Text표시(TMP, 한글지원되는 폰트)

    // 과제 2. 마우스 오른쪽 누르고 있다가 때면 폭탄 더 멀리 날라가기
    // - 누르고 있는 시간에 비례(최대치가 있다.)

    // 과제 4. 마우스 왼쪽 버튼 누르고 있으면 연사하기
    // - 쿨타임 존재

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        BombCount = MaxBombCount;
        BulletCount = MaxBulletCount;
        UIManager.Instance.SetBomb(BombCount, MaxBombCount);
        UIManager.Instance.SetBullet(BulletCount, MaxBulletCount);
        Cursor.lockState = CursorLockMode.Locked;
        SetWeaponModeAnimation(CurrentWeaponMode);
        
    }


    public void SetWeaponModeAnimation(WeaponMode weaponMode)
    {
        if (weaponMode == WeaponMode.Gun)
        {
            _animator.SetBool("AttackMode", true);
            _animator.SetBool("NifeMode", false);
            _animator.SetBool("ThrowMode", false);
        }
        if (weaponMode == WeaponMode.Nife)
        {
            //if (Input.GetMouseButtonDown(0) && !animator.GetCurrentAnimatorStateInfo(0).IsName("KnifeAttack"))
            //{
            //    animator.SetTrigger("KnifeSwing");
            //}
            _animator.SetBool("AttackMode", false);
            _animator.SetBool("NifeMode", true);
            _animator.SetBool("ThrowMode", false);
        }
        if (weaponMode == WeaponMode.Grenade)
        {
            _animator.SetBool("AttackMode", false);
            _animator.SetBool("NifeMode", false);
            _animator.SetBool("ThrowMode", true);
        }
        UIManager.Instance.SetCurrentWeapon(CurrentWeaponMode);
        SetWeapon();
    }

    private void SetWeapon()
    {
        for (int i = 0; i < Weapon.Length; i++)
        {
            Weapon[i].SetActive((int)CurrentWeaponMode == i);
        }
    }

    public WeaponMode GetWeaponMode()
    {
        return CurrentWeaponMode;
    }

    public void KnifeAttack()
    {
        //Animator ani = Weapon[(int)WeaponMode.Nife].GetComponent<Animator>();
        //ani.SetTrigger("Attack");
        Collider[] cols = Physics.OverlapSphere(transform.position, _knifeDestance, LayerMask.GetMask("Enemy"));

        for (int i = 0; i < cols.Length; i++)
        {
            Vector3 target = cols[i].transform.position - transform.position;
            target.y = 0; // 높이 차이는 무시할 경우

            if (cols[i].CompareTag("Enemy"))
            {
                float angle = Vector3.Angle(transform.forward, target);
                if (angle > _knifeAngle * 0.5f)
                {
                    Debug.Log("칼이 안닿는다");
                    return;
                }

                if (cols[i].TryGetComponent<IDamageable>(out IDamageable damageAble))
                {
                    Debug.Log("칼 맞았다!");
                    Damage damage = new Damage();
                    damage.Value = 100;
                    damageAble.TakeDamage(damage);
                }
            }
        }
    }

    private void Update()
    {

        float scroll = Input.GetAxisRaw("Mouse ScrollWheel");
        if (scroll > 0 )
        {
            Debug.Log("스크롤 플러스");
           int next = ((int)CurrentWeaponMode +1) % (int)WeaponMode.Count;
            CurrentWeaponMode = (WeaponMode)next;
            SetWeaponModeAnimation(CurrentWeaponMode);
        }
        else if( scroll < 0)
        {
            Debug.Log("스크롤 마이너스");
            int prev = ((int)CurrentWeaponMode - 1) % (int)WeaponMode.Count;
            CurrentWeaponMode = (WeaponMode)prev;
            SetWeaponModeAnimation(CurrentWeaponMode);
        }

        if (GameManager.Instance.CurrentGameMove != GameMode.Run)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CurrentWeaponMode = WeaponMode.Gun;
            SetWeaponModeAnimation(CurrentWeaponMode);
            Debug.Log("넘버키 1이 눌러 졌다");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CurrentWeaponMode = WeaponMode.Nife;
            SetWeaponModeAnimation(CurrentWeaponMode);
            Debug.Log("넘버키 2이 눌러 졌다");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CurrentWeaponMode = WeaponMode.Grenade;
            SetWeaponModeAnimation(CurrentWeaponMode);
            Debug.Log("넘버키 3이 눌러 졌다");
        }

        if (Input.GetMouseButtonDown(1))
        {
            //_holdStartTime = Time.time;

            UI_CrosseHair.SetActive(false);
            UI_SniperZoom.SetActive(true);
            Camera.main.fieldOfView = ZoomOutSize;

        }

        // 2. 오른쪽 버튼 입력 받기
        // - 0: 왼쪽, 1: 오른쪽, 2: 휠
        if (Input.GetMouseButtonUp(1))
        {
            UI_CrosseHair.SetActive(true);
            UI_SniperZoom.SetActive(false);
            Camera.main.fieldOfView = ZoomInSize;
        }

        if (!isReLoading)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                isReLoading = true;
                ReloadTimer = 0;
            }
        }

        if (isReLoading)
        {
            ReloadTimer += Time.deltaTime;
            if (ReloadTimer > ReloadInterval)
            {
                isReLoading = false;
                ReloadTimer = 0;
                BulletCount = MaxBulletCount;
                UIManager.Instance.SetBullet(BulletCount, MaxBulletCount);
            }
            UIManager.Instance.SetReload(ReloadTimer, ReloadInterval, isReLoading);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {

        }

        if (Input.GetKeyDown(0))
        {
            if (CurrentWeaponMode == WeaponMode.Grenade)
            {
                _holdStartTime = Time.deltaTime;
            }
        }
        if (Input.GetKeyDown(0))
        {
            if (CurrentWeaponMode == WeaponMode.Grenade)
            {
                _animator.SetTrigger("Throw");
                ThrowPower = MinThrowPower * Mathf.Clamp(Time.time - _holdStartTime, 0, MaxHoldTime) * ThrowPowerMultiplier;
                ThrowPower = Mathf.Clamp(ThrowPower, MinThrowPower, MaxThrowPower);

                if (BombCount > 0)
                {
                    GameObject bomb = PollingManager.Instance.GetBombPrefab();
                    bomb.transform.position = FirePosition.transform.position;

                    //4.생성된 슈류탄을 카메라 방향으로 물리적인 힘 가하기
                    Rigidbody bombRigidbody = bomb.GetComponent<Rigidbody>();
                    //반드시 초기화!
                    bombRigidbody.angularVelocity = Vector3.zero;
                    bombRigidbody.linearVelocity = Vector3.zero;
                    bombRigidbody.AddForce(Camera.main.transform.forward * ThrowPower, ForceMode.Impulse);
                    BombCount -= 1;
                    UIManager.Instance.SetBomb(BombCount, MaxBombCount);
                }
                // 3. 발사 위치에 수류탄 생성하기             
                return;
            }
        }


        // 1. 왼쪽 버튼 입력 받기
        if (Input.GetMouseButton(0))
        {
            if(CurrentWeaponMode == WeaponMode.Nife)
            {
                if (Input.GetMouseButtonDown(0) && !_animator.GetCurrentAnimatorStateInfo(0).IsName("KnifeAttack"))
                {
                    _animator.SetTrigger("KnifeAttack");
                }
            }

        if (CurrentWeaponMode == WeaponMode.Gun && BulletCount > 0)
        {
            //_animator.SetTrigger("Shot");
            ShotTimer -= Time.deltaTime;

            if (ShotTimer < 0)
            {
                BulletCount--;
                UIManager.Instance.SetBullet(BulletCount, MaxBulletCount);
                ShotTimer = ShotInterval;
                isReLoading = false;
                UIManager.Instance.SetReload(ReloadTimer, ReloadInterval, isReLoading);

                Vector3 targetPoint = Camera.main.transform.position + Camera.main.transform.forward * 100f;
                Vector3 shotDir = (targetPoint - Muzzle.position).normalized;
                Bullet bullet = Instantiate(BulletPrefab);
                Instantiate(MuzzleFlash, Muzzle.position, Quaternion.identity);
                bullet.transform.position = Muzzle.position;
                bullet.transform.forward = shotDir;
                Damage damage = new Damage();
                damage.Value = _damage;
                damage.From = this.gameObject;
                bullet.Damage = damage;

                // 2. 레이를 생성하고 발사 위치와 진행 방향을 설정
                //Ray ray = new Ray(FirePosition.transform.position, Camera.main.transform.forward);
                //Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

                //if(Physics.Raycast(ray, out RaycastHit hit, 100f))
                //{
                //    Vector3 shootDir = (hit.point - Muzzle.transform.position).normalized;
                //    GameObject bullet = Instantiate(BulletPrefab);
                //    bullet.transform.position = Muzzle.transform.position;
                //    bullet.transform.forward = shootDir;
                //}


                //// 3. 레이와 부딛힌 물체의 정보를 저장할 변수를 생성
                //RaycastHit hitInfo = new RaycastHit();

                //// 4. 레이저를 발사한 다음,                 -에 데이터가 있다면(부딧혔다면) 피격 이펙트 생성(표시)
                //bool isHit = Physics.Raycast(ray, out hitInfo);
                //if (isHit) //데이터가 있다면 (부딛혔다면
                //{
                //    // 피격 이펙트 생성(표시)
                //    BulletEffect.transform.position = hitInfo.point;
                //    BulletEffect.transform.forward = hitInfo.normal; //법선 벡터: 직선에 대하여 수직인 벡터
                //    BulletEffect.Play();

                //    //if (hitInfo.collider.gameObject.CompareTag("Enemy"))
                //    // 총알을 맞은 친구가 IDamageable 구현체라면...
                //    if (hitInfo.collider.TryGetComponent<IDamageable>(out IDamageable damageable))
                //    {
                //        Damage damage = new Damage();
                //        damage.Value = 10;
                //        damage.From = this.gameObject;
                //        damageable.TakeDamage(damage);
                //    }
                //    // 게임 수학: 선형대수학(스칼라, 벡터, 행렬), 기하학(삼각함수..)
                //}
                //// Ray: 레이저( 시작위치, 방향)
                //// RayCast : 레이저를 발사
                //// RayCastHit: 레이저가 물체와 부딛혔다면 그 정보를 저장하는 구조체
                //Debug.DrawRay(FirePosition.transform.position, Camera.main.transform.forward * DebugRayLength, Color.red, 2f);
            }
        }
        }
    }
}

