using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{
    public CinemachineVirtualCamera VirtualCamera;
    public float zoomInSize = 2.25f;
    public float zoomOutSize = 5f;
    public float zoomSpeed = 2f;
    private bool isZoomOut = false;

    // Update is called once per frame
    void Update()
    {
        if(VirtualCamera == null)
        {
            return;
        }
        var lens = VirtualCamera.m_Lens;

        if(Input.GetKey(KeyCode.Y))
        {
            isZoomOut = true;
        }
        else
        {
            isZoomOut = false;
        }
        float targetSize = isZoomOut ? zoomOutSize : zoomInSize;
        lens.OrthographicSize = Mathf.Lerp(lens.OrthographicSize, targetSize, Time.deltaTime * zoomSpeed);

        // 변경 사항 반영
        VirtualCamera.m_Lens = lens;
    }
}
