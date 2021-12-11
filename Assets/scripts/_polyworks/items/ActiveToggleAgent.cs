namespace Polyworks
{

    public class ActiveToggleAgent : Toggler
    {
        public override void Toggle()
        {
            base.Toggle();
            _toggle();
        }

        private void Awake()
        {
            _toggle();
        }

        private void _toggle()
        {
            //			Debug.Log ("----- ActiveToggleAgent[" + this.name + "]/_toggle, isOn = " + isOn);
            this.gameObject.SetActive(isOn);
        }
    }
}
