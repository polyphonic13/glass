using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class FogManager : MonoBehaviour {

	public Camera mainCamera;

	public Color normalColor;
	public float normalHeight = 0.001f;
	public float normalHeightDensity = 0.01f;

	public Color underwaterColor;
	public float underwaterHeight = 0.01f; 
	public float underwaterHeightDensity = 0.05f;

	private GlobalFog _fog;

	void Start () {
		EventCenter.Instance.onUnderWater += this.onUnderWater;
		_fog = mainCamera.GetComponent<GlobalFog>();
		this.onUnderWater(false);
	}
	
	public void onUnderWater(bool under) {
		Debug.Log("FogManager/onUnderWater, under = " + under);
		if(under) {
			RenderSettings.fogColor = underwaterColor;
			_fog.height = underwaterHeight;
			_fog.heightDensity = underwaterHeightDensity;
		} else {
			RenderSettings.fogColor = normalColor;
			_fog.height = normalHeight;
			_fog.heightDensity = normalHeightDensity;
		}
	}
	
}
