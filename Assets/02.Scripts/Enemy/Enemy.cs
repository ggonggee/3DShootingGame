using System;
using System.Collections;

using Unity.Android.Gradle.Manifest;
using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public enum EnemyType
{
    Tracer,
    patroller,
}

// 인공지능: 사랑처럼 똑똑하게 행동하는 알고리즘
// - 반응형/계획형 -> 규칙 기반 인공지능 (전통적인 방식)
//               ->   ㄴ 제어문(조건문, 반복문)

public class Enemy : MonoBehaviour, IDamageable
{
    // 1. 상태를 열거형으로 정의한다.
    public enum EnemyState
    {
        Idle,
        Patrol,
        Trace,
        Return,
        Attack,
        Damaged,
        Knockback,
        Die,
        Count,
    }

    [Header("적타입")]
    public EnemyType Type = EnemyType.Tracer;
    // 2. 현재 상태를 지정한다.
    public EnemyState CurrentState = EnemyState.Idle;
    private GameObject _player;                       // 플레이어
    private CharacterController _characterController; // 캐릭터 컨트롤러
    private NavMeshAgent _agent;                      // 네비메쉬 에이젼드
    private Vector3 _startPosition;                   // 시작 위치

    public float FindDistance = 5f;     // 플레이어 발견 범위
    public float ReturnDistance = 5f;     // 적 복귀 범위
    public float AttackDistance = 1.5f;   // 플레이어 공격 범위
    public float MoveSpeed = 3.3f;   // 이동 속도
    public float AttackCooltime = 2f;     // 공격 쿨타임
    private float _attackTimer = 0f;     // ㄴ 체크기
    private float _attackDelay = 0.5f;

    public int CurrentHealth = 0;
    public int MaxHealth = 100;
    public int EnemyDamage = 10;
    public float DamagedTime = 0.5f;   // 경직 시간
    public float _dethTime = 5f;
    public float MinDistance = 0.1f;

    private float _knockbackForce = 30f;
    private float _knockbackDuration = 0.1f;
    private float _knockbackTimer = 0;

    [Header("HP")]
    public Slider HPSlider;

    //일정시간이 있으면 다른 곳으로 이동한다.
    [Header("패트롤")]
    public Transform[] PatrolPositions;
    private float _patrolInterval = 3f;
    private float _patrolTimer;
    private int _PatrolPositionIndex = 0;

    private Animator _enemyAnimator;
    private void Awake()
    {
        _enemyAnimator = transform.GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        if (Type == EnemyType.Tracer)
        {
            FindDistance = 10000;
            ReturnDistance = 10000;
            //CurrentState = EnemyState.Trace;
            //SetAnimation(EnemyState.Die);
        }

        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = MoveSpeed;

        _startPosition = transform.position;
        _characterController = GetComponent<CharacterController>();
        _player = GameObject.FindGameObjectWithTag("Player");
        CurrentHealth = MaxHealth;
        HPSlider.maxValue = MaxHealth;
        HPSlider.value = MaxHealth;
    }

    private void Update()
    {
        //사망했거나 공격받고 있는 중이라면..
        if (CurrentState == EnemyState.Die)
        //if (CurrentState == EnemyState.Damaged || CurrentState == EnemyState.Die)
        {
            return;
        }

        //if(Type  == EnemyType.Tracer)
        //{
        //    Vector3 dir = _player.transform.position - transform.position;
        //    _agent.SetDestination(_player.transform.position);
        //    return;
        //}

        HPSlider.transform.LookAt(Camera.main.transform);

        //나의 현재 상태에 따라 상태 함수를 호출한다.
        switch (CurrentState)
        {
            case EnemyState.Idle:
                {
                    Idle();

                    break;
                }
            case EnemyState.Patrol:
                {
                    Patrol();
                    break;
                }

            case EnemyState.Trace:
                {
                    Trace();
                    break;
                }

            case EnemyState.Return:
                {
                    Return();
                    break;
                }

            case EnemyState.Attack:
                {
                    Attack();
                    break;
                }

            case EnemyState.Knockback:
                {
                    Knockback();
                    break;
                }
        }
    }


    private void SetAnimation(EnemyState enemyState)
    {
        _enemyAnimator.SetTrigger(enemyState.ToSafeString());
    }

    public void TakeDamage(Damage damage)
    {
        Debug.Log("데미지를 받았다!");
        if (CurrentState == EnemyState.Die)
        {
            return;
        }

        CurrentHealth -= damage.Value;
        HPSlider.value = CurrentHealth;

        if (CurrentHealth <= 0)
        {
            Debug.Log($"상태전환: {CurrentState} -> Die");
            _agent.isStopped = true;
            _agent.ResetPath();
            _characterController.enabled = false;
            CurrentState = EnemyState.Die;
            SetAnimation(EnemyState.Die);
            StartCoroutine(Die_Coroutine());
            return;
        }

        Debug.Log($"상태전환: {CurrentState} -> Damaged");
        //_damagedTimer = 0f;
        //CurrentState = EnemyState.Damaged;
        SetAnimation(EnemyState.Damaged);
        //StartCoroutine(Damaged_Coroutine());
    }

    public void TakeKnockback(Damage damage)
    {
        CurrentHealth -= damage.Value;

        Debug.Log($"상태 전환 {CurrentState} -> Knockback");
        _knockbackTimer = 0;
        CurrentState = EnemyState.Knockback;
    }

    // 3. 상태 함수들을 구현한다.

