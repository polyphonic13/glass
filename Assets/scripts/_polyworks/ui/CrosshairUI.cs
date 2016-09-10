using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Polyworks {
	public class CrosshairUI : MonoBehaviour {

		public Image image;
		public string[] icons;
		public int defaultIcon;

		private ArrayList _sprites;

		public void OnNearItem(Item item, bool isFocused) {
			if (isFocused && item.icon != null) {
				image.sprite = _sprites[item.icon] as Sprite;
			} else {
				image.sprite = _sprites [defaultIcon] as Sprite;
			}
		}

		private void Awake () {
			EventCenter.Instance.OnNearItem += this.OnNearItem;

			_sprites = new ArrayList ();
			for (int i = 0; i < icons.Length; i++) {
				GameObject iconObj = (GameObject)Instantiate (Resources.Load (icons[i], typeof(GameObject)), transform.position, transform.rotation);
				Image iconImg = iconObj.GetComponent<Image>();
				_sprites.Add (iconImg.sprite);

				if (i == defaultIcon) {
					image.sprite = iconImg.sprite;
				}
			}
		}

		private void OnDestroy() {
			EventCenter ec = EventCenter.Instance;
			if (ec != null) {
				ec.OnNearItem -= this.OnNearItem;
			}
		}
	}
}

