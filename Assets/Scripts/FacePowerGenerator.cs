using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FacePowerGenerator : MonoBehaviour
{
    public float rotationSpeed = 100.0f;
    public TMP_Text m_distanceText;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PowerGenerator.m_instance == null)
        {
            return;
        }

        Vector3 direction = PowerGenerator.m_instance.transform.position - transform.position;
        float distance = direction.magnitude;

        m_distanceText.text = Mathf.RoundToInt(distance).ToString() + "m";
        
        if (distance <= 0.0f)
        {
            return;
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation,
            Quaternion.LookRotation(direction / distance, Vector3.up), rotationSpeed);
    }
}
