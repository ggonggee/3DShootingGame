using System;
using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.Rendering;


// �ΰ�����: ���ó�� �ȶ��ϰ� �ൿ�ϴ� �˰���
// - ������/��ȹ�� -> ��Ģ ��� �ΰ����� (�������� ���)
//               ->   �� ���(���ǹ�, �ݺ���)

public class Enemy : MonoBehaviour
{
    // 1. ���¸� ���������� �����Ѵ�.
    public enum EnemyState
    {
        Idle,
        patrol,
        Trace,
        Return,
        Attack,
        Damaged,
        Knockback,
        Die,
    }

    // 2. ���� ���¸� �����Ѵ�.
    public EnemyState CurrentState = EnemyState.Idle;

    private GameObject _player;                       // �÷��̾�
    private CharacterController _characterController; // ĳ���� ��Ʈ�ѷ�
    private Vector3 _startPosition;                   // ���� ��ġ

    public float FindDistance = 5f;     // �÷��̾� �߰� ����
    public float ReturnDistance = 5f;     // �� ���� ����
    public float AttackDistance = 2.5f;   // �÷��̾� ���� ����
    public float MoveSpeed = 3.3f;   // �̵� �ӵ�
    public float AttackCooltime = 2f;     // ���� ��Ÿ��
    private float _attackTimer = 0f;     // �� üũ��
    public int Health = 100;
    public float DamagedTime = 0.5f;   // ���� �ð�
    private float _damagedTimer = 0f;     // �� üũ��

    private float _knockbackForce = 30f;
    private float _knockbackDuration = 0.1f;
    private float _knockbackTimer = 0;


    //�����ð��� ������ �ٸ� ������ �̵��Ѵ�.
    [Header("��Ʈ��")]
    public Transform[] PatrolPoints;
    private float _patrolInterval = 3f;
    private float _patrolTimer;
    private int _nextPointNumber;


    private void Start()
    {
        _startPosition = transform.position;
        _characterController = GetComponent<CharacterController>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }


    private void Update()
    {
        // ���� ���� ���¿� ���� ���� �Լ��� ȣ���Ѵ�.
        switch (CurrentState)
        {
            case EnemyState.Idle:
                {
                    Idle();
                    break;
                }
            case EnemyState.patrol:
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

            case EnemyState.Damaged:
                {
                    Damaged();
                    break;
                }
            case EnemyState.Knockback:
                {
                    Knockback();
                    break;
                }

            case EnemyState.Die:
                {
                    Die();
                    break;
                }
        }
    }

    public void TakeDamage(Damage damage)
    {
        Health -= damage.Value;

        Debug.Log($"������ȯ: {CurrentState} -> Damaged");

        _damagedTimer = 0f;
        CurrentState = EnemyState.Damaged;
    }

    public void TakeKnockback(Damage damage)
    {
        Health -= damage.Value;

        Debug.Log($"���� ��ȯ {CurrentState} -> Knockback");
        _knockbackTimer = 0;
        CurrentState = EnemyState.Knockback;
    }


    // 3. ���� �Լ����� �����Ѵ�.

    private void Idle()
    {
        // �ൿ: ������ �ִ´�. 

        // ����: �÷��̾�� ����� ���� -> Trace
        if (Vector3.Distance(transform.position, _player.transform.position) < FindDistance)
        {
            Debug.Log("������ȯ: Idle -> Trace");
            CurrentState = EnemyState.Trace;
        }

        // ����: �ð��� ������ ��Ʈ���� �Ѵ�.
        _patrolTimer += Time.deltaTime;
        if(_patrolTimer > _patrolInterval)
        {
            _patrolTimer = 0;
            _nextPointNumber++;
            Debug.Log("������ȯ: Idle -> Patrol");
            CurrentState = EnemyState.patrol;
        }


    }
    private void Patrol()
    {
        // ����: �÷��̾�� ����� ���� -> Trace
        if (Vector3.Distance(transform.position, _player.transform.position) < FindDistance)
        {
            Debug.Log("������ȯ: Patrol -> Trace");
            CurrentState = EnemyState.Trace;
        }

        // �ൿ : ��Ʈ�� ����Ʈ�� �Դ� ���� �Ѵ�.
        // ����Ʈ�� �����ϸ� Idle�� ����.
        // idle���� �����ð� �ӹ����� Patrol�� �Ѿ��

        Vector3 dir = transform.position - PatrolPoints[_nextPointNumber].position;
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);
        if(Vector3.Distance( transform.position, PatrolPoints[_nextPointNumber].position) <= _characterController.minMoveDistance)
        {
            transform.position = PatrolPoints[_nextPointNumber].position;
            CurrentState = EnemyState.Idle;
        }
    }

    private void Trace()
    {
        // ����: �÷��̾�� �־����� -> Return
        if (Vector3.Distance(transform.position, _player.transform.position) > ReturnDistance)
        {
            Debug.Log("������ȯ: Trace -> Return");
            CurrentState = EnemyState.Return;
            return;
        }

        // ����: ���� ���� ��ŭ ����� ���� -> Attack
        if (Vector3.Distance(transform.position, _player.transform.position) < AttackDistance)
        {
            Debug.Log("������ȯ: Trace -> Attack");
            CurrentState = EnemyState.Attack;
            return;
        }

        // �ൿ: �÷��̾ �����Ѵ�.
        Vector3 dir = (_player.transform.position - transform.position).normalized;
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);
    }

    private void Return()
    {
        // ����: ���� ��ġ�� ����� ���� -> Idle
        if (Vector3.Distance(transform.position, _startPosition) <= _characterController.minMoveDistance)
        {
            Debug.Log("������ȯ: Return -> Idle");
            transform.position = _startPosition;
            CurrentState = EnemyState.Idle;
            return;
        }

        // ����: �÷��̾�� ����� ���� -> Trace
        if (Vector3.Distance(transform.position, _player.transform.position) < FindDistance)
        {
            Debug.Log("������ȯ: Return -> Trace");
            CurrentState = EnemyState.Trace;
        }


        // �ൿ: ���� ��ġ�� �ǵ��ư���.
        Vector3 dir = (_startPosition - transform.position).normalized;
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);
    }

    private void Attack()
    {
        // ����: ���� ���� ���� �־����� -> Trace
        if (Vector3.Distance(transform.position, _player.transform.position) >= AttackDistance)
        {
            Debug.Log("������ȯ: Attack -> Trace");
            CurrentState = EnemyState.Trace;
            _attackTimer = 0f;
            return;
        }

        // �ൿ: �÷��̾ �����Ѵ�.
        _attackTimer += Time.deltaTime;
        if (_attackTimer >= AttackCooltime)
        {
            Debug.Log("�÷��̾� ����!");
            _attackTimer = 0f;
        }
    }

    private void Damaged()
    {
        // �ൿ: ���� �ð����� �����ִٰ� -> Trace
        _damagedTimer += Time.deltaTime;
        if (_damagedTimer >= DamagedTime)
        {
            _damagedTimer = 0f;
            Debug.Log("������ȯ: Damaged -> Trace");
            CurrentState = EnemyState.Trace;
        }
    }

    private void Knockback()
    {
        // �ൿ: ������ ���ϸ� �ڷ� �з�����.
        Vector3 dir = (_player.transform.position - transform.position ).normalized;
        _characterController.Move(dir * -_knockbackForce * Time.deltaTime);
        _knockbackTimer += Time.deltaTime;
        if(_knockbackTimer > _knockbackDuration)
        {
            _knockbackTimer = 0;
            CurrentState = EnemyState.Trace;
        }
    }

    private void Die()
    {
        // �ൿ �״´�.
    }

}
