using UnityEngine;

public class EventAnimationParent : ArmatureParent
{

    public AnimationClip animationClip;
    public string eventName = "";

    public bool isSingleUse = false;

    void Awake()
    {
        InitEventAnimationParent();
    }

    public virtual void InitEventAnimationParent()
    {
        if (eventName != "")
        {
            //			EventCenter.Instance.OnTriggerEvent += OnTriggerEvent;
        }
        Init();
    }

    public void OnTriggerEvent(string evt)
    {
        // Debug.Log("EventAnimationParent[ " + name + " ]/OnTriggerEvent, evt = " + evt + ", eventName = " + eventName + ", animationClip = " + animationClip.name);
        if (evt == eventName && animationClip != null)
        {
            PlayAnimation(animationClip.name);
            if (isSingleUse)
            {
                //				EventCenter.Instance.OnTriggerEvent -= OnTriggerEvent;
            }
        }
    }
}
