namespace Polyworks
{
    using UnityEngine;
    public class UIController : MonoBehaviour, IInputControllable
    {
        public Vector3 ActivePosition = new Vector3(0, 0, 0);
        public Vector3 InactivePosition = new Vector3(500, 0, 0);
        public bool IsActiveOnAwake = false;
        public bool IsLogOn;

        protected EventCenter eventCenter;
        protected float horizontal;
        protected float vertical;
        protected bool up;
        protected bool down;
        protected bool left;
        protected bool right;
        protected bool confirm;
        protected bool cancel;
        protected bool isActive;

        public void SetInput(InputObject input)
        {
            SetHorizontal(input.horizontal);
            SetVertical(input.vertical);
            SetUp(input.buttons[InputController.UP_BUTTON]);
            SetDown(input.buttons[InputController.DOWN_BUTTON]);
            SetLeft(input.buttons[InputController.LEFT_BUTTON]);
            SetRight(input.buttons[InputController.RIGHT_BUTTON]);
            SetConfirm(input.buttons[InputController.CONFIRM_BUTTON]);
            SetCancel(input.buttons[InputController.CANCEL_BUTTON]);
        }

        #region protected methods
        protected virtual void SetActive(bool isActive)
        {
            Vector3 position = (isActive) ? ActivePosition : InactivePosition;
            gameObject.transform.localPosition = new Vector3(position.x, position.y, position.z);

            this.isActive = isActive;

            if (!isActive)
            {
                return;
            }

            eventCenter.SetActiveInputTarget("set_active_input_object", this);
        }


        protected void SetHorizontal(float h)
        {
            horizontal = h;
        }

        protected void SetVertical(float v)
        {
            vertical = v;
        }

        protected void SetUp(bool isPressed)
        {
            up = isPressed;
        }

        protected void SetDown(bool isPressed)
        {
            down = isPressed;
        }

        protected void SetLeft(bool isPressed)
        {
            left = isPressed;
        }

        protected void SetRight(bool isPressed)
        {
            right = isPressed;
        }

        protected void SetConfirm(bool isPressed)
        {
            confirm = isPressed;
        }

        protected void SetCancel(bool isPressed)
        {
            cancel = isPressed;
        }
        #endregion

        #region protected methods
        protected virtual void Init()
        {
            eventCenter = EventCenter.Instance;

            SetActive(IsActiveOnAwake);
        }

        protected void Log(string message)
        {
            if (!IsLogOn)
            {
                return;
            }
            Debug.Log(message);
        }
        #endregion
    }
}

