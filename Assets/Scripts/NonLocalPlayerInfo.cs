using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NonLocalPlayerInfo : MonoBehaviour
{
    [Header("Settings")]
    public Vector3 offset;

    [Header("Resources")]
    public Text usernameDisplay;

    private Camera localPlayerCamera;
    private Transform targetTransform;

    private void Awake()
    {
        localPlayerCamera = Camera.main;
    }

    private void Update()
    {
        usernameDisplay.rectTransform.position = localPlayerCamera.WorldToScreenPoint(targetTransform.position + offset);
    }

    public void SetTarget(string _targetUsername, Transform _targetTransform)
    {
        usernameDisplay.text = _targetUsername;
        targetTransform = _targetTransform;
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 64, 512, 32), $"targetTransform.position -> {targetTransform.position}");
    }
}
