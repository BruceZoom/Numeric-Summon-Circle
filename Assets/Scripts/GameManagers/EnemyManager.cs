using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NSC.Data;
using NSC.Number;
using NSC.Utils;
using TMPro;
using UnityEngine;

namespace NSC.Creature
{
    public class EnemyManager : PassiveSingleton<EnemyManager>
    {
        [SerializeField] private EnemyCreature _enemyPrefab;

        public List<EnemyCreature> EnemyCreatures { get; private set; }

        //[SerializeField] private float _spawnInterval;
        private float _spawnTimer;
        private float _restTimer;

        [SerializeField] private List<WaveData> _waves;

        private int _curWaveIdx = -1;
        private int _curEnemyIdx;

        private List<NumberElement> _waveNumbers;
        private List<NumberElement> _nextWaveNumbers;

        public bool LevelClear => _curWaveIdx >= _waves.Count;
        public bool WaveFinished => _curEnemyIdx >= _waveNumbers.Count;
        public WaveData CurWave => _waves[_curWaveIdx];
        public WaveData NextWave => _waves[Mathf.Min(_waves.Count - 1, _curWaveIdx + 1)];

        [SerializeField] private TextMeshProUGUI _waveText;


        private void Awake()
        {
            Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();

            EnemyCreatures = new List<EnemyCreature>();
            _waveNumbers = new List<NumberElement>();
            _nextWaveNumbers = new List<NumberElement>();

            GoToNextWave();
        }

        private void Update()
        {
            if (!LevelClear)
            {
                if (_restTimer <= 0)
                {
                    _spawnTimer -= Time.deltaTime;
                    if (_spawnTimer <= 0)
                    {
                        _spawnTimer = SpawnEnemy();

                        if (_spawnTimer < 0)
                        {
                            _restTimer = -_spawnTimer;
                            GoToNextWave();
                        }
                    }
                }
                else
                {
                    _restTimer -= Time.deltaTime;
                }
            }
        }

        private void GoToNextWave()
        {
            if (_nextWaveNumbers.Count == 0)
            {
                GenerateNextWave();
            }

            _curWaveIdx += 1;
            if (LevelClear)
            {
                _curWaveIdx -= 1;
            }

            // set current wave
            _waveNumbers.Clear();
            _waveNumbers = _nextWaveNumbers.ToList();
            _curEnemyIdx = 0;

            // prepare next wave
            GenerateNextWave();

            _waveText.text = $"Next Wave: ";
            bool isFirst = true;
            foreach (var number in _nextWaveNumbers.Distinct())
            {
                if (!isFirst) _waveText.text += ", ";
                _waveText.text += number.ToString();
                isFirst = false;
            }
        }

        private void GenerateNextWave()
        {
            // generate enemy list
            _nextWaveNumbers.Clear();
            foreach (var enemy in NextWave.Enemies)
            {
                var nums = NextWave.GenerateEnemyNumbers(enemy);
                _nextWaveNumbers = _nextWaveNumbers.Concat(nums).ToList();
            }
            _nextWaveNumbers = _nextWaveNumbers.Shuffle();
        }

        /// <summary>
        /// Returns minus value for restTimer value.
        /// </summary>
        /// <returns></returns>
        private float SpawnEnemy()
        {
            var number = _waveNumbers[_curEnemyIdx];
            _curEnemyIdx += 1;
            var pos = GetRandomSpawnPosition(number);
            var enemy = GameObject.Instantiate(_enemyPrefab, pos, Quaternion.identity);
            
            enemy.SetNumber(number);
            EnemyCreatures.Add(enemy);

            if (WaveFinished)
            {
                return -CurWave.RestTime;
            }
            else
            {
                return Random.Range(CurWave.MinSpawnDuration, CurWave.MaxSpawnDuration);
            }
        }

        private Vector3 GetRandomSpawnPosition(NumberElement number)
        {
            var dist = Random.Range(0f, 3f);
            Vector2 viewPos = Vector2.zero;
            if (dist <= 1)
            {
                viewPos = new Vector2(0, dist / 2 + 0.4f);
            }
            else if (dist <= 2 && Mathf.Abs(number.Numerator) <= 16)
            {
                viewPos = new Vector2(dist - 1, 1);
            }
            else
            {
                viewPos = new Vector2(1, (dist - 2) / 2 + 0.4f);
            }

            return Camera.main.ViewportToWorldPoint(viewPos).SetZ(0);
        }

        public void UntrackEnemy(EnemyCreature enemy)
        {
            EnemyCreatures.Remove(enemy);
        }
    }
}