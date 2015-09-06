using UnityEngine;
using System.Collections;

public class PiggyBank : MonoBehaviour {

	public CollectableItem[] keys;
	public ArmatureParent dresser;

	private string[] _dresserAnimations = {
		"bedroom_e_dresser_bottom_drawer_open",
		"bedroom_e_dresser_middle_drawer_open",
		"bedroom_e_dresser_top_drawer_open",
		"bedroom_e_dresser_default"
	};
	private string[] _coins = {
		"astrological_coin_aries", 
		"astrological_coin_leo", 
		"astrological_coin_scropio"
	};

	public void InsertCoin(string coin, string coinName) {
		Debug.Log("PiggyBank/InsertCoin, coin = " + coin);
//		foreach(string c in coins) {
		dresser.PlayAnimation (_dresserAnimations [3]);
		for(int i = 0; i < _coins.Length; i++) {
			if(_coins[i] == coin) {
				Debug.Log ("this is a matching coin: " + i);
				keys[i].IsEnabled = true;
				dresser.PlayAnimation(_dresserAnimations[i]);
			}
		}
		EventCenter.Instance.CloseInventoryUI ();
		EventCenter.Instance.AddNote (coinName + " added to Piggy Bank");
	}

	private void Awake() {
		foreach (CollectableItem key in keys) {
			key.IsEnabled = false;
		}
	}
}
