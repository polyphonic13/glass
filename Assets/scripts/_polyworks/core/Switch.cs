namespace Polyworks
{
    using UnityEngine;
    using System.Collections;

    public class Switch : ActuateAgent
    {
        public override void Actuate()
        {
            Log("Switch[" + this.name + "]/Actuate");
        }

        public override void Use()
        {
            Log("Switch[" + this.name + "]/Use");
        }
    }
}

