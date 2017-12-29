using UnityEngine;
using System.Collections;
using Polyworks;

public class PiggyBank : Item {
	
	public CollectableItem[] crystalKeys;

	private const string UNLOCK_MESSAGE = "There was a click within the dresser";
	private const string UNLOCK_EVENT_TYPE = "bedroom_e_dresser_drawer_unlock";

	private string[] _coins = {
		"astrology_coin_aries(Clone)", 
		"astrology_coin_leo(Clone)", 
		"astrology_coin_scorpio(Clone)"
	};
	private string[] _unlockEvents = {
		"bedroom_e_dresser_drawer_bottom_unlock",
		"bedroom_e_dresser_drawer_middle_unlock",
		"bedroom_e_dresser_drawer_top_unlock"
	};

	public void OnStringEvent(string type, string value) {
		Log ("PiggyBank/OnStringEvent, type = " + type + ", value = " + value);
		if(type == "insert_coin") {
			for (int i = 0; i < _coins.Length; i++) {
				if (_coins [i] == value) {
					Log (" it is a matching coin");
					EventCenter ec = EventCenter.Instance;
					ec.AddNote (UNLOCK_MESSAGE);
					ec.InvokeStringEvent (UNLOCK_EVENT_TYPE, value);
					// open/unlock drawer with key?
				}
			}
		}
	}

	public override void Enable ()
	{
		Log ("PiggyBank/Enable");
		base.Enable ();
		EventCenter.Instance.OnStringEvent += OnStringEvent;
	}

	public override void Disable ()
	{
		base.Disable ();
		EventCenter.Instance.OnStringEvent -= OnStringEvent;
	}

	public void InsertCoin(string coin) {
		for(int i = 0; i < _coins.Length; i++) {
			if(_coins[i] == coin) {
				crystalKeys[i].isEnabled = true;
//				EventCenter.Instance.TriggerEvent(_unlockEvents[i]);
				break;
			}
		}
//		EventCenter.Instance.CloseInventoryUI ();
//		EventCenter.Instance.AddNote (coinName + " added to Piggy Bank");
	}

	private void Awake() {
//		foreach (CollectableItem crystalKey in crystalKeys) {
//			crystalKey.isEnabled = false;
//		}
	}

	private void OnDestroy() {
		EventCenter ec = EventCenter.Instance;
		if (ec != null) {
			ec.OnStringEvent -= OnStringEvent;
		}
	}
}
