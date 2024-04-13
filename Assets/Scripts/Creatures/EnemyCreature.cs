using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSC.Creature
{
    public class EnemyCreature : NumberCreature
    {
        [SerializeField] private float _moveSpeed;

        private Vector3 _direction;

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

        public override void Die()
        {
            base.Die();

            EnemyManager.Instance.UntrackEnemy(this);
        }
    }
}