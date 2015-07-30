using UnityEngine;
using System.Collections;

public class InvertedCamera : MonoBehaviour {
	public int forward = 1;
	public int up = 1;
	public int right = 1;

	public string containingRoom = "";

	void Start() {
		this.gameObject.SetActive (false);

		var eventCenter = EventCenter.Instance;
		eventCenter.onRoomEntered += this.onRoomEntered;
		eventCenter.onRoomExited += this.onRoomExited;
	}

	// EXAMPLE WITH CAMERA UPSIDEDOWN
	void OnPreCull () {
//		Debug.Log ("InvertedCamera/OnPreCull");
		camera.ResetWorldToCameraMatrix();
		camera.ResetProjectionMatrix();
		camera.projectionMatrix = camera.projectionMatrix * Matrix4x4.Scale(new Vector3(forward, up, right));
	}
	
	void OnPreRender () {
		GL.SetRevertBackfacing(true);
	}
	
	void OnPostRender () {
		GL.SetRevertBackfacing(false);
	}
	

	public virtual void onRoomEntered(string room) {
		Debug.Log("InvertedCamera["+this.name+"]/onRoomEntered: " + room + ", containingRoom: " + this.containingRoom);
		if(room == this.containingRoom) {
//			this.isRoomActive = true;
			this.gameObject.SetActive(true);
		}
	}
	
	public void onRoomExited(string room) {
		Debug.Log("InvertedCamera["+this.name+"]/onRoomExited: " + room + ", containingRoom: " + this.containingRoom);
		if(room == this.containingRoom) {
//			this.isRoomActive = false;
			this.gameObject.SetActive(false);
		}
	}
	

}
