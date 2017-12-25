using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Polyworks {
	public class CrosshairUI : MonoBehaviour {

		public Image image;
		public string[] icons;
		public int defaultIcon;

		private ArrayList _sprites;

		public void OnContextChange(InputContext context, string param) {
			if (context == InputContext.PLAYER) {
				this.gameObject.SetActive (true);
			} else {
				this.gameObject.SetActive (false);
			}
		}

		public void OnNearItem(Item item, bool isFocused) {
//			Debug.Log ("CrosshairUI/OnNearItem, isFocused = " + isFocused + ", item = " + item.name);
			if (isFocused && item.icon != null) {
				image.sprite = _sprites[item.icon] as Sprite;
			} else {
				image.sprite = _sprites [defaultIcon] as Sprite;
			}
		}

		private void Awake () {

			_sprites = new ArrayList ();
			for (int i = 0; i < icons.Length; i++) {
				GameObject iconObj = (GameObject)Instantiate (Resources.Load (icons[i], typeof(GameObject)), transform.position, transform.rotation);
				iconObj.transform.parent = this.transform.parent;
				Image iconImg = iconObj.GetComponent<Image>();
				_sprites.Add (iconImg.sprite);

				if (i == defaultIcon) {
					image.sprite = iconImg.sprite;
				}
			}

			_addHandlers ();
		}

		private void _addHandlers() {
			EventCenter ec = EventCenter.Instance;
			if (ec != null) {
				ec.OnNearItem += this.OnNearItem;
				ec.OnContextChange += this.OnContextChange;
			}
		}

		private void _removeHandlers() {
			EventCenter ec = EventCenter.Instance;
			if (ec != null) {
				ec.OnNearItem -= this.OnNearItem;
				ec.OnContextChange -= this.OnContextChange;
			}
		}

		private void OnDestroy() {
			_removeHandlers ();
		}
	}
}

