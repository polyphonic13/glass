using UnityEngine;
using System.Collections;

public class PiggyBank : MonoBehaviour {

	public CollectableItem[] crystalKeys;

	private string[] _dresserAnimations = {
		"bedroom_e_dresser_bottom_drawer_open",
		"bedroom_e_dresser_middle_drawer_open",
		"bedroom_e_dresser_top_drawer_open",
		"bedroom_e_dresser_default"
	};
	private string[] _coins = {
		"astrological_coin_aries", 
		"astrological_coin_leo", 
		"astrological_coin_scorpio"
	};
	private string[] _unlockEvents = {
		"bedroom_e_dresser_drawer_bottom_unlock",
		"bedroom_e_dresser_drawer_middle_unlock",
		"bedroom_e_dresser_drawer_top_unlock"
	};

	public void InsertCoin(string coin, string coinName) {
		Debug.Log("PiggyBank/InsertCoin, coin = " + coin);

		for(int i = 0; i < _coins.Length; i++) {
			if(_coins[i] == coin) {
				Debug.Log ("this is a matching coin: " + i);
				crystalKeys[i].IsEnabled = true;
				EventCenter.Instance.TriggerEvent(_unlockEvents[i]);
			}
		}
		EventCenter.Instance.CloseInventoryUI ();
		EventCenter.Instance.AddNote (coinName + " added to Piggy Bank");
	}

	private void Awake() {
		foreach (CollectableItem crystalKey in crystalKeys) {
			crystalKey.IsEnabled = false;
		}
	}

}
