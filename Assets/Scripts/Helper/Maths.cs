using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lemon
{
    public static class Maths
    {

        public static bool PointInCameraFrustum(Camera camera, Vector3 point)
        {
            Vector3 vp = camera.WorldToScreenPoint(point);
            if (vp.x > 0 && vp.x < Screen.width && vp.y > 0 && vp.y < Screen.height && vp.z > 0) return true;
            else return false;
        }

        public static float Truncate(this float value, int digits)
        {
            double m = Math.Pow(10.0, digits);
            double r = Math.Truncate(m * value) / m;
            return (float)r;
        }
    }
}
