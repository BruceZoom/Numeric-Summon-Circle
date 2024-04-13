using System.Collections;
using System.Collections.Generic;
using NSC.Number;
using NSC.Utils;
using UnityEngine;

namespace NSC.Creature
{
    public class EnemyManager : PassiveSingleton<EnemyManager>
    {
        [SerializeField] private EnemyCreature _enemyPrefab;

        public List<EnemyCreature> EnemyCreatures { get; private set; }

        [SerializeField] private float _spawnInterval;
        private float _spawnTimer;

        private void Awake()
        {
            Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();

            EnemyCreatures = new List<EnemyCreature>();
        }

        private void Update()
        {
            _spawnTimer -= Time.deltaTime;
            if (_spawnTimer <= 0)
            {
                _spawnTimer = _spawnInterval;

                SpawnEnemy();
            }
        }

        private void SpawnEnemy()
        {
            var dist = Random.Range(0, 3);
            Vector2 viewPos = Vector2.zero;
            if (dist <= 1)
            {
                viewPos = new Vector2(0, dist);
            }
            else if (dist <= 2)
            {
                viewPos = new Vector2(dist - 1, 1);
            }
            else
            {
                viewPos = new Vector2(1, dist - 2);
            }

            var enemy = GameObject.Instantiate(_enemyPrefab, Camera.main.ViewportToWorldPoint(viewPos).SetZ(0), Quaternion.identity);
            // TODO: test only
            enemy.SetNumber(new NumberElement(2, 1));
            EnemyCreatures.Add(enemy);
        }

        public void UntrackEnemy(EnemyCreature enemy)
        {
            EnemyCreatures.Remove(enemy);
        }
    }
}