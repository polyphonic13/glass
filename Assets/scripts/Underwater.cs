﻿using UnityEngine;
using System.Collections;

public class Underwater : MonoBehaviour {
	public float waterLevel;
	public Transform waterPlane;    //  Testing
	private bool isUnderwater;
	private Color normalColor;
	private Color underwaterColor;
	
	// Use this for initialization
	void Start () {
		normalColor = new Color (0.5f, 0.5f, 0.5f, 0.5f);
		underwaterColor = new Color (0.22f, 0.65f, 0.77f, 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
		if ((transform.position.y < waterLevel) != isUnderwater) {
			isUnderwater = transform.position.y < waterLevel;
			if (isUnderwater) SetUnderwater ();
			if (!isUnderwater) SetNormal ();
		}
	}
	
	void SetNormal () {
		RenderSettings.fogColor = normalColor;
		RenderSettings.fogDensity = 0.01f;
		
		//  Testing
		waterPlane.localScale = new Vector3 (waterPlane.localScale.x, 1.0f, waterPlane.localScale.z);
	}
	
	void SetUnderwater () {
		RenderSettings.fogColor = underwaterColor;
		RenderSettings.fogDensity = 0.1f;
		
		//  Testing
		waterPlane.localScale = new Vector3 (waterPlane.localScale.x, -1.0f, waterPlane.localScale.z);
	}
}
