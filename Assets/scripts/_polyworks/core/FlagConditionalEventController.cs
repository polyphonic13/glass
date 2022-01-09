namespace Polyworks
{
    public class FlagConditionalEventController : Item
    {
        public string flag;
        public string falseMessage = "";
        public EventSwitch[] switches;

        public override void Actuate()
        {
            bool isFlagOn = Game.Instance.GetFlag(flag);
            Log("FlagConditionalEventController[" + this.name + "]/Actuate, flag = " + flag + ", value = " + isFlagOn);

            if (isFlagOn)
            {

                foreach (EventSwitch s in switches)
                {
                    Log(" calling Actuate on [ " + s.name + " ]");
                    s.Actuate();
                }
                return;
            }

            if (falseMessage == "")
            {
                return;
            }
            EventCenter.Instance.AddNote(falseMessage);
        }
    }
}
