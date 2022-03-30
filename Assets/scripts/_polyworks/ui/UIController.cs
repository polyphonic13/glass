namespace Polyworks
{
    using UnityEngine;
    public class UIController : MonoBehaviour, IInputControllable
    {
        public Vector3 ActivePosition = new Vector3(0, 0, 0);
        public Vector3 InactivePosition = new Vector3(500, 0, 0);
        public bool IsActiveOnAwake = false;
        public bool IsZoomable;
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
        protected bool isZoomIn;
        protected bool isZoomOut;
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

            if (!IsZoomable)
            {
                return;
            }
            SetZoomIn(input.buttons[InputController.ZOOM_IN_BUTTON]);
            SetZoomOut(input.buttons[InputController.ZOOM_OUT_BUTTON]);
        }

        #region protected methods
        protected virtual void Init()
        {
            eventCenter = EventCenter.Instance;

            SetActive(IsActiveOnAwake);
        }

        protected virtual void SetActive(bool isActive)
        {
            Vector3 position = (isActive) ? ActivePosition : InactivePosition;
            gameObject.transform.localPosition = new Vector3(position.x, position.y, position.z);

            this.isActive = isActive;

            if (!isActive)
            {
                return;
            }

            eventCenter.SetActiveInputTarget(InputController.SET_ACTIVE_INPUT_TARGET, this);
        }


        protected virtual void SetHorizontal(float value)
        {
            horizontal = value;
        }

        protected virtual void SetVertical(float value)
        {
            vertical = value;
        }

        protected virtual void SetUp(bool value)
        {
            up = value;
        }

        protected virtual void SetDown(bool value)
        {
            down = value;
        }

        protected virtual void SetLeft(bool value)
        {
            left = value;
        }

        protected virtual void SetRight(bool value)
        {
            right = value;
        }

        protected virtual void SetConfirm(bool value)
        {
            confirm = value;
        }

        protected virtual void SetCancel(bool value)
        {
            cancel = value;
        }

        protected virtual void SetZoomIn(bool value)
        {
            isZoomIn = value;
        }

        protected virtual void SetZoomOut(bool value)
        {
            isZoomOut = value;
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

