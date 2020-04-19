using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float m_smoothTime = 1.0f;
    public float m_maxSpeed = 1.0f;

    public float m_cameraHeightOffset = 6.2f;
    public Vector2 m_centerOffset = new Vector2(0, -5.5f);
    public Vector2 m_minOffset = new Vector2(-1, -3);
    public Vector2 m_maxOffset = new Vector2(1, 3);
    
    public Rigidbody m_playerRB;
    public PlayerMovement m_playerMovement;
    
    private Vector3 m_cameraVelocity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = m_playerRB.transform.position + new Vector3(m_centerOffset.x, 0, m_centerOffset.y);
        targetPosition.y = m_playerRB.transform.position.y + m_cameraHeightOffset;
        {
            Vector3 playerVelocity = m_playerRB.velocity;
            float maxSpeedRatioX = Math.Abs(playerVelocity.x) / m_playerMovement.m_maxSpeed;
            float maxSpeedRatioZ = Math.Abs(playerVelocity.z) / m_playerMovement.m_maxSpeed;

            if (playerVelocity.x > 0.05f)
            {
                targetPosition.x += Mathf.Lerp(0, m_maxOffset.x, maxSpeedRatioX);
            }
            else if (playerVelocity.x < -0.05f)
            {
                targetPosition.x += Mathf.Lerp(0, m_minOffset.x, maxSpeedRatioX);
            }

            if (playerVelocity.z > 0.05f)
            {
                targetPosition.z += Mathf.Lerp(0, m_maxOffset.y, maxSpeedRatioZ);
            }
            else if (playerVelocity.z < -0.05f)
            {
                targetPosition.z += Mathf.Lerp(0, m_minOffset.y, maxSpeedRatioZ);
            }
        }

        
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref m_cameraVelocity, m_smoothTime, m_maxSpeed,
            Time.deltaTime);
    }
}
