using System;
using System.Collections;
using System.Collections.Generic;
using NSC.Audio;
using NSC.Data;
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
        [SerializeField] private DataManager _dataManager;
        [SerializeField] private UIManager _uIManager;
        [SerializeField] private ShopManager _shopManager;

        [SerializeField] private List<Pot> _pots;
        private int _nextPotIdx = 0;

        public bool HasPot => _nextPotIdx > 0;

        public int SummonCapacity
        {
            get => _summonCapacity;
            set
            {
                _summonCapacity = value;
                UIManager.Instance.CapacityText.text = $"Capacity: {_currentSummons}/{SummonCapacity}";
            }
        }
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

        private float _maxHP;
        private float _curHP;
        private int _summonCapacity;

        public float MaxHP
        {
            get => _maxHP;
            set
            {
                _maxHP = value;
                UIManager.Instance.HPBar.SetHP(_curHP, _maxHP);
            }
        }
        public float CurHP
        {
            get => _curHP;
            set
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

        public bool CanAddPot => _nextPotIdx < _pots.Count;

        private void GameOver()
        {
            UIManager.Instance.GameOverPanel.SetActive(true);
            AudioManager.Instance.PlayRandomSFX(AudioManager.Instance.GameOverSFX);
            Time.timeScale = 0;
        }

        public override void Initialize()
        {
            _dataManager.Initialize();

            _uIManager.Initialize();

            _shopManager.Initialize();

            SummonCapacity = DataManager.Instance.Data.CapacityBase;
            CurrentSummons = 0;

            MaxHP = DataManager.Instance.Data.HPBase;
            CurHP = MaxHP;

            foreach (var pot in _pots)
            {
                pot.gameObject.SetActive(false);
            }

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

        internal void AddPot()
        {
            _pots[_nextPotIdx].gameObject.SetActive(true);
            _nextPotIdx++;
        }
    }
}