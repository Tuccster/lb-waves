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
        version.text = $"Version {Application.version} running {Application.unityVersion}";
    }
}
