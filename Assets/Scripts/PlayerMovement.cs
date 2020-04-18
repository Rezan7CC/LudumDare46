using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public float m_accelerationSpeed = 500.0f;
    public float m_maxSpeed = 100.0f;
    public float m_decelerationSpeed = 200.0f;
    public float m_maxRotationSpeed = 100.0f;
    public LayerMask m_mouseRayCastLayer;
    
    private Rigidbody m_rigidbody;
    private Camera m_camera;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_camera = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 velocity = m_rigidbody.velocity;
        
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        velocity.x += horizontalInput * m_accelerationSpeed * Time.deltaTime;
        velocity.z += verticalInput * m_accelerationSpeed * Time.deltaTime;

        {
            Vector2 currentSpeed = new Vector2(velocity.x, velocity.z);
            float currentSpeedLength = currentSpeed.magnitude;
            if (currentSpeedLength > m_maxSpeed)
            {
                currentSpeed = currentSpeed.normalized;
                currentSpeed *= m_maxSpeed;
                velocity.x = currentSpeed.x;
                velocity.z = currentSpeed.y;
            }
        }

        if (Mathf.Abs(horizontalInput) < 0.05f)
        {
            velocity.x = Mathf.MoveTowards(velocity.x, 0.0f, m_decelerationSpeed * Time.deltaTime);
        }
        if (Mathf.Abs(verticalInput) < 0.05f)
        {
            velocity.z = Mathf.MoveTowards(velocity.z, 0.0f, m_decelerationSpeed * Time.deltaTime);
        }
        
        m_rigidbody.velocity = velocity;

        {
            Ray mouseRay = m_camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseRay, out RaycastHit hitInfo, 1000, m_mouseRayCastLayer.value))
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation,
                    Quaternion.LookRotation((hitInfo.point - transform.position).normalized, Vector2.up), m_maxRotationSpeed);
            }
            
            // Vector2 currentSpeed = new Vector2(velocity.x, velocity.z);
            // if (currentSpeed.magnitude > 0.05f)
            // {
            //     transform.rotation = Quaternion.LookRotation(new Vector3(velocity.x, 0, velocity.z), Vector2.up);
            // }
        }
    }
}