    private void Idle()
    {
        // 행동: 가만히 있는다. 

        // 전이: 플레이어와 가까워 지면 -> Trace
        if (Vector3.Distance(transform.position, _player.transform.position) < FindDistance)
        {
            Debug.Log("상태전환: Idle -> Trace");
            CurrentState = EnemyState.Trace;
            SetAnimation(CurrentState);
        }

        // 전이: 시간이 지나면 패트롤을 한다.
        _patrolTimer += Time.deltaTime;
        if (_patrolTimer > _patrolInterval)
        {
            _patrolTimer = 0;
            Debug.Log("상태전환: Idle -> Patrol");
            
            _startPosition = PatrolPositions[_PatrolPositionIndex].position;
            _PatrolPositionIndex++;
            if (_PatrolPositionIndex >= PatrolPositions.Length)
            {
                _PatrolPositionIndex = 0;
            }
            CurrentState = EnemyState.Patrol;
        }
    }
    private void Patrol()
    {
        // 전이: 플레이어와 가까워 지면 -> Trace
        if (Vector3.Distance(transform.position, _player.transform.position) < FindDistance)
        {
            Debug.Log("상태전환: Patrol -> Trace");
            CurrentState = EnemyState.Trace;
            SetAnimation(CurrentState);
        }

        // 행동 : 페트롤 포인트를 왔다 갔다 한다.
        // 포인트를 도착하면 Idle로 간다.
        // idle에서 일정시간 머무르면 Patrol로 넘어간다
        Vector3 dir = (_startPosition - transform.position).normalized;
        //_characterController.Move(dir * MoveSpeed * Time.deltaTime);
        _agent.SetDestination(_startPosition);
        if (Vector3.Distance(transform.position, _startPosition) <= MinDistance)
        {
            Debug.Log("상태전환: Patrol -> Idle");
            transform.position = _startPosition;
            CurrentState = EnemyState.Idle;
            SetAnimation(CurrentState);
        }
    }

    private void Trace()
    {
        // 전이: 플레이어와 멀어지면 -> Return
        if (Vector3.Distance(transform.position, _player.transform.position) > ReturnDistance)
        {
            Debug.Log("상태전환: Trace -> Return");
            CurrentState = EnemyState.Return;
            SetAnimation(CurrentState);
            return;
        }

        // 전이: 공격 범위 만큼 가까워 지면 -> Attack
        if (Vector3.Distance(transform.position, _player.transform.position) < AttackDistance)
        {
            _agent.isStopped = true;
            _agent.ResetPath();
            Debug.Log("상태전환: Trace -> Attack");
            CurrentState = EnemyState.Attack;
            SetAnimation(CurrentState);
            return;
        }

        // 행동: 플레이어를 추적한다.
        Vector3 dir = (_player.transform.position - transform.position).normalized;
        //_characterController.Move(dir * MoveSpeed * Time.deltaTime);
        _agent.SetDestination(_player.transform.position);
    }

    private void Return()
    {
        // 전이: 시작 위치와 가까워 지면 -> Idle
        if (Vector3.Distance(transform.position, _startPosition) <= MinDistance)
        {
            Debug.Log("상태전환: Return -> Idle");
            transform.position = _startPosition;
            CurrentState = EnemyState.Idle;
            SetAnimation(CurrentState);
            return;
        }

        // 전이: 플레이어와 가까워 지면 -> Trace
        if (Vector3.Distance(transform.position, _player.transform.position) < FindDistance)
        {
            Debug.Log("상태전환: Return -> Trace");
            CurrentState = EnemyState.Trace;
            SetAnimation(CurrentState);
        }


        // 행동: 시작 위치로 되돌아간다.
        Vector3 dir = (_startPosition - transform.position).normalized;
        //_characterController.Move(dir * MoveSpeed * Time.deltaTime);
        _agent.SetDestination(_startPosition);

    }

    private void Attack()
    {
        // 전이: 공격 범위 보다 멀어지면 -> Trace
        if (Vector3.Distance(transform.position, _player.transform.position) >= AttackDistance)
        {
            Debug.Log("상태전환: Attack -> Trace");
            CurrentState = EnemyState.Trace;
            SetAnimation(CurrentState);
            _attackTimer = 0f;
            return;
        }

        _agent.isStopped = true;
        _agent.ResetPath();
        // 행동: 플레이어를 공격한다.
        _attackTimer -= Time.deltaTime;
        if (_attackTimer <= 0)
        {
            SetAnimation(CurrentState);
            _attackTimer = AttackCooltime;
            Debug.Log("상태전화: Attck -> Trace");
            CurrentState = EnemyState.Trace;
        }
    }

    //EnemyAttackEvent();
    public void EnemyAttackEvent()
    {
        if (_player.TryGetComponent<IDamageable>(out IDamageable idamageable))
        {
            Debug.Log("플레이어 공격!");
            Damage damage = new Damage();
            damage.Value = 10;
            damage.From = this.gameObject;
            idamageable.TakeDamage(damage);
        }
    }
    private void Knockback()
    {
        // 행동: 공격을 당하면 뒤로 밀려난다.
        Vector3 dir = (_player.transform.position - transform.position).normalized;
        _characterController.Move(dir * -_knockbackForce * Time.deltaTime);
        _knockbackTimer += Time.deltaTime;
        if (_knockbackTimer > _knockbackDuration)
        {
            _knockbackTimer = 0;
            CurrentState = EnemyState.Trace;
            SetAnimation(CurrentState);
        }
    }
    private IEnumerator Die_Coroutine()
    {

        //CharacterController controller = GetComponent<CharacterController>();
        //controller.enabled = false;
        yield return new WaitForSeconds(_dethTime);
        transform.gameObject.SetActive(false);
    }

}
