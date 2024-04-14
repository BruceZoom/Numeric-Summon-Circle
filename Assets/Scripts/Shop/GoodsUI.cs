using System.Collections;
using System.Collections.Generic;
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

        public int Cost => _definition.Cost;

        public void SetGoods(GoodsDefinition definition)
        {
            _definition = definition;


            // set UI
            _nameText.text = _definition.GoodsName;
            _descriptionText.text = _definition.GoodsDescription;
            _costText.text = $"Cost: {Cost}";
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
            // TODO:
            Debug.Log("On purchse");
        }
    }
}