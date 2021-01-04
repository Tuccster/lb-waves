using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lemon
{
    public static class Maths
    {
        // Should probably be moved to UnityHelper
        public static bool PointInCameraFrustum(Camera camera, Vector3 point)
        {
            Vector3 vp = camera.WorldToScreenPoint(point);
            if (vp.x > 0 && vp.x < Screen.width && vp.y > 0 && vp.y < Screen.height && vp.z > 0) return true;
            else return false;
        }
    }   
}
