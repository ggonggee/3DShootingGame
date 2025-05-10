using UnityEngine;

public class GunAim : MonoBehaviour
{
    public Transform cameraTransform;
    public Transform gunPivot; // 총구 또는 총기 상단  Pivot
    //public Animator animator;

    // Update is called once per frame
    void Update()
    {
        Vector3 forward = cameraTransform.forward;
        Quaternion lookRotaton = Quaternion.LookRotation(forward);
        gunPivot.rotation = Quaternion.Lerp(gunPivot.rotation, lookRotaton, Time.deltaTime * 10f);
    }

    //private void OnAnimatorIK(int layerIndex)
    //{
    //    animator.SetLookAtWeight(1.0f);
    //    animator.SetLookAtPosition(cameraTransform.position + cameraTransform.forward * 100f);
    //}
}
