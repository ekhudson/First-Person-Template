using System;
using System.Collections.Generic;

using UnityEngine;

using Cinemachine;

public class MainCameraController : Singleton<MainCameraController>
{
    [SerializeField]
    private Camera m_MainCamera;
    public CinemachineVirtualCamera MainVirtualCamera;
    public CinemachineVirtualCamera ZoomVirtualCamera;

    public enum CameraStates
    {
        REGULAR,
        ZOOMED,
        SCOPED,
    }

    private CameraStates mCameraState = CameraStates.REGULAR;

    public CameraStates CameraState
    {
        get
        {
            return mCameraState;
        }
    }

    public Camera MainCameraReference
    {
        get
        {
            return m_MainCamera;
        }
    }
   
    public void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    ScopeVirtualCamera.enabled = true;
        //    mCameraState = CameraStates.SCOPED;
        //}

        //if (Input.GetKeyUp(KeyCode.Q))
        //{
        //    ScopeVirtualCamera.enabled = false;
        //    mCameraState = CameraStates.REGULAR;
        //}

        //if (Input.GetMouseButtonDown(1))
        //{
        //    ZoomVirtualCamera.enabled = true;
        //    mCameraState = CameraStates.ZOOMED;
        //}

        //if (Input.GetMouseButtonUp(1))
        //{
        //    ZoomVirtualCamera.enabled = false;
        //    mCameraState = CameraStates.REGULAR;
        //}
    }

    public void EnableZoom(bool enable)
    {
        ZoomVirtualCamera.enabled = enable;
        mCameraState = enable ? CameraStates.ZOOMED : CameraStates.REGULAR;
    }
}

