using UnityEngine;

public class AimTargetFollower : MonoBehaviour
{
    public Transform cameraTransform;

    void Update()
    {
        transform.position = cameraTransform.position + cameraTransform.forward * 20f;
    }
}
