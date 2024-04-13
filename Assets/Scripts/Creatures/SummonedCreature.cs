using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NSC.Creature
{
    public class SummonedCreature : NumberCreature
    {
        [SerializeField] private float _updateInterval;

        private Transform _trackTarget;

        [SerializeField] private float _moveSpeed;

        private float _nextTargetUpdateTime;

        private void Update()
        {
            // FIXME: use callbacks to unbind tracked but dead enemy
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
                }
            }

            if (transform != _trackTarget)
            {
                transform.transform.position += (_trackTarget.position - transform.position).normalized * _moveSpeed * Time.deltaTime;
            }
        }
    }
}