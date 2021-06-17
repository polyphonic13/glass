using Polyworks;

public class LockableArmatureTrigger : OpenCloseArmatureTrigger
{

    public string _keyName = "";

    public bool IsLocked = true;

    public void InitLockableArmatureTrigger()
    {
        InitOpenCloseArmatureTrigger();
    }

    public override void HandleAnimation()
    {
        HandleLockCheck();
    }

    public void HandleLockCheck()
    {
        //		// Debug.Log("LockableArmatureTrigger[ " + name + " ]/HandleLockCheck, IsLocked = " + IsLocked);
        if (!IsLocked)
        {
            HandleOpenClose();
        }
        else
        {
            EventCenter.Instance.AddNote("The " + this.name + " is locked");
        }
    }

    public void Unlock()
    {
        _unlock();
    }

    private void _unlock()
    {
        IsLocked = false;
        EventCenter.Instance.AddNote("The " + this.name + " was unlocked");
        HandleOpenClose();
    }

    private void Awake()
    {
        InitLockableArmatureTrigger();
    }
}
