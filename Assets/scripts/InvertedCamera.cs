using UnityEngine;
using System.Collections;

public class InvertedCamera : MonoBehaviour {
	public int forward = 1;
	public int up = 1;
	public int right = 1;

	public string containingRoom = "";

	void Start() {
		gameObject.SetActive (false);

		var eventCenter = EventCenter.Instance;
		eventCenter.onRoomEntered += onRoomEntered;
		eventCenter.onRoomExited += onRoomExited;
	}

	// EXAMPLE WITH CAMERA UPSIDEDOWN
	void OnPreCull() {
//		Debug.Log ("InvertedCamera/OnPreCull");
		camera.ResetWorldToCameraMatrix();
		camera.ResetProjectionMatrix();
		camera.projectionMatrix = camera.projectionMatrix * Matrix4x4.Scale(new Vector3(forward, up, right));
	}
	
	void OnPreRender() {
		GL.SetRevertBackfacing(true);
	}
	
	void OnPostRender() {
		GL.SetRevertBackfacing(false);
	}
	

	public virtual void onRoomEntered(string room) {
		Debug.Log("InvertedCamera["+name+"]/onRoomEntered: " + room + ", containingRoom: " + containingRoom);
		if(room == containingRoom) {
//			isRoomActive = true;
			gameObject.SetActive(true);
		}
	}
	
	public void onRoomExited(string room) {
		Debug.Log("InvertedCamera["+name+"]/onRoomExited: " + room + ", containingRoom: " + containingRoom);
		if(room == containingRoom) {
//			isRoomActive = false;
			gameObject.SetActive(false);
		}
	}
	

}
