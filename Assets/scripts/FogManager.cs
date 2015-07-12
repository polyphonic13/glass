using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class FogManager : MonoBehaviour {

	public Camera mainCamera;

	public Color normalColor;
	public float normalStartDistance = 0f;

	public Color underwaterColor;
	public float underwaterStartDistance = 2f; 

	private GlobalFog _fog;

	void Start () {
		EventCenter.Instance.OnUnderWater += this.OnUnderWater;
		_fog = mainCamera.GetComponent<GlobalFog>();
		_fog.distanceFog = true;
		this.OnUnderWater(false);
	}
	
	public void OnUnderWater(bool under) {
//		Debug.Log("FogManager/OnUnderWater, under = " + under);
		if(under) {
			_fog.enabled = true;
			RenderSettings.fogColor = underwaterColor;
			_fog.startDistance = underwaterStartDistance;
//			_fog.height = underwaterHeight;
//			_fog.heightDensity = underwaterHeightDensity;
		} else {
			_fog.enabled = true;
			RenderSettings.fogColor = normalColor;
			_fog.startDistance = normalStartDistance;
//			_fog.height = normalHeight;
//			_fog.heightDensity = normalHeightDensity;
		}
	}
	
}
