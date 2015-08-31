using UnityEngine;
using System.Collections;

public class PiggyBank : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void InsertCoin(string coin) {
		Debug.Log("PiggyBank/InsertCoin, coin = " + coin);
	}
}
