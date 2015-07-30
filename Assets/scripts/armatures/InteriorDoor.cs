using UnityEngine;
using System.Collections;

public class InteriorDoor : ArmatureParent {
	public Texture doorTexture;
	public Texture frameTexture;

	void Awake() {
		if(doorTexture != null) {
			var door = transform.Find("door");
			door.GetComponent<Renderer>().material.mainTexture = doorTexture;
		}
		
		if(frameTexture != null) {
			var frame = transform.Find("frame");
			frame.GetComponent<Renderer>().material.mainTexture = frameTexture;
		}
		Init();
	}
}
