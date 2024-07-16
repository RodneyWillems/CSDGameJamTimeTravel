using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private LayerMask m_floorMask;
    [SerializeField] private Transform m_feet;
    [SerializeField] private InputActionReference m_look, m_movement, m_sprint;
    [SerializeField] private int m_movementSpeed, m_rotationSpeed, m_jumpForce;

    private Rigidbody m_rb;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        m_sprint.action.performed += StartSprint;
        m_sprint.action.canceled += StopSprint;
    }

    private void StartSprint(InputAction.CallbackContext context)
    {
        m_movementSpeed *= 2;
    }

    private void StopSprint(InputAction.CallbackContext context)
    {
        m_movementSpeed /= 2;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnJump()
    {
        if (Physics.CheckSphere(m_feet.position, 0.1f, m_floorMask))
        {
            m_rb.AddForce(Vector3.up * m_jumpForce, ForceMode.VelocityChange);
        }
    }

    private void FixedUpdate()
    {
        Vector3 movement = transform.TransformDirection(m_movement.action.ReadValue<Vector2>().x, 0, m_movement.action.ReadValue<Vector2>().y).normalized;
        m_rb.velocity = movement * m_movementSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, m_look.action.ReadValue<Vector2>().x * Time.deltaTime * m_rotationSpeed, 0));
    }
}
