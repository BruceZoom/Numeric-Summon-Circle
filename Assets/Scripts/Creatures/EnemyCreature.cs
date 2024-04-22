using System.Collections;
using System.Collections.Generic;
using NSC.Number;
using NSC.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace NSC.Creature
{
    public class EnemyCreature : NumberCreature
    {
        [Header("Enemy Creature")]
        [SerializeField] private float _moveSpeed;
        private float _initSpeed;

        private Vector3 _direction;

        //public UnityEvent DeathCallback;

        protected override void Awake()
        {
            base.Awake();

            _initSpeed = _moveSpeed;
        }

        private void Start()
        {
            _direction = (Vector3.zero - transform.position).normalized;
            _direction.SetZ(0);
        }

        public override void SetNumber(NumberElement number)
        {
            base.SetNumber(number);

            _moveSpeed = _initSpeed / (0.9f + Mathf.Min(Mathf.Abs(number.Value) / 100f, 1.1f));
        }

        protected override void Update()
        {
            base.Update();

            transform.position += _direction * _moveSpeed * Time.deltaTime;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                // damage player
                GameManager.Instance.TakeDamage((float)Number.Numerator / Number.Denominator);

                var vfx = GameObject.Instantiate(_damageVFX, transform.position / 2, Quaternion.identity);
                vfx.AnimScale = 2;

                Die();
            }
        }

        public override void Die(bool definiteDrop = false, bool noDrop = false)
        {
            base.Die(definiteDrop, noDrop);

            EnemyManager.Instance.UntrackEnemy(this);
            //DeathCallback?.Invoke();
            //DeathCallback.RemoveAllListeners();
        }
    }
}