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
    public Vector3 OriginalPosition;
    private Camera cam;

    public bool ShotShake;


    private void Update()
    {
        if(CurrentCameraMode == CameraMode.FpsMode)
        {
        // 보간(inter 
            transform.position = FpsTarget.position;
            //transform.rotation = FpsTarget.rotation;
            OriginalPosition = transform.position;

        }
        if (CurrentCameraMode == CameraMode.TpsMode)
        {
            // 보간(inter 
            transform.position = TpsTarget.position;
            //transform.rotation = TpsTarget.rotation;
            OriginalPosition = transform.position;
        }
        if (CurrentCameraMode == CameraMode.QuaterMode)
        {
            // 보간(inter 
            transform.position = QuarterTarget.position;
            transform.rotation = QuarterTarget.rotation;
            OriginalPosition = transform.position;
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            CurrentCameraMode = CameraMode.FpsMode;
            HidePlayerHead();
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            CurrentCameraMode = CameraMode.TpsMode;
            ShowPlayerHead();
            
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            CurrentCameraMode = CameraMode.QuaterMode;
            ShowPlayerHead();
        }


        

    }

 


    public void HidePlayerHead()
    {
        int layer = LayerMask.NameToLayer("PlayerHead");
        Camera.main.cullingMask &= ~(1<<layer);
    }
    public void ShowPlayerHead()
    {
        int layer = LayerMask.NameToLayer("PlayerHead");
        Camera.main.cullingMask |= (1 << layer);
    }

    public void CameraShake()
    {
        float ran = Random.Range(0f, 0.02f);
        Vector3 randomPosition = new Vector3(transform.position.x + ran, transform.position.y + ran, transform.position.z + ran);
        transform.position = randomPosition;
        //bool comebackPosition = false;
        //while (!comebackPosition)
        //{
            //Vector3 direction = OriginalPosition - transform.position;
            //float distance = direction.magnitude;
            //Vector3.Lerp(OriginalPosition, transform.position, 0.5f);
            //if(distance < 0.1f)
            //{
            //    comebackPosition = true;
            //    transform.position = OriginalPosition;
            //}
        //}
    }

    public void SetCamera()
    {

    }
}
