using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TcgEngine.UI
{
    public class XSollaPanel : UIPanel
    {
        private static XSollaPanel instance;

        [SerializeField] InputField input_email;
        [SerializeField] InputField input_code;
        [SerializeField] Button verify_btn;
        [SerializeField] GameObject login_panel;

        UnityAction OnPurchaseSuccess;
        bool isLoggedIn = false;

        protected override void Awake()
        {
            base.Awake();
            instance = this;
        }

        private void OnEnable()
        {
            verify_btn.interactable = false;
        }

        public void SendCode()
        {
            if(input_email.text == "")
            {
                Debug.Log("Email cannot be empty");
                return;
            }

            ApiClient.Get().XSollaSendCode(input_email.text);
            verify_btn.interactable = true;
        }

        public void Verify()
        {
            if (input_code.text == "")
            {
                Debug.Log("Code cannot be empty");
                return;
            }

            ApiClient.Get().XSollaVerifyCode(input_email.text, input_code.text, OnVerificationComplete);
        }

        void OnVerificationComplete()
        {
            isLoggedIn = true;
            input_email.text = "";
            input_code.text = "";
            Hide();
            OnPurchaseSuccess?.Invoke();
        }

        public void Show(UnityAction onSuccess)
        {
            base.Show();
            OnPurchaseSuccess = onSuccess;
        }

        public bool IsLoggedIn => isLoggedIn;

        public static XSollaPanel Get()
        {
            return instance;
        }
    }
}
