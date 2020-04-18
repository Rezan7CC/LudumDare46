using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerGenerator : MonoBehaviour
{
    public float m_fuel = 0.2f;
    public float m_fuelTickDownRate = 0.025f;
    public float m_fuelTickDownDelayS = 3.0f; 

    public Image m_fillerUI = null;
    public GameObject m_gameWonScreen = null;
    public GameObject m_gameOverScreen = null;
    
    // Start is called before the first frame update
    void Start()
    {
        m_fillerUI.fillAmount = m_fuel;
        StartCoroutine(FuelTickDown());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FuelTickDown()
    {
        while (true)
        {
            RemoveFuel(m_fuelTickDownRate);
            yield return new WaitForSeconds(m_fuelTickDownDelayS);
        }
    }

    public void AddFuel(float fuelAmount)
    {
        m_fuel += fuelAmount;
        m_fuel = Mathf.Clamp01(m_fuel);
        
        m_fillerUI.fillAmount = m_fuel;
        if (m_fuel >= 1.0f)
        {
            OnGameWon();
        }
    }

    public void RemoveFuel(float fuelAmount)
    {
        m_fuel -= fuelAmount;
        m_fuel = Mathf.Clamp01(m_fuel);
        
        m_fillerUI.fillAmount = m_fuel;
        if (m_fuel <= 0.0f)
        {
            OnGameOver();
        }
    }

    void OnGameWon()
    {
        m_gameWonScreen.SetActive(true);
        Time.timeScale = 0.0f;
    }

    void OnGameOver()
    {
        m_gameOverScreen.SetActive(true);
        Time.timeScale = 0.0f;
    }
}
