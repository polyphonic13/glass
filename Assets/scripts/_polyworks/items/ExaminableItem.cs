namespace Polyworks
{
    public class ExaminableItem : Item
    {
        public bool isSingleUse = true;

        private bool _isUsedOnce = false;

        public override void Enable()
        {
            base.Enable();
        }

        public override void Actuate()
        {
            if (!isEnabled)
            {
                return;
            }

            if (isSingleUse && _isUsedOnce)
            {
                return;
            }

            EventCenter eventCenter = EventCenter.Instance;

            _isUsedOnce = true;
            eventCenter.AddNote(description);

            if (!isSingleUse)
            {
                return;
            }

            Destroy(this.gameObject);
            eventCenter.NearItem(this, false);
        }
    }
}
