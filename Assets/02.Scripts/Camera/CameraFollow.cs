using UnityEngine;

public enum CameraMode
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

    public CameraMode CurrentCameraMode;
    

    private void Update()
    {
        if(CurrentCameraMode == CameraMode.FpsMode)
        {
        // 보간(inter 
            transform.position = FpsTarget.position;
        }
        if (CurrentCameraMode == CameraMode.TpsMode)
        {
            // 보간(inter 
            transform.position = TpsTarget.position;
        }
        if (CurrentCameraMode == CameraMode.QuaterMode)
        {
            // 보간(inter 
            transform.position = QuarterTarget.position;
        }

    }

    public void SetCamera()
    {

    }
}
