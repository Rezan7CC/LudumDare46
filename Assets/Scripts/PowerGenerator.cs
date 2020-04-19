using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum FurnaceStage
{
    Stage1,
    Stage2,
    Stage3
}

public class PowerGenerator : MonoBehaviour
{
    public float m_fuel = 0.2f;
    public float m_fuelTickDownRateStage1 = 0.01f;
    public float m_fuelTickDownRateStage2 = 0.01f;
    public float m_fuelTickDownRateStage3 = 0.01f;

    public float m_fuelTickDownDelaySStage1 = 10.0f;
    public float m_fuelTickDownDelaySStage2 = 10.0f;
    public float m_fuelTickDownDelaySStage3 = 10.0f;
    public float m_fuelUIConversionRate = 1200.0f;
    public static FurnaceStage m_currentStage = FurnaceStage.Stage1;
    public static FurnaceStage m_achievedStage = FurnaceStage.Stage1;

    public float m_minTPSpawnTime = 0.1f;
    public float m_maxTPSpawnTime = 0.2f;

    public float m_maxTPSpawnDistance = 200.0f;
    public float m_TPSpawnHeight = 50.0f;

    public GameObject m_TPPrefab = null;
    public TMP_Text m_fuelTextUI = null;
    public TMP_Text m_burnRateTextUI = null;
    public Image m_fillerUIStage1 = null;
    public Image m_fillerUIStage2 = null;
    public Image m_fillerUIStage3 = null;
    public TMP_Text m_Stage1TextUI = null;
    public TMP_Text m_Stage2TextUI = null;
    public TMP_Text m_Stage3TextUI = null;
    public GameObject m_gameWonScreen = null;
    public GameObject m_gameOverScreen = null;

    public Image m_storyTextBG = null;
    public TMP_Text m_storyTextStage1 = null;
    public TMP_Text m_storyTextStage2 = null;
    public TMP_Text m_storyTextStage3 = null;
    public float m_storyTextDisplayDurationS = 5.0f;
    
    public static PowerGenerator m_instance = null;

    private bool m_gameEnded = false;
    
    void Awake()
    {
        m_instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        UpdateStage(m_fuel);
        SetFuelTextUI(m_fuel);
        SetFuelFillUI(m_fuel);
        StartCoroutine(FuelTickDown());
        StartCoroutine(PlayStoryText());

        //AddFuel(0.85f);
    }

    IEnumerator PlayStoryText()
    {
        m_storyTextBG.gameObject.SetActive(true);
        
        switch (m_achievedStage)
        {
            case FurnaceStage.Stage1:
                m_storyTextStage1.gameObject.SetActive(true);
                break;
            case FurnaceStage.Stage2:
                m_storyTextStage2.gameObject.SetActive(true);
                break;
            case FurnaceStage.Stage3:
                m_storyTextStage3.gameObject.SetActive(true);
                break;
        }
        
        yield return new WaitForSeconds(m_storyTextDisplayDurationS);
        
        m_storyTextBG.gameObject.SetActive(false);
        m_storyTextStage1.gameObject.SetActive(false);
        m_storyTextStage2.gameObject.SetActive(false);
        m_storyTextStage3.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator FuelTickDown()
    {
        while (true)
        {
            switch (m_achievedStage)
            {
                case FurnaceStage.Stage1:
                {
                    yield return new WaitForSeconds(m_fuelTickDownDelaySStage1);
                    RemoveFuel(m_fuelTickDownRateStage1);
                } break;
                case FurnaceStage.Stage2:
                {
                    yield return new WaitForSeconds(m_fuelTickDownDelaySStage2);
                    RemoveFuel(m_fuelTickDownRateStage2);
                } break;
                case FurnaceStage.Stage3:
                {
                    yield return new WaitForSeconds(m_fuelTickDownDelaySStage3);
                    RemoveFuel(m_fuelTickDownRateStage3);
                } break;
            }
        }
    }

    IEnumerator TPSpawning()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(m_minTPSpawnTime, m_maxTPSpawnTime));

            Vector3 randomPos = Random.insideUnitSphere * m_maxTPSpawnDistance;
            randomPos.y = m_TPSpawnHeight;

