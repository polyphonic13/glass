using UnityEngine;
using System.Collections;

public class HeadPointer : MonoBehaviour {
	
	public Light laser;
	public float frontRay = 1.0f;
	
	private RaycastHit hit;
	private OVRCamera cameraRight;
	private Camera screenCamera;
	private CollectableItem rabbit;
	// Use this for initialization
	void Start () {
		cameraRight = GameObject.Find ("CameraRight").GetComponent<OVRCamera> ();
		rabbit = GameObject.Find ("rabbit_doll_holder").GetComponent<CollectableItem> ();
//		Debug.Log ("HeadPointer, cameraRight = " + cameraRight);
//		Debug.Log ("Camera.main = " + Camera.main);
		Debug.Log ("rabbit = " + rabbit);
	}
	
	// Update is called once per frame
	void Update () {

//		var mousePos = Input.mousePosition;
		var mousePos = Camera.main.ScreenPointToRay (Input.mousePosition);
//		Debug.Log ("mousePos = " + mousePos);
		var position = this.transform.position;

		rabbit.transform.position = (new Vector3(position.x + mousePos.direction.x + frontRay, position.y + mousePos.direction.y, position.z - frontRay));
		Debug.Log (rabbit.transform.position);
		//		this.transform.position = screenCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0));

//		Debug.DrawRay(this.transform.position, this.transform.forward * frontRay, Color.blue);

		/*
		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		
		if (Physics.Raycast(ray, out hit))
		{
			Debug.DrawRay(this.transform.position, this.transform.forward * frontRay, Color.blue);
			hit.collider.renderer.material.color = Color.red;
			//Debug.Log(hit);
		}
		*/
	}

	void OnMouseDown() 
	{
			var pos = Camera.main.ScreenPointToRay (Input.mousePosition);
			Debug.Log ("mouse down at: " + pos);
	}
}
