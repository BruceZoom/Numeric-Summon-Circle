using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace NSC.Creature
{
    public class EnemyCreature : NumberCreature
    {
        [Header("Enemy Creature")]
        [SerializeField] private float _moveSpeed;

        private Vector3 _direction;

        //public UnityEvent DeathCallback;

        private void Start()
        {
            _direction = (Vector3.zero - transform.position).normalized;
        }

        private void Update()
        {
            transform.position += _direction * _moveSpeed * Time.deltaTime;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                // TODO: damage player
                Debug.Log("Damage player");

                Die();
            }
        }

        public override void Die(bool definiteDrop = false)
        {
            base.Die(definiteDrop);

            EnemyManager.Instance.UntrackEnemy(this);
            //DeathCallback?.Invoke();
            //DeathCallback.RemoveAllListeners();
        }
    }
}