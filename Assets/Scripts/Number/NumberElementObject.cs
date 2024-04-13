using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NSC.UI;
using NSC.Utils;
using TMPro;
using UnityEngine;

namespace NSC.Number
{
    public class NumberElementObject : MonoBehaviour
    {
        private TextMeshPro _numberText;
        private Rigidbody2D _rb;

        [SerializeField] private Sprite _moneySprite;
        [SerializeField] private Sprite _numberSprite;
        [SerializeField] private Sprite _goldSprite;

        [SerializeField] private float _pickupDropForce = 1f;
        [SerializeField] private float _pickupDropTime = 1f;
        [SerializeField] private float _pickupTime = 0.5f;

        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _numberText = GetComponentInChildren<TextMeshPro>();
            _rb = GetComponent<Rigidbody2D>();
            _rb.bodyType = RigidbodyType2D.Kinematic;

            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetNumber(NumberElement number, bool isGold = false)
        {
            _numberText.text = number.ToString();

            _spriteRenderer.sprite = number.Numerator != 0 ? _numberSprite : (isGold ? _goldSprite : _moneySprite);
        }

        public Tweener StartMoveAnimation(Vector3 from, Vector3 to, float duration)
        {
            transform.position = from;
            return transform.DOMove(to, duration)
                .SetEase(Ease.Linear)
                .OnComplete(delegate
                {
                    Destroy(gameObject);
                });
        }

        public void StartPickupAnimation(Action finishCallback)
        {
            _rb.bodyType = RigidbodyType2D.Dynamic;
            Vector2 force = new Vector2(UnityEngine.Random.Range(-1, 1), 1) * _pickupDropForce;
            _rb.AddForce(force, ForceMode2D.Impulse);
            StartCoroutine(PickupAnimation(finishCallback));
        }

        IEnumerator PickupAnimation(Action finishCallback)
        {
            yield return new WaitForSeconds(_pickupDropTime);
            _rb.bodyType = RigidbodyType2D.Kinematic;
            var pos = Camera.main.ScreenToWorldPoint(UIManager.Instance.InventoryUI.position).SetZ(0);
            transform.DOMove(pos, _pickupTime)
                .OnComplete(delegate
                {
                    finishCallback?.Invoke();
                    Destroy(gameObject);
                });
        }
    }
}