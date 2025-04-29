using UnityEngine;

public class UpperBodyAim : MonoBehaviour
{
    public Transform upperArmBone; // 오른팔 어퍼암 뼈대
    public Transform aimTarget;    // 카메라의 포워드 방향으로 두는 임시 오브젝트

    void LateUpdate()
    {
        Vector3 aimDir = (aimTarget.position - upperArmBone.position).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(aimDir);
        upperArmBone.rotation = Quaternion.Slerp(
            upperArmBone.rotation,
            targetRotation,
            Time.deltaTime * 10f
        );
    }
}

