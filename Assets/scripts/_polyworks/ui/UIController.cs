namespace Polyworks
{
    using UnityEngine;
    public class UIController : MonoBehaviour, IInputControllable
    {
        public bool isActiveOnAwake = false;

        protected float horizontal { get; set; }
        protected float vertical { get; set; }

        protected bool up { get; set; }
        protected bool down { get; set; }
        protected bool left { get; set; }
        protected bool right { get; set; }

        protected bool confirm { get; set; }
        protected bool cancel { get; set; }
        protected bool isActive;
        protected bool IsLogOn;

        protected EventCenter eventCenter;
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
            gameObject.SetActive(isActive);
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
            gameObject.SetActive(isActiveOnAwake);

            eventCenter = EventCenter.Instance;
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

