namespace Polyworks
{

    public class BoolAnimationParamSwitch : AnimationParamSwitch
    {
        public bool IsParamOn;
        public bool IsSingleUse;

        private bool isUsed;

        public override void Actuate()
        {
            if (IsSingleUse && isUsed)
            {
                return;
            }

            IsParamOn = !IsParamOn;
            Log("BoolAnimationParamSwitch[ " + this.name + " ]/Actuate, type = " + type + ", IsParamOn now = " + IsParamOn + ", target = " + target);
            target.SetBool(type, IsParamOn);

            if (isUsed)
            {
                return;
            }
            isUsed = true;
        }
    }
}
