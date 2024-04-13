using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NSC.Number;
using TMPro;
using UnityEngine;

namespace NSC.Creature
{
    public class NumberCreature : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _numberText;
        [SerializeField] private float _textAnimScale;
        [SerializeField] private float _textAnimDuration;
        private Tweener _textAnim;

        public NumberElement Number { get; private set; }

        public void SetNumber(NumberElement number)
        {
            Number = number;
            _numberText.text = number.ToString();
            transform.localScale *= (0.8f + Mathf.Abs(number.IntegerPart) / 10);
        }

        public void MeleeAttack(NumberCreature other)
        {
            // netrualized
            if (Number == other.Number)
            {
                // destroy myself
                this.Die();
                // destroy the other
                other.Die();
                // TODO: extra bonus
            }
            // other win
            else if (Number.CompareTo(other.Number) < 0)
            {
                // destroy myself
                this.Die();
                // damage the other
                other.TakeDamage(Number);
            }
            // myself win
            else
            {
                // damage myself
                this.TakeDamage(other.Number);
                // destroy the other
                other.Die();
            }
        }

        public virtual void Die()
        {
            Destroy(gameObject);
            // TODO: basic reward
        }

        public virtual void TakeDamage(NumberElement number)
        {
            Number = NumberElement.Subtract(Number, number);
            _numberText.text = Number.ToString();
            if (_textAnim == null || !_textAnim.IsActive())
            {
                _textAnim = _numberText.transform.DOPunchScale(Vector3.one * _textAnimScale, _textAnimDuration, 1, 0);
            }
        }
    }
}