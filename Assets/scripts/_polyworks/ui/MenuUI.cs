namespace Polyworks
{
    using UnityEngine.UI;

    public class MenuUI : UIController
    {
        public Button[] buttons;

        private int _btnIndex = 0;

        #region public event handlers
        public void OnOpenMenuUI()
        {
            SetActive(true);
        }

        public void OnCloseMenuUI()
        {
            SetActive(false);
        }
        #endregion

        #region protected methods
        protected override void Init()
        {
            base.Init();
            eventCenter.OnOpenMenuUI += OnOpenMenuUI;
            eventCenter.OnCloseMenuUI += OnCloseMenuUI;
        }
        #endregion

        #region unity methods
        private void Awake()
        {
            Init();
        }

        private void FixedUpdate()
        {
            if (!isActive)
            {
                return;
            }

            // Debug.Log("MenuUI/FixedUpdate, canvas.enabled = " + canvas.enabled);
            if (cancel)
            {
                cancel = false;
                eventCenter.CloseMenuUI();
                return;
            }

            if (down)
            {
                if (_btnIndex < buttons.Length - 1)
                {
                    _btnIndex++;
                    return;
                }

                _btnIndex = 0;
                return;
            }

            if (up)
            {
                if (_btnIndex > 0)
                {
                    _btnIndex--;
                    return;
                }
                _btnIndex = buttons.Length - 1;
                return;
            }

            if (!confirm)
            {
                return;
            }
            Log("confirm");
            buttons[_btnIndex].onClick.Invoke();
        }

        private void OnDestroy()
        {
            if (eventCenter == null)
            {
                return;
            }
            eventCenter.OnOpenMenuUI -= OnOpenMenuUI;
            eventCenter.OnCloseMenuUI -= OnCloseMenuUI;
        }
        #endregion
    }
}


