using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MeshRenderToggler : MonoBehaviourPunCallbacks
{
    public MeshRenderer[] renderers;

    private void Start()
    {
        if (!photonView.IsMine) return;
        foreach (MeshRenderer renderer in renderers)
            renderer.enabled = false;
    }
}
