using System.Collections;
using System.Collections.Generic;
using NSC.UI;
using NSC.Utils;
using UnityEngine;

namespace NSC.Shop
{
    public class ShopManager : PassiveSingleton<ShopManager>
    {
        private int _money;
        public int Money
        {
            get => _money;
            set
            {
                _money = value;
                // update UI
                UIManager.Instance.MoneyText.text = $"Money: {_money}";
            }
        }

        private void Awake()
        {
            Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();
        }
    }
}