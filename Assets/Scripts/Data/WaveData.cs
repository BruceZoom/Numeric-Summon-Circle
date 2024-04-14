using System;
using System.Collections.Generic;
using NSC.Number;
using UnityEngine;

namespace NSC.Data
{
    [CreateAssetMenu]
    public class WaveData : ScriptableObject
    {
        [Serializable]
        public class EnemyInfo
        {
            [Header("Deterministic Settings")]
            public NumberElement Number;
            public int Count;

            [Header("Non-Deterministic Settings")]
            public NumberElement MinNumber;
            public NumberElement MaxNumber;
        }

        public List<EnemyInfo> Enemies;
        public float MinSpawnDuration;
        public float MaxSpawnDuration;
        public float RestTime;

        public List<NumberElement> GenerateEnemyNumbers(EnemyInfo enemy)
        {
            var res = new List<NumberElement>();
            // random
            if (enemy.Number.Numerator == 0)
            {
                var num = NumberElement.RandomNumberElement(enemy.MinNumber, enemy.MaxNumber);
                for (int i = 0; i < enemy.Count; i++)
                {
                    res.Add(num);
                }
            }
            // fixed
            else
            {
                for (int i = 0; i < enemy.Count; i++)
                    res.Add(enemy.Number);
            }

            return res;
        }
    }
}