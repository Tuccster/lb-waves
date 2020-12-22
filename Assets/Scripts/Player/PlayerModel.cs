using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Lemon
{
    public class PlayerModel : MonoBehaviourPunCallbacks
    {
        public MeshRenderer[] modelFirstPerson;
        public MeshRenderer[] modelThirdPerson;

        public void Awake()
        {
            for (int i = 0; i < modelFirstPerson.Length; i++)
                if (modelFirstPerson[i] != null)
                    modelFirstPerson[i].enabled = photonView.IsMine;

            for (int i = 0; i < modelThirdPerson.Length; i++)
                if (modelThirdPerson[i] != null)
                    modelThirdPerson[i].enabled = !photonView.IsMine;
        }
    }
}
