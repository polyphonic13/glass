public class CollectedAnimationParent : EventAnimationParent
{

    void Awake()
    {
        InitEventAnimationParent();
    }

    public override void InitEventAnimationParent()
    {
        // Debug.Log("CollectedAnimationParent[ " + name + " ]/InitEventAnimationParent");
        if (eventName != "")
        {
            //			EventCenter.Instance.OnTriggerCollectedEvent += OnTriggerEvent;
        }
        Init();
    }

}

