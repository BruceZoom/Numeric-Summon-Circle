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
                for (int i = 0; i < enemy.Count; i++)
                {
                    var num = UnityEngine.Random.Range(enemy.MinNumber.Numerator, enemy.MaxNumber.Numerator);
                    var den = UnityEngine.Random.Range(enemy.MinNumber.Denominator, enemy.MaxNumber.Denominator);
                    num = (num == 0 ? 1 : num);
                    den = (den == 0 ? 1 : den);
                    res.Add(new NumberElement(num, den));
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