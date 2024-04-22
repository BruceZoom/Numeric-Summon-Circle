using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NSC.Audio;
using NSC.Data;
using NSC.Inventory;
using NSC.Number;
using NSC.Shop;
using NSC.Utils;
using NSC.VFX;
using TMPro;
using UnityEngine;

namespace NSC.Creature
{
    public class NumberCreature : MonoBehaviour
    {
        [SerializeField] protected DamageVFX _damageVFX;

        [Header("Rewards")]
        [SerializeField] private NumberElementObject _numberRewardPrefab;
        public int GoldAmount => DataManager.Instance.GoldValue;

        public float NumberRewardProb => DataManager.Instance.NumberDropRate;

        [Header("Number Text")]
        [SerializeField] private TextMeshPro _numberText;
        [SerializeField] private float _textAnimScale;
        [SerializeField] private float _textAnimDuration;
        private Tweener _textAnim;

        private Vector3 _initScale;

        public NumberElement Number { get; private set; }

        protected virtual void Awake()
        {
            _initScale = transform.localScale;
        }

        public virtual void SetNumber(NumberElement number)
        {
            Number = number;
            _numberText.text = number.ToString();
            transform.localScale = _initScale * Mathf.Min(0.8f + Mathf.Abs(number.Value) / 20, 5f);
        }

        protected virtual void Update()
        {
            // z gets locked by rigidbody
            //var pos = transform.position;
            //pos.SetZ(pos.y);
            //transform.position = pos;
            //Debug.Log(pos);
        }

        public void MeleeAttack(NumberCreature other)
        {
            // netrualized
            if (Number.Equals(other.Number))
            {
                // destroy myself
                this.Die(true);
                // destroy the other
                other.Die(true);
                // extra bonus
                var reward = GameObject.Instantiate(_numberRewardPrefab, transform.position, Quaternion.identity);
                reward.SetNumber(new NumberElement(0), true);
                reward.StartPickupAnimation(delegate
                {
                    ShopManager.Instance.Money += GoldAmount;
                });
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

        public void ForceKill()
        {
            Die(noDrop : true);
        }

        public virtual void Die(bool definiteDrop = false, bool noDrop = false)
        {
            Destroy(gameObject);

            // vfx
            GameObject.Instantiate(_damageVFX, transform.position, Quaternion.identity);
            // sfx
            AudioManager.Instance.PlayRandomSFX(AudioManager.Instance.DamageSFX);

            if (noDrop)
            {
                return;
            }

            // basic money reward
            var reward = GameObject.Instantiate(_numberRewardPrefab, transform.position, Quaternion.identity);
            reward.SetNumber(new NumberElement(0));
            reward.StartPickupAnimation(delegate
            {
                ShopManager.Instance.Money += 1;
            });
            // basic number reward
            if (definiteDrop || Random.Range(0, 1) < NumberRewardProb)
            {
                reward = GameObject.Instantiate(_numberRewardPrefab, transform.position, Quaternion.identity);
                reward.SetNumber(Number);
                var number = Number;
                reward.StartPickupAnimation(delegate
                {
                    InventoryManager.Instance.AddNumber(number);
                });
            }
        }

        public virtual void TakeDamage(NumberElement number)
        {
            SetNumber(NumberElement.Subtract(Number, number));
            if (_textAnim == null || !_textAnim.IsActive())
            {
                _textAnim = _numberText.transform.DOPunchScale(Vector3.one * _textAnimScale, _textAnimDuration, 1, 0);
            }
            GameObject.Instantiate(_damageVFX, transform.position, Quaternion.identity);
            AudioManager.Instance.PlayRandomSFX(AudioManager.Instance.DamageSFX);
        }
    }
}