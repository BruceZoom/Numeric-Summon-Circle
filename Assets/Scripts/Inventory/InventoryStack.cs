using System.Collections;
using System.Collections.Generic;
using NSC.Number;
using UnityEngine;

namespace NSC.Inventory
{
    public class InventoryStack
    {
        private int _count;
        
        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                if (_count == 0)
                {
                    NumberUI.gameObject.SetActive(false);
                }
                else
                {
                    if (!NumberUI.gameObject.activeSelf)
                    {
                        NumberUI.gameObject.SetActive(true);
                    }
                    NumberUI.SetCount(_count);
                }
            }
        }

        public NumberElement Number;
        
        public NumberElementUI NumberUI;

        public InventoryStack(int count, NumberElement number, NumberElementUI numberUI)
        {
            _count = count;
            Number = number;
            NumberUI = numberUI;
            NumberUI.SetCount(_count);
        }
    }
}