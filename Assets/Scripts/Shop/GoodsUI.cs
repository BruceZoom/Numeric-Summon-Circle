using System.Collections;
using System.Collections.Generic;
using NSC.Data;
using NSC.Inventory;
using NSC.Number;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NSC.Shop
{
    public class GoodsUI : MonoBehaviour
    {
        private GoodsDefinition _definition;

        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private TextMeshProUGUI _costText;

        public int Cost { get; private set; }

        private NumberElement _number;
        private int _count;

        public void SetGoods(GoodsDefinition definition, int cost)
        {
            _definition = definition;

            // special cost calculation for number element
            if (_definition.Type == ShopOption.NumberElement)
            {
                // sample number
                do
                {
                    _number = NumberElement.RandomNumberElement(_definition.MinNumber, _definition.MaxNumber);
                } while (_number.Value == 1);
                _count = Random.Range(_definition.MinCount, _definition.MaxCount);

                // base cost
                float baseCost = Mathf.Abs(_number.Value) * DataManager.Instance.Data.MagnitudeCostCoef;
                // discount
                float limit = DataManager.Instance.Data.DenoDiscountLimit;
                float discount = limit / (Mathf.Log10(_number.Denominator) + 1f) + 1f - limit;
                // final
                Cost = (int)(baseCost * discount) * _count;

                // set UI
                _nameText.text = _definition.GoodsName + $"\n{_number}  x{_count}";
                _descriptionText.text = _definition.GoodsDescription;
                _costText.text = $"Cost: {Cost}<sprite name=\"money\">";
            }
            else
            {
                Cost = cost;
                // set UI
                _nameText.text = _definition.GoodsName;
                _descriptionText.text = _definition.GoodsDescription;
                _costText.text = $"Cost: {Cost}<sprite name=\"money\">";
            }
        }

        public void OnPointerClick()
        {
            if (ShopManager.Instance.TryPurchase(this))
            {
                OnPurchase();
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Different on purchase logic for different objects.
        /// </summary>
        public void OnPurchase()
        {
            if (_definition.Type == ShopOption.NumberElement)
            {
                InventoryManager.Instance.AddNumber(_number, _count);
            }
            else
            {
                _definition.OnPurchase();
            }

            // increase cost
            ShopManager.Instance.Costs[_definition] += _definition.CostInc;
        }
    }
}