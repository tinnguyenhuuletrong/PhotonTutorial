using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityStandardAssets.Cameras;

class CameraManager
{
    public static void SetCameraTarget(BoltEntity Entity)
    {
        FreeLookCam lookCam = GameObject.Find("FreeLookCameraRig").GetComponent<FreeLookCam>();
        lookCam.SetTarget(Entity.gameObject.transform);
    }
}
