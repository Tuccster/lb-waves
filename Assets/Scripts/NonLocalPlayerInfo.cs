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
    
    private Vector3 screenPoint;

    private void Awake()
    {
        localPlayerCamera = Camera.main;
    }

    private void Update()
    {
        screenPoint = localPlayerCamera.WorldToScreenPoint(targetTransform.position + offset);
        usernameDisplay.rectTransform.position = localPlayerCamera.WorldToScreenPoint(targetTransform.position + offset);

        if (targetTransform == null) Destroy(gameObject);
    }

    public void SetTarget(string _targetUsername, Transform _targetTransform)
    {
        usernameDisplay.text = _targetUsername;
        targetTransform = _targetTransform;
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 64, 512, 32), $"targetTransform.position -> {targetTransform.position}");
        GUI.Label(new Rect(10, 64+32, 512, 32), $"targetTransform.position within frustum -> {Lemon.Maths.PointInCameraFrustum(localPlayerCamera, targetTransform.position)}");
    }
}
