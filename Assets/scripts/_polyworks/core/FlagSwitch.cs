namespace Polyworks
{
    using UnityEngine;

    public class FlagSwitch : Switch
    {

        public string key;
        public bool isActivate = true;

        public override void Actuate()
        {
            Log("FlagSwitch[" + this.name + "]/Actuate, key = " + key);

            Game.Instance.SetFlag(key, isActivate);

            isActivate = !isActivate;
        }

        public override void Use()
        {
            Actuate();
        }
    }
}
