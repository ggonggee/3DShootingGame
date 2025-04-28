using UnityEngine;

public class EnemyAttackEvent : MonoBehaviour
{
    private Enemy MyEnemy;

    private void Start()
    {
        MyEnemy = GetComponentInParent<Enemy>();
    }

    public void AttackEvent()
    {
        if (MyEnemy != null)
        {
            MyEnemy.EnemyAttackEvent();
            Debug.Log("플레이어 공격!");
        }
        else
        {
            Debug.LogWarning("Enemy  컴포넌트를 찾을수 없습니다.");
        }
    }
}
