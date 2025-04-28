using UnityEngine;

public class EnemyAttackEvent : MonoBehaviour
{
    public Enemy MyEnemy;

    private void Start()
    {
        MyEnemy = GetComponentInParent<Enemy>();
    }

    public void AttackEvent()
    {
        //EnemyAttackEvent();
        Debug.Log("플레이어 공격!");
    }
}
