using UnityEngine;

public class CharacterIK : MonoBehaviour
{
    public Animator animator;
    public Transform leftHandTarget;
    private PlayerFire PlayerFire;

    private void Start()
    {
        PlayerFire = GetComponentInParent<PlayerFire>();
    }

    void OnAnimatorIK(int layerIndex)
    {
        WeaponMode weaponMode = PlayerFire.GetWeaponMode();
        if (!(weaponMode == WeaponMode.Gun))
        {
            return;
        }

        if (leftHandTarget == null || animator == null) return;

        // 왼손 위치/회전 타겟 지정
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);

        animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTarget.rotation);
    }

    public void AttackEventt()
    {
        WeaponMode weaponMode = PlayerFire.GetWeaponMode();
        if (!(weaponMode == WeaponMode.Nife))
        {
            return;
        }

        PlayerFire.KnifeAttack();
    }

}

