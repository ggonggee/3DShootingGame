using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;

    private void Update()
    {
        // º¸°£(inter 
        transform.position = Target.position;

    }
}
