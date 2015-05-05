using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class FogManager : MonoBehaviour {

	public Camera mainCamera;

	public Color normalColor;
	public float normalStartDistance = 0f;

	public Color underwaterColor;
	public float underwaterStartDistance = 5f; 

	private GlobalFog _fog;

	void Start () {
		EventCenter.Instance.onUnderWater += this.onUnderWater;
		_fog = mainCamera.GetComponent<GlobalFog>();
		_fog.distanceFog = true;
		this.onUnderWater(false);
	}
	
	public void onUnderWater(bool under) {
		Debug.Log("FogManager/onUnderWater, under = " + under);
		if(under) {
			_fog.enabled = true;
			RenderSettings.fogColor = underwaterColor;
			_fog.startDistance = underwaterStartDistance;
//			_fog.height = underwaterHeight;
//			_fog.heightDensity = underwaterHeightDensity;
		} else {
			_fog.enabled = false;
			RenderSettings.fogColor = normalColor;
			_fog.startDistance = normalStartDistance;
//			_fog.height = normalHeight;
//			_fog.heightDensity = normalHeightDensity;
		}
	}
	
}
