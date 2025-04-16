using Cinemachine;
using UnityEngine;

public class SwitchCameras : MonoBehaviour
{
    public CinemachineVirtualCamera Cam1Narrow;
    public CinemachineVirtualCamera Cam2Wide;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
            CameraManager.SwitchCamera(Cam1Narrow);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            CameraManager.SwitchCamera(Cam2Wide);
        }
    }
}
