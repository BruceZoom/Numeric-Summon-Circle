using System.Collections;
using System.Collections.Generic;
using NSC.Shop;
using NSC.UI;
using NSC.Utils;
using TMPro;
using UnityEngine;

namespace NSC
{
    public class GameManager : MonoSingleton<GameManager>
    {
        [SerializeField] private UIManager _uIManager;
        [SerializeField] private ShopManager _shopManager;

        [field: SerializeField] public int SummonCapacity { get; private set; } = 5;
        private int _currentSummons;
        public int CurrentSummons
        {
            get => _currentSummons;
            set
            {
                _currentSummons = value;
                UIManager.Instance.CapacityText.text = $"Capacity: {_currentSummons}/{SummonCapacity}";
            }
        }


        public override void Initialize()
        {
            _uIManager.Initialize();

            _shopManager.Initialize();
            CurrentSummons = 0;
        }
    }
}