using System.Collections.Generic;
using UnityEngine;

namespace TcgEngine
{
    /// <summary>
    /// Defines all shop data
    /// </summary>

    [CreateAssetMenu(fileName = "CoinStoreData", menuName = "TcgEngine/CoinStoreData", order = 5)]
    public class CoinStoreData : ScriptableObject
    {
        [Header("Content")]
        public string id;
        public int amount;

        [Header("Display")]
        public string title;
        public Sprite bg_img;
        [TextArea(5, 10)]
        public string desc;
        public int sort_order;

        [Header("Availability")]
        public bool available = true;
        public float cost = 4.99f;  //Cost to buy

        public static List<CoinStoreData> pack_list = new List<CoinStoreData>();

        public static void Load(string folder = "")
        {
            if (pack_list.Count == 0)
                pack_list.AddRange(Resources.LoadAll<CoinStoreData>(folder));

            pack_list.Sort((CoinStoreData a, CoinStoreData b) => {
                if (a.sort_order == b.sort_order)
                    return a.id.CompareTo(b.id);
                else
                    return a.sort_order.CompareTo(b.sort_order);
            });
        }

        public static List<CoinStoreData> GetAllAvailable()
        {
            List<CoinStoreData> valid_list = new List<CoinStoreData>();
            foreach (CoinStoreData apack in GetAll())
            {
                if (apack.available)
                    valid_list.Add(apack);
            }
            return valid_list;
        }

        public static List<CoinStoreData> GetAll()
        {
            return pack_list;
        }
    }
}