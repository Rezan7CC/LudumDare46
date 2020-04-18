using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int m_maxWood = 250;

    public TMPro.TMP_Text m_woodUI;
    
    public int m_wood = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddWood(int amount)
    {
        m_wood = Mathf.Clamp(m_wood + amount, 0, m_maxWood);
        m_woodUI.text = "Wood: " + m_wood + "/" + m_maxWood;
        //Debug.Log("Wood: " + m_wood + "/" + m_maxWood);
    }

    public void RemoveWood(int amount)
    {
        m_wood = Mathf.Clamp(m_wood - amount, 0, m_maxWood);
        m_woodUI.text = "Wood: " + m_wood + "/" + m_maxWood;
    }

    private void OnTriggerStay(Collider other)
    {
        PowerGenerator powerGenerator = other.GetComponentInParent<PowerGenerator>();
        if (powerGenerator && m_wood > 0)
        {
            powerGenerator.AddFuel(m_wood * 0.001f);
            RemoveWood(m_wood);
        }
    }
}
