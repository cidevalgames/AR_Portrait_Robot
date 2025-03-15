using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;
using Vuforia;

public class PlayerTouch : MonoBehaviour
{
    private PlayerInput m_playerInput;

    private InputAction _touchPressAction;

    private Ray _ray;

    private Collider _lastRaycastedObject;

    private void Awake()
    {
        m_playerInput = GetComponent<PlayerInput>();
        _touchPressAction = m_playerInput.actions["TouchPress"];
    }

    private void OnEnable()
    {
        _touchPressAction.performed += TouchPressed;
    }
    private void OnDisable()
    {
        _touchPressAction.performed -= TouchPressed;
    }

    private void Update()
    {
#if UNITY_EDITOR
        Debug.DrawRay(_ray.origin, _ray.direction * 100f, Color.yellow);
#endif
    }

    private void TouchPressed(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();

        _ray = Camera.main.ScreenPointToRay(value);

        if (Physics.Raycast(_ray, out RaycastHit hitInfo, 100f))
        {
            Debug.Log($"Touched object: {hitInfo.collider.name}");

            if (hitInfo.collider.GetComponent<ImageTargetBehaviour>())
            {
                MyTargetsManager.Instance.ChangeCurrentTarget(hitInfo.collider.GetComponent<MyImageTarget>());
            }
        }

        Debug.Log($"Input position: {value}");
    }

    public void OnTouchPosition(InputValue value)
    {
#if UNITY_EDITOR
        Vector2 position = value.Get<Vector2>();
        _ray = Camera.main.ScreenPointToRay(position);

        if (_lastRaycastedObject)
        {
            SetTrackIndicator(_lastRaycastedObject.transform, true);
        }
        
        _lastRaycastedObject = null;

        if (Physics.Raycast(_ray, out RaycastHit hitInfo, 100f))
        {
            Debug.Log($"Touched object: {hitInfo.collider.name}");
            _lastRaycastedObject = hitInfo.collider;

            if (hitInfo.collider.GetComponent<ImageTargetBehaviour>())
            {
                SetTrackIndicator(hitInfo.collider.transform, false);
            }
        }
#endif
    }

    private void SetTrackIndicator(Transform obj, bool enable)
    {
        MeshRenderer[] meshes = obj.GetComponentsInChildren<MeshRenderer>(true);

        foreach (MeshRenderer m in meshes)
        {
            if (m.name.Contains("Track_Indicator"))
            {
                m.enabled = enable;
            }
        }
    }
}
