namespace Polyworks
{
    using UnityEngine;

    public class NotificationUIController : Singleton<NotificationUIController>
    {

        public NotificationUI[] notificationUIs;

        public float fadeTime = 2f;
        public float fadeSpeed = 0.03f;

        public void OnAddNote(string message)
        {
            for (int i = 0; i < notificationUIs.Length; i++)
            {
                if (!notificationUIs[i].IsDisplayingNote)
                {
                    notificationUIs[i].AddNote(message);
                    break;
                }
            }
        }

        public void Init()
        {
            EventCenter ec = EventCenter.Instance;
            ec.OnAddNote += OnAddNote;

            foreach (NotificationUI ui in notificationUIs)
            {
                ui.Init(fadeTime, fadeSpeed);
            }
        }

        private void OnDestroy()
        {
            EventCenter ec = EventCenter.Instance;

            if (ec != null)
            {
                ec.OnAddNote += OnAddNote;
            }
        }
    }
}
