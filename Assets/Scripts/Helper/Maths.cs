using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lemon
{
    public static class Maths
    {
        public static bool PointInCameraFrustum(Camera camera, Vector3 point)
        {
            Vector3 vp = camera.ScreenToViewportPoint(point);
            if (vp.x > 0 && vp.x < 1 && vp.y > 0 && vp.y < 1 && vp.z > 0) return true;
            else return false;
        }
    }   
}
