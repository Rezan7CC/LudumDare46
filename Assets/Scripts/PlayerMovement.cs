using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float m_accelerationSpeed = 500.0f;
    public float m_maxSpeed = 100.0f;
    public float m_decelerationSpeed = 200.0f;
    
    private Rigidbody m_rigidbody;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 velocity = m_rigidbody.velocity;
        
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        velocity.x += horizontalInput * m_accelerationSpeed * Time.deltaTime;
        velocity.z += verticalInput * m_accelerationSpeed * Time.deltaTime;

        Vector2 currentSpeed = new Vector2(velocity.x, velocity.z);
        float currentSpeedLength = currentSpeed.magnitude;
        if (currentSpeedLength > m_maxSpeed)
        {
            currentSpeed = currentSpeed.normalized;
            currentSpeed *= m_maxSpeed;
            velocity.x = currentSpeed.x;
            velocity.z = currentSpeed.y;
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
    }
}
