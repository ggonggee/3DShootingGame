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
            //transform.rotation = FpsTarget.rotation;

        }
        if (CurrentCameraMode == CameraMode.TpsMode)
        {
            // 보간(inter 
            transform.position = TpsTarget.position;
            //transform.rotation = TpsTarget.rotation;
        }
        if (CurrentCameraMode == CameraMode.QuaterMode)
        {
            // 보간(inter 
            transform.position = QuarterTarget.position;
            transform.rotation = QuarterTarget.rotation;
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            CurrentCameraMode = CameraMode.FpsMode;
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            CurrentCameraMode = CameraMode.TpsMode;
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            CurrentCameraMode = CameraMode.QuaterMode;
        }

    }

    public void SetCamera()
    {

    }
}
