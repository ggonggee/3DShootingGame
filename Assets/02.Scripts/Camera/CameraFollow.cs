using UnityEngine;

enum CameraType
{
    FpsMode,
    TpsMode,
    QuaterMode
}

public class CameraFollow : MonoBehaviour
{
    public Transform FpsTarget;
    public Transform TpsTarget;
    public Transform QuarterTarget;
    private void Update()
    {
        // º¸°£(inter 
        transform.position = FpsTarget.position;

    }

    public void SetCamera()
    {

    }
}
