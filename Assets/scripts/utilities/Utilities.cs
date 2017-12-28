using UnityEngine;
using System.Collections;

public class Utilities : MonoBehaviour {

	private static Utilities _instance;
	private Utilities() {}
	
	public static Utilities Instance {
		get {
			if(_instance == null) {
				_instance = GameObject.FindObjectOfType(typeof(Utilities)) as Utilities;      
			}
			return _instance;
		}
	}
	
	// http://answers.unity3d.com/questions/168084/change-layer-of-child.html
	public void ChangeLayers(GameObject go, string name)
	{
		ChangeLayers(go, LayerMask.NameToLayer(name));
	}
	
	public void ChangeLayers(GameObject go, int layer)
	{
		go.layer = layer;
		foreach (Transform child in go.transform)
		{
			ChangeLayers(child.gameObject, layer);
		}
	}
}
