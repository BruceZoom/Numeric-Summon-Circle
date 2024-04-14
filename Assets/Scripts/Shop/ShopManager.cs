using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NSC.Data;
using NSC.UI;
using NSC.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NSC.Shop
{
    public class ShopManager : PassiveSingleton<ShopManager>
    {
        [SerializeField] private int _money;
        public int Money
        {
            get => _money;
            set
            {
                _money = value;
                // update UI
                UIManager.Instance.MoneyText.text = $"Money <sprite name=\"money\">: {_money}";
            }
        }

        public Dictionary<GoodsDefinition, int> Costs { get => _costs; private set => _costs = value; }
        
        [Header("UI References")]
        [SerializeField] private Button _shopButton;
        [SerializeField] private Button _refreshButton;
        [SerializeField] private Transform _goodsParent;
        [SerializeField] private GoodsUI _goodsUIPrefab;

        private TextMeshProUGUI _shopButtonText;

        [Header("Shop Settings")]
        [SerializeField] private List<GoodsDefinition> _allGoods;
        [SerializeField] private int _refreshCost;

        public int MaxGoods { get; set; }

        private List<GoodsUI> _goods;
        Dictionary<GoodsDefinition, int> _costs;

        public override void Initialize()
        {
            base.Initialize();

            // initial max goods
            MaxGoods = 3;
            // initial cost
            Costs = new Dictionary<GoodsDefinition, int>();
            foreach (var goods in _allGoods)
            {
                Costs.Add(goods, goods.BaseCost);
            }

            _goods = new List<GoodsUI>();

            RefreshGoods();

            _shopButton.onClick.AddListener(ShopButtonClick);
            _shopButtonText = _shopButton.GetComponentInChildren<TextMeshProUGUI>();
            ToggleShop(false);

            Money = 0;
        }

        private void OnEnable()
        {
            _refreshButton.onClick.AddListener(RefreshButtonClick);
            Time.timeScale = 0;
        }

        private void OnDisable()
        {
            _refreshButton.onClick.RemoveListener(RefreshButtonClick);
            Time.timeScale = 1;
        }

        private void OnDestroy()
        {
            _shopButton.onClick.RemoveAllListeners();
        }

        private void RefreshButtonClick()
        {
            if (_refreshCost < Money)
            {
                Money -= _refreshCost;
                RefreshGoods();
            }
        }

        private void ShopButtonClick()
        {
            ToggleShop(!gameObject.activeSelf);
            _shopButtonText.text = gameObject.activeSelf ? "Close Shop" : "Open Shop";
        }

        public void ToggleShop(bool enable)
        {
            gameObject.SetActive(enable);
        }

        public void RefreshGoods()
        {
            // remove existing goods
            foreach(var good in _goods)
            {
                Destroy(good.gameObject);
            }
            _goods.Clear();

            // randomly draw new ones
            var defs = DrawRandomGoods();
            foreach (var def in defs)
            {
                var ui = GameObject.Instantiate(_goodsUIPrefab, _goodsParent);
                ui.SetGoods(def, Costs[def]);
                _goods.Add(ui);
            }
        }

        private List<GoodsDefinition> DrawRandomGoods()
        {
            var goods = new List<GoodsDefinition>();
            var total = _allGoods.Sum(g => g.RandomWeight);
            while (goods.Count < MaxGoods)
            {
                var r = Random.Range(0, total);
                var good = GetGoodsWithWeight(r);
                while (!good.CanSellGoods())
                {
                    r = Random.Range(0, total);
                    good = GetGoodsWithWeight(r);
                }
                goods.Add(good);
            }
            return goods;
        }

        private GoodsDefinition GetGoodsWithWeight(int weight)
        {
            int total = 0;
            int i = 0;
            while (weight >= total && i < _allGoods.Count)
            {
                total += _allGoods[i].RandomWeight;
                i++;
            }
            return _allGoods[i - 1];
        }

        public bool TryPurchase(GoodsUI goods)
        {
            // not enough money
            if (goods.Cost > Money)
            {
                return false;
            }

            // remove goods and subtract money
            _goods.Remove(goods);
            Money -= goods.Cost;
            return true;
        }
    }
}