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
        private Rigidbody2D _rb;

        [SerializeField] private float _spawnAnimTime = 0.2f;

        [SerializeField] private Sprite _moneySprite;
        [SerializeField] private Sprite _numberSprite;
        [SerializeField] private Sprite _goldSprite;

        [SerializeField] private float _pickupDropForce = 1f;
        [SerializeField] private float _pickupDropTime = 1f;
        [SerializeField] private float _pickupTime = 0.5f;

        public SpriteRenderer SpriteRend { get; private set; }
        [field: SerializeField] public TextMeshPro NumberText { get; private set; }

        private void Awake()
        {
            NumberText = GetComponentInChildren<TextMeshPro>();
            _rb = GetComponent<Rigidbody2D>();
            _rb.bodyType = RigidbodyType2D.Kinematic;

            SpriteRend = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            transform.DOScale(1f, _spawnAnimTime).From(0f);
        }

        public void SetNumber(NumberElement number, bool isGold = false)
        {
            NumberText.text = number.ToString();

            SpriteRend.sprite = number.Numerator != 0 ? _numberSprite : (isGold ? _goldSprite : _moneySprite);
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
            Vector2 force = new Vector2(UnityEngine.Random.Range(-2f, 2), 1) * _pickupDropForce;
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