using UnityEngine;
using Cinemachine;

public class SyncAuxiliaryCamera : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera; // Assign your Cinemachine Virtual Camera here
    private Transform virtualCamTransform;

    void Start()
    {
        if (virtualCamera != null)
        {
            virtualCamTransform = virtualCamera.transform;
        }
    }

    void LateUpdate()
    {
        if (virtualCamTransform != null)
        {
            // Sync position and rotation of this camera to the Virtual Camera
            transform.position = virtualCamTransform.position;
            transform.rotation = virtualCamTransform.rotation;
        }
    }
}