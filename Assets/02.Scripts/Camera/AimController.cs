using UnityEngine;

public class AimController : MonoBehaviour
{
    public Transform gunPivot; // 총구가 회전할 축 (예: 팔이나 총 본체의 피벗)
    public Transform cameraTransform;

    void LateUpdate()
    {
        Vector3 lookDirection = cameraTransform.forward;

        // 카메라의 정면 방향을 기준으로 회전
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);

        // 총구를 상하 각도만 조정 (좌우는 안 따라가게 제한)
        Vector3 euler = targetRotation.eulerAngles;
        euler.y = gunPivot.rotation.eulerAngles.y; // 좌우 회전은 고정
        gunPivot.rotation = Quaternion.Euler(euler);
    }
}
