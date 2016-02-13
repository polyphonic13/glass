using UnityEngine;
using System.Collections;

public class SunshineSampleMove : MonoBehaviour
{
	public bool Move = true;
	public Vector3 MoveVector = Vector3.up;
	public float MoveRange = 4.0f;
	public float MoveSpeed = 1f;

	public bool Spin = false;
	public float SpinSpeed = 20f;
	
	Vector3 startPosition;
	void Start()
	{
		startPosition = transform.position;
	}
	void Update()
	{
		if(Move)
			transform.position = startPosition + MoveVector * (MoveRange * Mathf.Sin(Time.timeSinceLevelLoad * MoveSpeed));
		if(Spin)
			transform.eulerAngles = new Vector3(0f, Time.timeSinceLevelLoad * SpinSpeed, 0f);
	}
}
