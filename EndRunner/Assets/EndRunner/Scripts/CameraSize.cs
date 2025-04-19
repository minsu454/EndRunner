using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSize : MonoBehaviour
{
    private void Awake()
    {
        float orthoWidth = 5f / 720f * 1280f;
        Camera.main.orthographicSize = orthoWidth / Screen.width * Screen.height;
    }
}
