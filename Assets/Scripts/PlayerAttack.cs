using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ToolType
{
    Axe,
    Pickaxe
}

public class PlayerAttack : MonoBehaviour
{
    public GameObject m_playerArm;
    public float m_bottomRotationRad;
    public float m_topRotationRad;
    public float m_downSpeed;
    public float m_upSpeed;

    public GameObject m_axeHead;
    public GameObject m_pickaxeHead;

    public Image m_toolAxeBGUI = null;
    public Image m_toolPickaxeBGUI = null;
    
    public TMP_Text m_toolAxeNumberUI = null;
    public TMP_Text m_toolPickaxeNumberUI = null;
    
    public TMP_Text m_toolAxeTextUI = null;
    public TMP_Text m_toolPickaxeTextUI = null;

    private ToolType m_activeTool = ToolType.Axe;
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
    private float m_oldRedValue;

    void Awake()
    {
        m_armTransform = m_playerArm.transform;
        m_playerInventory = GetComponent<PlayerInventory>();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_defaultRotationRad = m_armTransform.localRotation.x;
        UpdateToolUI(0.5f, 0.2f);

        Color bgColor = m_toolPickaxeBGUI.color;
        m_oldRedValue = bgColor.r;
        bgColor.r = 0.8f;
        m_toolPickaxeBGUI.color = bgColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (PowerGenerator.m_achievedStage > FurnaceStage.Stage1)
        {
            Color bgColor = m_toolPickaxeBGUI.color;
            bgColor.r = m_oldRedValue;
            m_toolPickaxeBGUI.color = bgColor;

            m_toolPickaxeTextUI.text = "Pickaxe";
        }
        
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

        if (Input.GetButton("FirstItem"))
        {
            m_activeTool = ToolType.Axe;
            m_axeHead.SetActive(true);
            m_pickaxeHead.SetActive(false);
            
            UpdateToolUI(0.5f, 0.2f);
        }
        else if (Input.GetButton("SecondItem") && PowerGenerator.m_achievedStage > FurnaceStage.Stage1)
        {
            m_activeTool = ToolType.Pickaxe;
            m_axeHead.SetActive(false);
            m_pickaxeHead.SetActive(true);

            UpdateToolUI(0.2f, 0.5f);
        }
        else if(Input.GetButton("ThirdItem"))
        {
            
        }
    }

    void UpdateToolUI(float axeAlpha, float pickaxeAlpha)
    {
        Color axeBGColor = m_toolAxeBGUI.color;
        axeBGColor.a = axeAlpha;
        m_toolAxeBGUI.color = axeBGColor;
            
        Color pickaxeBGColor = m_toolPickaxeBGUI.color;
        pickaxeBGColor.a = pickaxeAlpha;
        m_toolPickaxeBGUI.color = pickaxeBGColor;

        m_toolAxeNumberUI.alpha =axeAlpha;
        m_toolPickaxeNumberUI.alpha = pickaxeAlpha;

        m_toolAxeTextUI.alpha = axeAlpha;
        m_toolPickaxeTextUI.alpha = pickaxeAlpha;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);
        OnToolCollision(other, m_activeTool);
    }

    private void OnTriggerStay(Collider other)
    {
        OnToolCollision(other, m_activeTool);
    }

    private void OnCollisionEnter(Collision other)
    {
        if(!other.collider.CompareTag("TP"))
            return;
        
        Destroy(other.gameObject);
        m_playerInventory.AddTP();
    }

    public void OnToolCollision(Collider other, ToolType toolType)
    {
        FuelResource fuelResource = other.GetComponentInParent<FuelResource>();
        //Debug.Log("Dealt Damage: " + (m_dealtDamage ? "True" : "False"));
        //Debug.Log("Hit Box enabled: " + (m_hitBoxEnabled ? "True" : "False"));

        if (fuelResource && !m_dealtDamage && m_hitBoxEnabled)
        {
            GatherInfo gatherInfo = fuelResource.Gather(toolType);
            if (gatherInfo.amount == 0)
            {
                return;
            }
            
            m_dealtDamage = true;
            
            switch (gatherInfo.resourceType)
            {
                case ResourceType.Wood:
                {
                    m_playerInventory.AddWood(gatherInfo.amount);
                    break;
                }
                case ResourceType.Coal:
                {
                    m_playerInventory.AddCoal(gatherInfo.amount);
                    break;
                }
            }
        }
    }
}
