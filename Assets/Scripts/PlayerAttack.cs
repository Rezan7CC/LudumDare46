using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ToolType
{
    Axe
}

public class PlayerAttack : MonoBehaviour
{
    public GameObject m_playerArm;
    public float m_bottomRotationRad;
    public float m_topRotationRad;
    public float m_downSpeed;
    public float m_upSpeed;

    public ArmState m_armState = ArmState.Idle;
    private float m_defaultRotationRad;
    private Transform m_armTransform = null;
    private PlayerInventory m_playerInventory = null;

    public enum ArmState
    {
        Idle,
        MovingUp,
        MovingDown,
        MovingDefault,
    }

    private bool m_hitBoxEnabled = false;
    private bool m_dealtDamage = false;

    void Awake()
    {
        m_armTransform = m_playerArm.transform;
        m_playerInventory = GetComponent<PlayerInventory>();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_defaultRotationRad = m_armTransform.localRotation.x;
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion armRotation = m_armTransform.localRotation;
        
        if (Input.GetButton("Fire1"))
        {
            if (m_armState == ArmState.Idle)
            {
                m_armState = ArmState.MovingUp;
            }

            if (m_armState == ArmState.MovingUp)
            {
                Vector3 eulerAngles = armRotation.eulerAngles;
                eulerAngles.x = m_topRotationRad;
                Quaternion upRotation = Quaternion.Euler(eulerAngles);

                m_armTransform.localRotation =
                    Quaternion.RotateTowards(armRotation, upRotation, m_upSpeed * Time.deltaTime);
                if (Quaternion.Angle(m_armTransform.localRotation, upRotation) < 0.1f)
                {
                    m_armState = ArmState.MovingDown;
                    m_dealtDamage = false;
                }
            }
            else if (m_armState == ArmState.MovingDown)
            {
                Vector3 eulerAngles = armRotation.eulerAngles;
                eulerAngles.x = m_bottomRotationRad;
                Quaternion downRotation = Quaternion.Euler(eulerAngles);
                
                m_armTransform.localRotation = Quaternion.RotateTowards(armRotation, downRotation, m_downSpeed * Time.deltaTime);

                float angleDiff = Quaternion.Angle(m_armTransform.localRotation, downRotation);

                if (angleDiff < 45)
                {
                    m_hitBoxEnabled = true;
                }
                
                if (angleDiff < 0.1f)
                {
                    m_armState = ArmState.MovingUp;
                    m_hitBoxEnabled = false;
                }
            }
        }
        else
        {
            if (m_armState == ArmState.MovingUp || m_armState == ArmState.MovingDown)
            {
                m_armState = ArmState.MovingDefault;
            }

            if (m_armState == ArmState.MovingDefault)
            {
                Vector3 eulerAngles = armRotation.eulerAngles;
                eulerAngles.x = m_defaultRotationRad;
                Quaternion defaultRotation = Quaternion.Euler(eulerAngles);

                m_armTransform.localRotation =
                    Quaternion.RotateTowards(armRotation, defaultRotation, m_upSpeed * Time.deltaTime);
                if (Quaternion.Angle(m_armTransform.localRotation, defaultRotation) < 0.1f)
                {
                    m_armState = ArmState.Idle;
                }
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);
        OnToolCollision(other, ToolType.Axe);
    }

    private void OnTriggerStay(Collider other)
    {
        OnToolCollision(other, ToolType.Axe);
    }

    public void OnToolCollision(Collider other, ToolType axe)
    {
        FuelResource fuelResource = other.GetComponentInParent<FuelResource>();
        //Debug.Log("Dealt Damage: " + (m_dealtDamage ? "True" : "False"));
        //Debug.Log("Hit Box enabled: " + (m_hitBoxEnabled ? "True" : "False"));

        if (fuelResource && !m_dealtDamage && m_hitBoxEnabled)
        {
            GatherInfo gatherInfo = fuelResource.Gather();
            m_dealtDamage = true;
            
            switch (gatherInfo.resourceType)
            {
                case ResourceType.Wood:
                {
                    m_playerInventory.AddWood(gatherInfo.amount);
                    break;
                }
            }
        }
    }
}
