using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Polyworks {
	public class CrosshairUI : MonoBehaviour {

		public Image image;
		public string[] icons;
		public int defaultIcon;

		private ArrayList _sprites;

		void Awake () {
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

		void OnNearItem(Item item, bool isFocused) {
			Debug.Log ("CrosshairUI/OnNearItem, item = " + item.name + ", isFocused = " + isFocused + ", icon = " + item.icon);
			if (isFocused && item.icon != null) {
//				image.sprite = icons[item.icon];
				image.sprite = _sprites[item.icon] as Sprite;
			} else {
				image.sprite = _sprites [defaultIcon] as Sprite;
//				image.sprite = icons[defaultIcon];
			}
		}
	}
}

