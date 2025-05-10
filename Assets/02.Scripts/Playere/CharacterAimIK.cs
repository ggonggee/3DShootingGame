using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAimIK : MonoBehaviour
{
    public Transform cameraTransform;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (cameraTransform == null) return;

        // 머리 방향 LookAt
        animator.SetLookAtWeight(1.0f);
        Vector3 lookTarget = cameraTransform.position + cameraTransform.forward * 100f;
        animator.SetLookAtPosition(lookTarget);

        // 상체 애니메이션 Blend용 pitch 값 전달
        float pitch = cameraTransform.eulerAngles.x;
        if (pitch > 180f) pitch -= 360f; // Normalize to -180 ~ +180
        pitch = Mathf.Clamp(pitch, -45f, 45f); // 제한 범위

        animator.SetFloat("AimPitch", pitch); // Blend Tree에서 사용
    }


}
