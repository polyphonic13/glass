namespace Polyworks
{
    public class EventSwitch : Switch
    {
        public string type;

        public override void Actuate()
        {
            base.Actuate();
            Log("EventSwitch[" + this.name + "]/Actuate, type = " + type);
        }

        public override void Use()
        {
            Actuate();
        }
    }
}
