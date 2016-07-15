using UnityEngine;

public class InvertedCamera : MonoBehaviour {
	public int forward = 1;
	public int up = 1;
	public int right = 1;

	public string containingRoom = "";

	void Start() {
		gameObject.SetActive(false);

		var eventCenter = EventCenter.Instance;
		eventCenter.OnRoomEntered += OnRoomEntered;
		eventCenter.OnRoomExited += OnRoomExited;
	}

	// EXAMPLE WITH CAMERA UPSIDEDOWN
	void OnPreCull() {
//		// Debug.Log("InvertedCamera/OnPreCull");
		GetComponent<Camera>().ResetWorldToCameraMatrix();
		GetComponent<Camera>().ResetProjectionMatrix();
		GetComponent<Camera>().projectionMatrix = GetComponent<Camera>().projectionMatrix * Matrix4x4.Scale(new Vector3(forward, up, right));
	}
	
	void OnPreRender() {
		GL.SetRevertBackfacing(true);
	}
	
	void OnPostRender() {
		GL.SetRevertBackfacing(false);
	}
	

	public virtual void OnRoomEntered(string room) {
		// Debug.Log("InvertedCamera["+name+"]/OnRoomEntered: " + room + ", containingRoom: " + containingRoom);
		if(room == containingRoom) {
//			IsRoomActive = true;
			gameObject.SetActive(true);
		}
	}
	
	public void OnRoomExited(string room) {
		// Debug.Log("InvertedCamera["+name+"]/OnRoomExited: " + room + ", containingRoom: " + containingRoom);
		if(room == containingRoom) {
//			IsRoomActive = false;
			gameObject.SetActive(false);
		}
	}
	

}
