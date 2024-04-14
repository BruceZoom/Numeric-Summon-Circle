using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NSC.Data;
using NSC.Inventory;
using NSC.Number;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NSC.Player
{
    public class Pot : MonoBehaviour, IDropHandler
    {
        [SerializeField] private NumberElementObject _numberElementPrefab;
        private TextMeshPro _numberText;

        [SerializeField] private float _animScale;
        [SerializeField] private float _animDuration;
        [SerializeField] private Transform _generatePoint;

        public float GenerateDuration => DataManager.Instance.PotGenerationTime;
        private float _generationTimer;

        private NumberElement _number;
        private Tweener _anim;

        public NumberElement Number
        {
            get => _number;
            private set
            {
                _number = value;
                _numberText.text = (_number.Numerator == 0 ? "" : _number.ToString());
            }
        }

        private void Awake()
        {
            _numberText = GetComponentInChildren<TextMeshPro>();
            Number = new NumberElement(0);
        }

        private void Update()
        {
            GenerationUpdate();
        }

        private void GenerationUpdate()
        {
            if (Number.Numerator == 0) return;

            _generationTimer -= Time.deltaTime;
            if (_generationTimer <= 0)
            {
                _generationTimer = GenerateDuration;

                GenerateElement();
            }
        }

        private void GenerateElement()
        {
            if (_anim == null || !_anim.IsActive())
            {
                _anim = transform.DOPunchScale(new Vector3(-_animScale, _animScale, 0), _animDuration, 1, 0.1f);
            }

            var reward = GameObject.Instantiate(_numberElementPrefab, _generatePoint.position, Quaternion.identity);
            reward.SetNumber(Number);
            var number = Number;
            reward.StartPickupAnimation(delegate
            {
                InventoryManager.Instance.AddNumber(number);
            });
        }

        public void OnDrop(PointerEventData eventData)
        {
            var ui = eventData.selectedObject.GetComponent<NumberElementUI>();
            if (ui == null) return;

            SetNumber(ui.Number);
        }

        public void SetNumber(NumberElement number)
        {
            if (number?.Denominator == 0)
            {
                number = null;
            }

            Number = number;
            _generationTimer = GenerateDuration;
            //_node.SetIngredient(number);

            if (_anim == null || !_anim.IsActive())
            {
                _anim = transform.DOPunchScale(Vector3.one * _animScale, _animDuration, 1, 0.1f);
            }
        }
    }
}