            GameObject.Instantiate(m_TPPrefab, randomPos, Random.rotation);
        }
    }

    public void AddFuel(float fuelAmount)
    {
        if(m_gameEnded)
            return;
        
        m_fuel += fuelAmount;
        m_fuel = Mathf.Clamp01(m_fuel);

        UpdateStage(m_fuel);
        SetFuelTextUI(m_fuel);
        SetFuelFillUI(m_fuel);
        if (m_fuel >= 0.9999f)
        {
            StartCoroutine(OnGameWon());
            m_gameEnded = true;
        }
    }

    public void RemoveFuel(float fuelAmount)
    {
        if(m_gameEnded)
            return;
        
        m_fuel -= fuelAmount;
        m_fuel = Mathf.Clamp01(m_fuel);

        UpdateStage(m_fuel);
        SetFuelTextUI(m_fuel);
        SetFuelFillUI(m_fuel);
        if (m_fuel <= 0.0001f)
        {
            StartCoroutine(OnGameOver());
            m_gameEnded = true;
        }
    }

    void UpdateStage(float fuelAmount)
    {
        if (fuelAmount * m_fuelUIConversionRate > (m_fuelUIConversionRate / 3 * 2))
        {
            m_currentStage = FurnaceStage.Stage3;
            if (m_achievedStage != m_currentStage)
            {
                StopAllCoroutines();
                StartCoroutine(FuelTickDown());
                m_achievedStage = m_currentStage;
                StartCoroutine(TPSpawning());
                StartCoroutine(PlayStoryText());
            }
        }
        else if (fuelAmount * m_fuelUIConversionRate > m_fuelUIConversionRate / 3)
        {
            m_currentStage = FurnaceStage.Stage2;
        }
        else
        {
            m_currentStage = FurnaceStage.Stage1;
        }
        
        if (m_currentStage > m_achievedStage)
        {
            StopAllCoroutines();
            StartCoroutine(FuelTickDown());
            m_achievedStage = m_currentStage;
            StartCoroutine(PlayStoryText());
        }

        SetFuelFillUI(m_fuel);
        UpdateStageUI();
        UpdateBurnRateTextUI();
    }

    void SetFuelFillUI(float fuelAmount)
    {
        switch (m_currentStage)
        {
            case FurnaceStage.Stage1:
            {
                m_fillerUIStage1.fillAmount = (fuelAmount * m_fuelUIConversionRate) / (m_fuelUIConversionRate / 3);
                m_fillerUIStage2.fillAmount = 0.0f;
                m_fillerUIStage3.fillAmount = 0.0f;
            } break;
            case FurnaceStage.Stage2:
            {
                m_fillerUIStage1.fillAmount = 1.0f;
                float stage2StartAmount = m_fuelUIConversionRate / 3;
                m_fillerUIStage2.fillAmount = (fuelAmount * m_fuelUIConversionRate - stage2StartAmount) / (stage2StartAmount);
                m_fillerUIStage3.fillAmount = 0.0f;
            } break;
            case FurnaceStage.Stage3:
            {
                m_fillerUIStage1.fillAmount = 1.0f;
                m_fillerUIStage2.fillAmount = 1.0f;
                float stage3StartAmount = m_fuelUIConversionRate / 3 * 2;
                m_fillerUIStage3.fillAmount = (fuelAmount * m_fuelUIConversionRate - stage3StartAmount) / (m_fuelUIConversionRate / 3);
            } break;
        }
    }

    void SetFuelTextUI(float fuelAmount)
    {
        int fuelAmountUI = Mathf.RoundToInt(fuelAmount * m_fuelUIConversionRate);
        
        switch (m_achievedStage)
        {
            case FurnaceStage.Stage1:
            {
                m_fuelTextUI.text = "Fuel: " + fuelAmountUI + "/" + m_fuelUIConversionRate / 3;
            } break;
            case FurnaceStage.Stage2:
            {
                m_fuelTextUI.text = "Fuel: " + fuelAmountUI + "/" + (m_fuelUIConversionRate / 3 * 2);
            } break;
            case FurnaceStage.Stage3:
            {
                m_fuelTextUI.text = "Fuel: " + fuelAmountUI + "/" + m_fuelUIConversionRate;
            } break;
        }
    }
    
    void UpdateBurnRateTextUI()
    {
        switch (m_achievedStage)
        {
            case FurnaceStage.Stage1:
            {
                int seconds = Mathf.RoundToInt(m_fuelTickDownDelaySStage1);
                int burnRateUI = Mathf.RoundToInt(m_fuelTickDownRateStage1 * m_fuelUIConversionRate);
                m_burnRateTextUI.text = "Burn Rate: -" + burnRateUI + "/" + seconds + "s";
            } break;
            case FurnaceStage.Stage2:
            {
                int seconds = Mathf.RoundToInt(m_fuelTickDownDelaySStage2);
                int burnRateUI = Mathf.RoundToInt(m_fuelTickDownRateStage2 * m_fuelUIConversionRate);
                m_burnRateTextUI.text = "Burn Rate: -" + burnRateUI + "/" + seconds + "s";
            } break;
            case FurnaceStage.Stage3:
            {
                int seconds = Mathf.RoundToInt(m_fuelTickDownDelaySStage3);
                int burnRateUI = Mathf.RoundToInt(m_fuelTickDownRateStage3 * m_fuelUIConversionRate);
                m_burnRateTextUI.text = "Burn Rate: -" + burnRateUI + "/" + seconds + "s";
            } break;
        }
    }

    void UpdateStageUI()
    {
        switch (m_achievedStage)
        {
            case FurnaceStage.Stage1:
            {
                m_Stage1TextUI.alpha = 1.0f;
                m_Stage2TextUI.alpha = 0.2f;
                m_Stage3TextUI.alpha = 0.2f;
            } break;
            case FurnaceStage.Stage2:
            {
                m_Stage1TextUI.alpha = 1.0f;
                m_Stage2TextUI.alpha = 1.0f;
                m_Stage3TextUI.alpha = 0.2f;
            } break;
            case FurnaceStage.Stage3:
            {
                m_Stage1TextUI.alpha = 1.0f;
                m_Stage2TextUI.alpha = 1.0f;
                m_Stage3TextUI.alpha = 1.0f;
            } break;
        }
    }

    IEnumerator OnGameWon()
    {
        yield return new WaitForSeconds(0.0f);
        
        m_gameWonScreen.SetActive(true);
        Time.timeScale = 0.0f;
    }

    IEnumerator OnGameOver()
    {
        yield return new WaitForSeconds(0.0f);

        m_gameOverScreen.SetActive(true);
        Time.timeScale = 0.0f;
    }
}
