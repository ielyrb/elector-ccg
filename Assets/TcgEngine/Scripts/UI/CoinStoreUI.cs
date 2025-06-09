using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TcgEngine;
using UnityEngine.EventSystems;

namespace TcgEngine.UI
{
    /// <summary>
    /// Display a pack and all its information
    /// </summary>

    public class CoinStoreUI: MonoBehaviour, IPointerClickHandler
    {
        public Image entity_img;
        public Text entity_title;

        public UnityAction<CoinStoreUI> onClick;
        public UnityAction<CoinStoreUI> onClickRight;

        private CoinStoreData coin_store;

        void Awake()
        {

        }

        public void SetEntity(CoinStoreData coin_store)
        {
            this.coin_store = coin_store;

            if (coin_store != null)
            {
                if (entity_title != null)
                {
                    entity_title.enabled = true;
                    entity_title.text = coin_store.title;
                }
                entity_img.enabled = true;
                entity_img.sprite = coin_store.bg_img;
            }
        }

        public void SetCoinStoreData(CoinStoreData pack)
        {
            SetEntity(pack);
        }

        public void Hide()
        {
            this.coin_store = null;
            entity_img.enabled = false;
            if(entity_title != null)
                entity_title.enabled = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (onClick != null)
                    onClick.Invoke(this);
            }

            if (eventData.button == PointerEventData.InputButton.Right)
            {
                if (onClickRight != null)
                    onClickRight.Invoke(this);
            }
        }

        public CoinStoreData GetCoinStoreData()
        {
            return coin_store;
        }
    }
}