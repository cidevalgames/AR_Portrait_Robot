using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Vuforia;

public class PlayerTouch : MonoBehaviour
{
    private PlayerInput m_playerInput;

    private InputAction _touchPressAction;

    private Ray _ray;

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
        Debug.DrawRay(_ray.origin, _ray.direction * 100f, Color.yellow);
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
}
