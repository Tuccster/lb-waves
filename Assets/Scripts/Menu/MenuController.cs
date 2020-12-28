using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [Header("Resources")]
    public Text version;

    private void Start()
    {
        string buildID = Application.buildGUID;
        string shortBuildID = buildID.Substring(Mathf.Max(0, buildID.Length - 6));
        version.text = $"Version {Application.version} running {Application.unityVersion} (...{shortBuildID})";
    }
}
