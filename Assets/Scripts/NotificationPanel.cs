using UnityEngine;
using UnityEngine.UI;

namespace TcgEngine.UI
{
    public class NotificationPanel : UIPanel
    {
        private static NotificationPanel instance;

        [SerializeField] Text title;
        [SerializeField] Text message;
        [SerializeField] Button btn;

        protected override void Awake()
        {
            base.Awake();
            instance = this;
            btn.onClick.AddListener(() => { Hide(); });
        }

        public void ShowMessage(string title, string message)
        {
            this.title.text = title;
            this.message.text = message;
            Show();
        }

        public override void Hide(bool instant = false)
        {
            base.Hide(instant);
            title.text = "";
            message.text = "";
        }

        public static NotificationPanel Get()
        {
            return instance;
        }
    }
}
