using UnityEngine;

public class CharacterIK : MonoBehaviour
{
    public Animator animator;
    public Transform leftHandTarget;

    void OnAnimatorIK(int layerIndex)
    {
        if (leftHandTarget == null || animator == null) return;

        // �޼� ��ġ/ȸ�� Ÿ�� ����
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);

        animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTarget.rotation);
    }
}

