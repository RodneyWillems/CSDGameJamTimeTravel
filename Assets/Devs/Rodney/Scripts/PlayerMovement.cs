using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] private InputActionReference m_look;
    [SerializeField] private InputActionReference m_movement, m_sprint;

    [Header("Physics check")]
    [SerializeField] private LayerMask m_floorMask;
    [SerializeField] private Transform m_feet;

    [Header("Movement strengths")]
    [SerializeField] private int m_movementSpeed;
    [SerializeField] private int m_rotationSpeed, m_jumpForce;

    private Rigidbody m_rb;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        // performed is called when the button is pressed and canceled is called when the button is released
        m_sprint.action.performed += StartSprint;
        m_sprint.action.canceled += StopSprint;
    }

    private void OnDisable()
    {
        m_sprint.action.performed -= StartSprint;
        m_sprint.action.canceled -= StopSprint;
    }

    private void StartSprint(InputAction.CallbackContext context)
    {
        m_movementSpeed *= 2;
    }

    private void StopSprint(InputAction.CallbackContext context)
    {
        m_movementSpeed /= 2;
    }

    private void OnJump()
    {
        // When jump is pressed it first checks if you're on the floor before jumping
        if (Physics.CheckSphere(m_feet.position, 0.1f, m_floorMask))
        {
            m_rb.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        // Basic movement with rigidbody and new unity input system
        Vector3 movement = transform.TransformDirection(m_movement.action.ReadValue<Vector2>().x, 0, m_movement.action.ReadValue<Vector2>().y).normalized;
        m_rb.velocity = movement * m_movementSpeed;
    }

    void Update()
    {
        // Rotate when moving the mouse left/right
        transform.Rotate(new Vector3(0, m_look.action.ReadValue<Vector2>().x * Time.deltaTime * m_rotationSpeed, 0));
    }
}
