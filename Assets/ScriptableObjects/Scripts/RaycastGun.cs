using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lemon
{
    [CreateAssetMenu(fileName = "RaycastGun", menuName = "Weapons/RaycastGun", order = 1)]
    public class RaycastGun : ScriptableObject
    {
        [Header("General")]
        public string id;
        public string displayName;
        public int roundPerMinute;
        public float maxDamage;
        public float maxDistance;
        [Range(0.0000f, 100.0000f)]
        public float falloffPercentPerUnit;
        public float force;
        public bool automatic;
        
        [Header("Recoil")]
        public float rStrength;
        [Range(0, 10)] public int rFrames;

        [Header("Classification")]
        public RaycastGunType gunType;
        public enum RaycastGunType { Pistol, Rifle, Shotgun, Sniper, SMG }
        public RaycastGunPurpose gunPurpose;
        public enum RaycastGunPurpose { Primary, Secondary }

        [Header("Resources")]
        public AudioClip clipPullout;
        public AudioClip clipShoot;
        public AudioClip clipReload;
    }
}