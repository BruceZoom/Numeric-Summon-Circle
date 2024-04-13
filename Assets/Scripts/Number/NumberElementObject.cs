using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace NSC.Number
{
    public class NumberElementObject : MonoBehaviour
    {
        private TextMeshPro _numberText;

        private void Awake()
        {
            _numberText = GetComponentInChildren<TextMeshPro>();
        }

        public void SetNumber(NumberElement number)
        {
            _numberText.text = number.ToString();
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
    }
}