using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class FogManager : MonoBehaviour {


	public Color _normalColor;
	public float _normalStartDistance;

	public Color _underwaterColor;
	public float _underwaterStartDistance = 2f; 

	private Camera _mainCamera;
	private GlobalFog _fog;

	void Start() {
		EventCenter.Instance.OnUnderWater += OnUnderWater;
		_mainCamera = Camera.main;
		_fog = _mainCamera.GetComponent<GlobalFog>();
		_fog.distanceFog = true;
		OnUnderWater(false);
	}
	
	public void OnUnderWater(bool under) {
		if(under) {
			_fog.enabled = true;
			RenderSettings.fogColor = _underwaterColor;
			_fog.startDistance = _underwaterStartDistance;
		} else {
			_fog.enabled = true;
			RenderSettings.fogColor = _normalColor;
			_fog.startDistance = _normalStartDistance;
		}
	}
	
}
