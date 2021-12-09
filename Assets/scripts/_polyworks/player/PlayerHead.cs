namespace Polyworks
{
    public class PlayerHead : RaycastAgent
    {
        public void OnContextChange(InputContext context, string param)
        {
            this.isActive = (context == InputContext.PLAYER);
        }

        private void Awake()
        {
            this.detectionDistance = 4f;
            this.dynamicTag = "interactive";
            this.staticTag = "persistent";

            EventCenter ec = EventCenter.Instance;
            if (ec == null)
            {
                return;
            }
            ec.OnContextChange += this.OnContextChange;
        }

        private void OnDestroy()
        {
            EventCenter ec = EventCenter.Instance;
            if (ec == null)
            {
                return;
            }
            ec.OnContextChange -= this.OnContextChange;
        }

        private void Update()
        {
            if (!this.isActive)
            {
                return;
            }
            CheckRayCast();
        }
    }
}
