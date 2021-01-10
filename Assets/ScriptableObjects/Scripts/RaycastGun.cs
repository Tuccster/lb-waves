using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lemon
{
    [CreateAssetMenu(fileName = "RaycastGun", menuName = "Weapons/RaycastGun", order = 1)]
    public class RaycastGun : ScriptableObject
    {
        public string id;
        public string displayName;
        public float fireCooldown;
        public float maxDamage;
        public float maxDistance;
        public float falloffPercentPerUnit;
        public float force;
        public bool automatic;
        public enum RaycastGunType { Pistol, Rifle, Shotgun, Sniper, SMG }
        public RaycastGunType gunType;
        public enum RaycastGunPurpose { Primary, Secondary }
        public RaycastGunPurpose gunPurpose;
    }
}