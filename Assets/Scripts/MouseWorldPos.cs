using UnityEngine;

public class MouseWorldPosition : MonoBehaviour
{
    private Camera _mainCam;

    private void Awake()
    {
        _mainCam = Camera.main;
    }
}