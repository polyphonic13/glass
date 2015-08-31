using UnityEngine;
using System.Collections;

public class PiggyBank : MonoBehaviour {

	private string[] coins = {"astrological_coin_aries", "astrological_coin_scorpio", "astrological_coin_leo"};
	
	public void InsertCoin(string coin, string coinName) {
		Debug.Log("PiggyBank/InsertCoin, coin = " + coin);
		foreach(string c in coins) {
			if(c == coin) {
				Debug.Log ("this is a matching coin");
			}
		}
		EventCenter.Instance.CloseInventoryUI ();
		EventCenter.Instance.AddNote (coinName + " added to Piggy Bank");
	}
}
