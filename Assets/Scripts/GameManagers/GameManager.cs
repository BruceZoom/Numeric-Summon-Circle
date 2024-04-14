using System;
using System.Collections;
using System.Collections.Generic;
using NSC.Player;
using NSC.Shop;
using NSC.UI;
using NSC.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NSC
{
    public class GameManager : MonoSingleton<GameManager>
    {
        [SerializeField] private UIManager _uIManager;
        [SerializeField] private ShopManager _shopManager;

        [SerializeField] private List<Pot> _pots;

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

        [SerializeField] private float _initHP = 100;

        private float _maxHP;
        private float _curHP;

        public float MaxHP
        {
            get => _maxHP;
            private set
            {
                _maxHP = value;
                UIManager.Instance.HPBar.SetHP(_curHP, _maxHP);
            }
        }
        public float CurHP
        {
            get => _curHP;
            private set
            {
                _curHP = value;
                if (_curHP > _maxHP)
                {
                    _curHP = _maxHP;
                }
                else if (_curHP <= 0)
                {
                    _curHP = 0;
                    GameOver();
                }
                UIManager.Instance.HPBar.SetHP(_curHP, _maxHP);
            }
        }

        private void GameOver()
        {
            UIManager.Instance.GameOverPanel.SetActive(true);
            Time.timeScale = 0;
        }

        public override void Initialize()
        {
            _uIManager.Initialize();

            _shopManager.Initialize();
            CurrentSummons = 0;

            MaxHP = _initHP;
            CurHP = MaxHP;

            Time.timeScale = 1;
        }

        public void TakeDamage(float amount)
        {
            CurHP -= amount;
        }

        public void Retry()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}