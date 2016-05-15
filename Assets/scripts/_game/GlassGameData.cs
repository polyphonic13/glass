using UnityEngine;
using System.Collections;
using Polyworks;

public class GlassGameData: GameData
{
	public float startingHealth = 100f; 
	public float startingBreath = 120f;
	public float startingStamina = 5f;

	public float remainingHealth;
	public float remainingBreath;
	public float remainingStamina;

	public string currentTargetScene = "";

	public bool hasFlashlight = false; 

	public int targetRoom = -1;

	public string loadingScene = "loading";
	public int loadingScenePause = 2; 

	public string[] playerScenes = {
		"house_floor02a",
		"cave01",
		"climb_test"
	};

}

