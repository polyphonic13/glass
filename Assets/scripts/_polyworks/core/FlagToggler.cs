namespace Polyworks
{
    public class FlagToggler : Toggler
    {
        public string key;

        public override void Toggle()
        {
            Log("FlagToggler[" + this.name + "]/Toggle, key = " + key + ", isOn = " + isOn);
            base.Toggle();
            Game.Instance.SetFlag(key, isOn);
        }
    }
}
