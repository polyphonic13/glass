namespace Polyworks
{

    public class FlagToggler : Toggler
    {
        public string key;

        public override void Toggle()
        {
            Log("FlagToggler[" + this.name + "]/Toggle, key = " + key + ", isOn = " + isOn);
            base.Toggle();
            Log("  about to call SetFlag with " + isOn);
            Game.Instance.SetFlag(key, isOn);
        }
    }
}
