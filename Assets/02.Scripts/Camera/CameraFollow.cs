using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;

    private void Update()
    {
        // ����(inter 
        transform.position = Target.position;

    }
}
