using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TcgEngine.UI
{
    /// <summary>
    /// Pack panel is similar to the collection, but shows all the packs you own and all available packs
    /// </summary>

    public class CoinStorePanel : UIPanel
    {
        [Header("Packs")]
        public ScrollRect scroll_rect;
        public RectTransform scroll_content;
        public CoinStoreGrid grid_content;
        public GameObject entity_prefab;

        private List<GameObject> entity_list = new List<GameObject>();

        private static CoinStorePanel instance;

        protected override void Awake()
        {
            base.Awake();
            instance = this;

            //Delete grid content
            for (int i = 0; i < grid_content.transform.childCount; i++)
                Destroy(grid_content.transform.GetChild(i).gameObject);

        }

        public void RefreshStore()
        {
            UserData udata = Authenticator.Get().UserData;

            foreach (GameObject entity in entity_list)
                Destroy(entity.gameObject);
            entity_list.Clear();

            foreach (CoinStoreData pack in CoinStoreData.GetAllAvailable())
            {
                GameObject nEntity = Instantiate(entity_prefab, grid_content.transform);
                CoinStoreUI entity_ui = nEntity.GetComponentInChildren<CoinStoreUI>();
                entity_ui.SetCoinStoreData(pack);
                entity_ui.onClick += OnClick;
                entity_list.Add(nEntity);
            }
        }
        
        public void OnClick(CoinStoreUI entity)
        {
            CoinStoreZoomPanel.Get().ShowPack(entity.GetCoinStoreData());
        }

        public override void Show(bool instant = false)
        {
            base.Show(instant);
            RefreshStore();
        }

        public static CoinStorePanel Get()
        {
            return instance;
        }
    }
}