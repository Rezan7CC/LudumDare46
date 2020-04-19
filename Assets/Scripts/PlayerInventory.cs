using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int m_maxWood = 250;
    public int m_maxCoal = 100;
    public int m_maxTP = 2400;
    public int m_TPAmountPerPickup = 100;

    public TMPro.TMP_Text m_woodUI;
    public TMPro.TMP_Text m_coalUI;
    public TMPro.TMP_Text m_tpUI;

    public int m_wood = 0;
    public int m_coal = 0;
    public int m_TP = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        UpdateWoodUI();
        UpdateCoalUI();
        UpdateTPUI();
    }

    // Update is called once per frame
    void Update()
    {
        switch (PowerGenerator.m_achievedStage)
        {
            case FurnaceStage.Stage1:
            {
                m_woodUI.gameObject.SetActive(true);
                m_coalUI.gameObject.SetActive(false);
                m_tpUI.gameObject.SetActive(false);
            } break;
            case FurnaceStage.Stage2:
            {
                m_woodUI.gameObject.SetActive(true);
                m_coalUI.gameObject.SetActive(true);
                m_tpUI.gameObject.SetActive(false);
            } break;
            case FurnaceStage.Stage3:
            {
                m_woodUI.gameObject.SetActive(false);
                m_coalUI.gameObject.SetActive(false);
                m_tpUI.gameObject.SetActive(true);
            } break;
        }
    }

    public void AddWood(int amount)
    {
        m_wood = Mathf.Clamp(m_wood + amount, 0, m_maxWood);
        UpdateWoodUI();
        //Debug.Log("Wood: " + m_wood + "/" + m_maxWood);
    }

    public void RemoveWood(int amount)
    {
        m_wood = Mathf.Clamp(m_wood - amount, 0, m_maxWood);
        UpdateWoodUI();
    }

    void UpdateWoodUI()
    {
        m_woodUI.text = "Wood: " + m_wood + "/" + m_maxWood;
    }
    
    public void AddCoal(int amount)
    {
        m_coal = Mathf.Clamp(m_coal + amount, 0, m_maxCoal);
        UpdateCoalUI();
        //Debug.Log("Wood: " + m_wood + "/" + m_maxWood);
    }

    public void RemoveCoal(int amount)
    {
        m_coal = Mathf.Clamp(m_coal - amount, 0, m_maxCoal);
        UpdateCoalUI();
    }

    void UpdateCoalUI()
    {
        m_coalUI.text = "Coal: " + m_coal + "/" + m_maxCoal;
    }

    public void AddTP()
    {
        m_TP = Mathf.Clamp(m_TP + m_TPAmountPerPickup, 0, m_maxTP);
        UpdateTPUI();
        //Debug.Log("Wood: " + m_wood + "/" + m_maxWood);
    }

    public void RemoveTP(int amount)
    {
        m_TP = Mathf.Clamp(m_TP - amount, 0, m_maxTP);
        UpdateTPUI();
    }

    void UpdateTPUI()
    {
        m_tpUI.text = "Toilet Paper: " + m_TP + "/" + m_maxTP;
    }
    
    private void OnTriggerStay(Collider other)
    {
        PowerGenerator powerGenerator = other.GetComponentInParent<PowerGenerator>();
        if (powerGenerator)
        {
            if (m_wood > 0)
            {
                powerGenerator.AddFuel(m_wood / PowerGenerator.m_instance.m_fuelUIConversionRate);
                RemoveWood(m_wood);
            }
            if (m_coal > 0)
            {
                powerGenerator.AddFuel(m_coal / PowerGenerator.m_instance.m_fuelUIConversionRate);
                RemoveCoal(m_coal);
            }
            if (m_TP > 0)
            {
                powerGenerator.AddFuel(m_TP / PowerGenerator.m_instance.m_fuelUIConversionRate);
                RemoveTP(m_TP);
            }
        }
    }
}
