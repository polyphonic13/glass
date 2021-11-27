using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Polyworks
{
    public class CrosshairUI : MonoBehaviour
    {

        public Image image;
        public string[] icons;
        public int defaultIcon;

        private ArrayList sprites;

        public void OnContextChange(InputContext context, string param)
        {
            if (context == InputContext.PLAYER)
            {
                this.gameObject.SetActive(true);
                return;
            }
            this.gameObject.SetActive(false);
        }

        public void OnInspectItem(bool isInspecting, string itemName)
        {
            if (isInspecting)
            {
                this.gameObject.SetActive(false);
                return;
            }
            this.gameObject.SetActive(true);
        }

        public void OnItemDisabled()
        {
            image.sprite = sprites[defaultIcon] as Sprite;
        }

        public void OnNearItem(Item item, bool isFocused)
        {
            // Debug.Log ("CrosshairUI/OnNearItem, isFocused = " + isFocused + ", item = " + item.name);
            if (isFocused && item.icon != -1 && item.isEnabled)
            {
                image.sprite = sprites[item.icon] as Sprite;
            }
            else
            {
                image.sprite = sprites[defaultIcon] as Sprite;
            }
        }

        private void Awake()
        {
            sprites = new ArrayList();

            for (int i = 0; i < icons.Length; i++)
            {
                GameObject iconObj = (GameObject)Instantiate(Resources.Load(icons[i], typeof(GameObject)), transform.position, transform.rotation);
                // iconObj.transform.parent = this.transform.parent;
                iconObj.transform.SetParent(this.transform.parent, false);

                Image iconImg = iconObj.GetComponent<Image>();
                sprites.Add(iconImg.sprite);

                if (i == defaultIcon)
                {
                    image.sprite = iconImg.sprite;
                }
            }

            _addHandlers();
        }

        private void _addHandlers()
        {
            EventCenter ec = EventCenter.Instance;
            if (ec == null)
            {
                return;
            }
            ec.OnNearItem += OnNearItem;
            ec.OnContextChange += OnContextChange;
            ec.OnItemDisabled += OnItemDisabled;
            ec.OnInspectItem += OnInspectItem;
        }

        private void _removeHandlers()
        {
            EventCenter ec = EventCenter.Instance;
            if (ec == null)
            {
                return;
            }
            ec.OnNearItem -= OnNearItem;
            ec.OnContextChange -= OnContextChange;
            ec.OnItemDisabled -= OnItemDisabled;
            ec.OnInspectItem -= OnInspectItem;
        }

        private void OnDestroy()
        {
            _removeHandlers();
        }
    }
}

