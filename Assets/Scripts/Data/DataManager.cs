using System.Collections;
using System.Collections.Generic;
using NSC.Utils;
using UnityEngine;

namespace NSC.Data
{
    public class DataManager : PassiveSingleton<DataManager>
    {
        [field: SerializeField] public DataDefinition Data { get; private set; }

        #region Summon Circle Data
        public float AddMultipler { get; set; } = 1;
        public float SubtractMultipler { get; set; } = 1;
        public float MultiplyMultipler { get; set; } = 1;
        public float DivideMultipler { get; set; } = 1;

        public float AddTime => Mathf.Max(AddMultipler * Data.AddTimeBase, Data.SummonTime);
        public float SubtractTime => Mathf.Max(SubtractMultipler * Data.SubtractTimeBase, Data.SummonTime);
        public float MultiplyTime => Mathf.Max(MultiplyMultipler * Data.MultiplyTimeBase, Data.SummonTime);
        public float DivideTime => Mathf.Max(DivideMultipler * Data.DivideTimeBase, Data.SummonTime);
        #endregion

        #region Pot Data
        public float PotGenerationTime => Mathf.Max(Data.PotTimeBase * PotMultiplier, Data.PotTimeMin);
        public float PotMultiplier { get; set; } = 1;
        #endregion

        #region Battle Data
        public float NumberDropRateExtra { get; set; } = 0;
        public float NumberDropRate => Data.NumberDropRateBase + NumberDropRateExtra;

        public int GoldValueExtra { get; set; } = 0;
        public int GoldValue => Data.GoldValueBase + GoldValueExtra;
        #endregion

        public override void Initialize()
        {
            base.Initialize();
        }
    }
}