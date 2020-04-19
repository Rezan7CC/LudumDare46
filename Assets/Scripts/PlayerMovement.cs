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
    public float m_maxTiltAngle = 30.0f;
    public float m_tileSmoothTime = 2.0f;
    public float m_tiltMaxSpeed = 100.0f;
    public LayerMask m_mouseRayCastLayer;

    public Transform m_segwayBodySlotTransform;
    public Transform m_playerBodySlotTransform;

    private float m_tiltVelocity;
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

            // Segway Body Tilt
            {
                Quaternion segwayBodyRotation = m_segwayBodySlotTransform.localRotation;
                Vector3 segwayBodyAngles = segwayBodyRotation.eulerAngles;
                segwayBodyAngles.x = Mathf.SmoothDamp(segwayBodyAngles.x,
                    Mathf.Lerp(0, m_maxTiltAngle, currentSpeed.magnitude / m_maxSpeed),
                    ref m_tiltVelocity, m_tileSmoothTime, m_tiltMaxSpeed, Time.deltaTime);
                
                segwayBodyRotation.eulerAngles = segwayBodyAngles;
                m_segwayBodySlotTransform.localRotation = segwayBodyRotation;
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
            Vector2 currentVelocity = new Vector2(velocity.x, velocity.z);
            if (currentVelocity.magnitude > 0.1f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation,
                    Quaternion.LookRotation(new Vector3(currentVelocity.x, 0, currentVelocity.y), Vector3.up), m_maxRotationSpeed * Time.deltaTime);
            }
        }

        {
            Ray mouseRay = m_camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseRay, out RaycastHit hitInfo, 1000, m_mouseRayCastLayer.value))
            {
                Vector3 targetPosition = hitInfo.point;
                targetPosition.y = transform.position.y;

                Quaternion tiltRotation = Quaternion.FromToRotation(Vector3.up, m_segwayBodySlotTransform.up);
                
                Quaternion targetRotation =
                    Quaternion.LookRotation((targetPosition - transform.position).normalized, Vector3.up);

                m_playerBodySlotTransform.rotation = Quaternion.RotateTowards(m_playerBodySlotTransform.rotation,
                    tiltRotation * targetRotation, m_maxRotationSpeed * Time.deltaTime);
            }
        }
    }
}
