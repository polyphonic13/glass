using UnityEngine;

namespace Polyworks
{
    public class RaycastAgent : MonoBehaviour
    {
        public float detectionDistance = 2f;
        public string dynamicTag = "interactive";
        public string staticTag = "persistent";
        public bool isActive;
        public bool isLogOn = false;

        public Color rayColor = Color.red;

        public ProximityAgent focusedItem { get; set; }
        public string itemJustHit { get; set; }

        private RaycastHit _hit;

        public virtual void CheckRayCast()
        {
            // _log ("RaycastAgent[" + this.name + "]/CheckRayCast, dynamicTag = " + dynamicTag);
            if (Physics.Raycast(this.transform.position, this.transform.forward, out _hit, detectionDistance))
            {
                Debug.DrawRay(this.transform.position, this.transform.forward, rayColor);
                // _log (" _hit tag = " + _hit.transform.tag + ", name = " + _hit.transform.name);
                if (_hit.transform != this.transform && (_hit.transform.tag == dynamicTag || _hit.transform.tag == staticTag))
                {
                    _log(" _hit name = " + _hit.transform.name + ", just hit = " + itemJustHit);
                    if (_hit.transform.name != itemJustHit)
                    {
                        ProximityAgent pa = _hit.transform.gameObject.GetComponent<ProximityAgent>();
                        _log("  pa = " + pa);
                        if (pa != null)
                        {
                            if (pa.Check())
                            {
                                _log("    pa check returned true");
                                _clearFocus();
                                pa.SetFocus(true);
                                itemJustHit = _hit.transform.name;
                                focusedItem = pa;
                            }
                        }
                    }
                }
                else
                {
                    _clearFocus();
                }
            }
            else
            {
                _clearFocus();
            }
        }

        public void ClearFocus()
        {
            //			_log ("RaycastAgent/ClearFocus, focusedItem = " + focusedItem);
            _clearFocus();
        }

        private void _clearFocus()
        {
            if (focusedItem != null)
            {
                focusedItem.SetFocus(false);
                focusedItem = null;
            }
            itemJustHit = "";
        }

        private void _log(string message)
        {
            if (isLogOn)
            {
                Debug.Log(message);
            }
        }
    }
}

