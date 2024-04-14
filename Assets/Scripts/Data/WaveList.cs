using NSC.Number;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace NSC.Data
{
    [CreateAssetMenu]
    public class WaveList : ScriptableObject
    {
        public List<WaveData> Waves;
    }
}