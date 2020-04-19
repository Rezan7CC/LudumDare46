using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
	public void RestartLevel()
	{
		StartCoroutine(RestartDelay());
	}

	static IEnumerator WaitForRealSeconds(float time)
	{
		float start = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup < start + time)
		{
			yield return null;
		}
	}
	
	IEnumerator RestartDelay()
	{
		yield return StartCoroutine(WaitForRealSeconds(0.5f));
		PowerGenerator.m_currentStage = FurnaceStage.Stage1;
		PowerGenerator.m_achievedStage = FurnaceStage.Stage1;
		Time.timeScale = 1.0f;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
