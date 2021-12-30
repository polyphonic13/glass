using Polyworks;

public class RabbitHuntToy : CollectableItem
{
    public override void Use()
    {
        EventCenter.Instance.InvokeStringEvent(ToyChest.RABBIT_HUNT_ADD_EVENT, this.name);
    }
}
