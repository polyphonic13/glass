using UnityEngine;
using System.Collections;

public class SkyController : MonoBehaviour {

	public Transform stars;
	public MoonController moon; 

	public bool usingSunshine;
	public AnimationCurve sunshineCurve; 

	public AnimationCurve skyExposureCurve; 

	public Gradient nightDayColor;

	public float maxIntensity = 3f;
	public float minIntensity = 0f;
	public float minPoint = -0.2f; 

	public float maxAmbient = 1f;
	public float minAmbient = 0f;
	public float minAmbientPoint = -0.2f;

	public Gradient nightDayFogColor;
	public AnimationCurve fogDensityCurve;
	public float fogScale = 1f;

	public float dayAtmosphereThickness = 0.4f;
	public float nightAtmosphereThickness = 0.87f;

	public Vector3 dayRotationSpeed = new Vector3(-2, 0, 0);
	public Vector3 nightRotationSpeed = new Vector3(-2, 0, 0); 

	private Vector3 speed; 
	private float skySpeed = 1f;

	private float exposure;
	private Light mainLight;
	private Skybox sky;
	private Material skyMat;

	private string _currentState = "";
	private string _previousState = "";

	private bool _isJustChanged = false; 

	// Use this for initialization
	void Start () {
		mainLight = GetComponent<Light> ();
		skyMat = RenderSettings.skybox;
	}
	
	// Update is called once per frame
	void Update () {
		float tRange = 1 - minPoint;
		float dot = Mathf.Clamp01 ((Vector3.Dot (mainLight.transform.forward, Vector3.down) - minPoint) / tRange);
		float i = ((maxIntensity - minIntensity) * dot) + minIntensity;
		/*
		Debug.Log ("dot forward,down = " + Vector3.Dot (mainLight.transform.forward, Vector3.down)
			+ " - minPoint = " + (Vector3.Dot (mainLight.transform.forward, Vector3.down) - minPoint)
			+ ", dot = " + dot + ", tRange = " + tRange
			+ ", light intensity = " + i);
		*/
		mainLight.intensity = i;

		tRange = 1 - minAmbientPoint;
		dot = Mathf.Clamp01 ((Vector3.Dot (mainLight.transform.forward, Vector3.down) - minAmbientPoint) / tRange);
		i = ((maxAmbient - minAmbient) * dot) + minAmbient;
		RenderSettings.ambientIntensity = i;

		mainLight.color = nightDayColor.Evaluate (dot);
		RenderSettings.ambientLight = mainLight.color;

		RenderSettings.fogColor = nightDayFogColor.Evaluate (dot);
		RenderSettings.fogDensity = fogDensityCurve.Evaluate (dot) * fogScale;
	
//		Debug.Log ("ss curve = " + sunshineCurve.Evaluate (dot) + ", i = " + i + ", dot = " + dot);
		if (usingSunshine) {
			Sunshine.Instance.ScatterIntensity = sunshineCurve.Evaluate (dot);
			Sunshine.Instance.ScatterColor = RenderSettings.fogColor;
		}

		i = (((dayAtmosphereThickness - nightAtmosphereThickness) * dot) + nightAtmosphereThickness);
		skyMat.SetFloat("_AtmosphereThickness", i);

		exposure = skyExposureCurve.Evaluate (dot);
		Debug.Log("exposure = " + exposure + ", atmosphere thickness = " + i);
	
		skyMat.SetFloat("_Exposure", exposure);

		if(dot > 0) {
			_currentState = "day";
			speed = dayRotationSpeed;
		} else {
			_currentState = "night";
			speed = nightRotationSpeed;
		}

		this.transform.Rotate(speed * Time.deltaTime * skySpeed);
//		if (moon != null) {
//			moon.UpdateCycle (speed, skySpeed);
//		}

		if (_currentState != _previousState) {
			// dispatch new state
			EventCenter.Instance.ChangeDayNightState(_currentState);
			_previousState = _currentState;
		}

		stars.rotation = this.transform.rotation;

		if(Input.GetKeyDown(KeyCode.Q)) skySpeed *= 0.5f;
		if(Input.GetKeyDown(KeyCode.E)) skySpeed *= 2f;
	}
}
