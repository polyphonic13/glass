namespace Polyworks
{
    public class StringEventSwitch : EventSwitch
    {
        public override void Actuate()
        {
            if (type == null || type == "")
            {
                return;
            }
            Log("StringEventSwitch[" + this.name + "]/Actuate, type = " + type + ", value = " + this.name);
            EventCenter.Instance.InvokeStringEvent(type, this.name);
        }
    }
}

