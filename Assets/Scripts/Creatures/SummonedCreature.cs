using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NSC.Number;
using UnityEngine;

namespace NSC.Creature
{
    public class SummonedCreature : NumberCreature
    {
        [Header("Summoned Creature")]
        [SerializeField] private float _updateInterval;

        private Transform _trackTarget;

        [SerializeField] private float _moveSpeed;
        private float _initSpeed;

        private float _nextTargetUpdateTime;

        private Rigidbody2D _rb;

        protected override void Awake()
        {
            base.Awake();

            _rb = GetComponent<Rigidbody2D>();
            _initSpeed = _moveSpeed;
        }

        protected override void Update()
        {
            base.Update();

            _nextTargetUpdateTime -= Time.deltaTime;
            if (_nextTargetUpdateTime <= 0 || _trackTarget == null)
            {
                _nextTargetUpdateTime = _updateInterval;

                if (EnemyManager.Instance.EnemyCreatures.Count <= 0)
                {
                    _trackTarget = transform;
                }
                else
                {
                    _trackTarget = null;
                    float minDist = 0;
                    foreach (var enemy in EnemyManager.Instance.EnemyCreatures)
                    {
                        var newDist = (enemy.transform.position - transform.position).sqrMagnitude;
                        if (_trackTarget == null || newDist < minDist)
                        {
                            _trackTarget = enemy.transform;
                            minDist = newDist;
                        }
                    }

                    //_trackTarget.GetComponent<EnemyCreature>().DeathCallback.AddListener(UntrackEnemy);
                }
            }

            if (_trackTarget != transform && _trackTarget != null)
            {
                _rb.velocity = (_trackTarget.position - transform.position).normalized * _moveSpeed;
                //transform.position += (_trackTarget.position - transform.position).normalized * _moveSpeed * Time.deltaTime;
            }
            else
            {
                _rb.velocity = Vector2.zero;
            }
        }

        public override void SetNumber(NumberElement number)
        {
            base.SetNumber(number);

            _moveSpeed = _initSpeed / (0.9f + Mathf.Min(Mathf.Abs(number.Value) / 100f, 1.1f));
        }

        private void UntrackEnemy()
        {
            _trackTarget = null;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out EnemyCreature enemy))
            {
                MeleeAttack(enemy);
            }
        }

        public override void Die(bool definiteDrop = false, bool noDrop = false)
        {
            base.Die(definiteDrop, noDrop);

            GameManager.Instance.CurrentSummons -= 1;
        }


#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (_trackTarget != null)
            {
                Gizmos.DrawIcon(_trackTarget.position, "Track Target");
            }
        }
#endif
    }
}