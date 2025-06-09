using UnityEngine;
using UnityEngine.UI;
using Xsolla.Catalog;

namespace TcgEngine.UI
{
    /// <summary>
    /// When you click on a pack in the PackPanel, a box will appear to show more information about it
    /// You can also buy pack in this panel
    /// </summary>

    public class CoinStoreZoomPanel : UIPanel
    {
        public CoinStoreUI store_ui;

        public GameObject buy_area;
        public Text buy_cost;
        public Text buy_error;

        private CoinStoreData data;

        private static CoinStoreZoomPanel instance;

        protected override void Awake()
        {
            base.Awake();
            instance = this;

            TabButton.onClickAny += OnClickTab;
        }

        private void OnDestroy()
        {
            TabButton.onClickAny -= OnClickTab;
        }

        protected override void Update()
        {
            base.Update();

            if (data != null)
            {
                buy_cost.text = $"${data.cost}";
            }
        }

        public void ShowPack(CoinStoreData data)
        {
            this.data = data;

            UserData udata = Authenticator.Get().UserData;
            store_ui.SetEntity(data);
            buy_error.text = "";
            buy_area?.SetActive(data.available);
            Show();
        }

        public void OnClickBuy()
        {
            if (!XSollaPanel.Get().IsLoggedIn)
            {
                XSollaPanel.Get().Show(() =>
                {
                    XSollaPurchase();
                });
            }
            else
                XSollaPurchase();
        }

        void XSollaPurchase()
        {
            XsollaCatalog.Purchase(
                    data.id,
                    onSuccess: (orderStatus) =>
                    {
                        AddCoinsAPI(data.amount);
                    },
                    onError: (error) =>
                    {
                        //Debug.LogError($"Order creation failed: {error.errorMessage}");
                        NotificationPanel.Get().ShowMessage("Coins Purchase", "Transaction Failed");
                    }
                );
        }

        private async void AddCoinsAPI(int amount)
        {
            AddCoinsRequest req = new AddCoinsRequest();
            req.amount = amount;

            string url = ApiClient.ServerURL + "/coinstore/add";
            string json = ApiTool.ToJson(req);
            WebResponse res = await ApiClient.Get().SendPostRequest(url, json);
            await Authenticator.Get().LoadUserData();
            Hide();
            CoinStorePanel.Get().Hide();
            PackPanel.Get().Show();
            NotificationPanel.Get().ShowMessage("Coins Purchase", "Transaction Successful");
        }

        private void OnClickTab(TabButton btn)
        {
            if (btn.group == "menu")
                Hide();
        }

        public CoinStoreData GetPack()
        {
            return data;
        }

        public static CoinStoreZoomPanel Get()
        {
            return instance;
        }
    }
}