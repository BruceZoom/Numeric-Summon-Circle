using System.Collections;
using System.Collections.Generic;
using NSC.Number;
using NSC.Utils;
using Unity.VisualScripting;
using UnityEngine;

namespace NSC.Inventory
{
    public class InventoryManager : PassiveSingleton<InventoryManager>
    {
        private Dictionary<NumberElement, InventoryStack> _inventory;

        [SerializeField] private Transform _inventoryUIParent;
        [SerializeField] private NumberElementUI _elementUIPrefab;

        private void Awake()
        {
            Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();

            _inventory = new Dictionary<NumberElement, InventoryStack>();

            var ui = GameObject.Instantiate(_elementUIPrefab, _inventoryUIParent);
            var one = new NumberElement(1, 1);
            _inventory.Add(one, new InventoryStack(-1, one, ui));
        }

        /// <summary>
        /// TODO: check whether a number is in inventory
        /// </summary>
        public bool HasNumber(NumberElement number)
        {
            return _inventory.ContainsKey(number) && (_inventory[number].Count == -1 || _inventory[number].Count > 0);
        }

        /// <summary>
        /// TODO: consume a number by removing one of it from inventory
        /// </summary>
        public void ConsumeNumber(NumberElement number)
        {
            if (_inventory[number].Count != -1)
            {
                _inventory[number].Count -= 1;
            }
        }

        public void AddNumber(NumberElement number)
        {
            if (!_inventory.ContainsKey(number))
            {
                var ui = GameObject.Instantiate(_elementUIPrefab, _inventoryUIParent);
                _inventory.Add(number, new InventoryStack(1, number, ui));
            }
            else if (_inventory[number].Count >= 0)
            {
                _inventory[number].Count += 1;
            }
        }
    }
}