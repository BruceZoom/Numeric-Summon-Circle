using UnityEngine;

namespace NSC.Data
{
    [CreateAssetMenu]
    public class DataDefinition : ScriptableObject
    {
        [field: SerializeField, Header("Summon Circle Data")] public float AddTimeBase { get; private set; }
        [field: SerializeField] public float MultiplyTimeBase { get; private set; }
        [field: SerializeField] public float SubtractTimeBase { get; private set; }
        [field: SerializeField] public float DivideTimeBase { get; private set; }
        [field: SerializeField, Space(5)] public float AddTimeMultiplierDec { get; private set; }
        [field: SerializeField] public float MultilyTimeMultiplierDec { get; private set; }
        [field: SerializeField] public float SubtractTimeMultiplierDec { get; private set; }
        [field: SerializeField] public float DivideTimeMultiplierDec { get; private set; }

        [field: SerializeField, Space(5)] public float SummonTime { get; private set; }

        [field: SerializeField, Header("Pot Data")] public float PotTimeBase { get; private set; }
        [field: SerializeField] public float PotTimeMultiplierDec { get; private set; }
        [field: SerializeField] public float PotTimeMin { get; private set; }


        [field: SerializeField, Header("Player Data")] public int CapacityBase { get; private set; }
        [field: SerializeField] public int CapacityInc { get; private set; }
        [field: SerializeField] public int CapacityMax { get; private set; }
        [field: SerializeField, Space(5)] public float HPBase { get; private set; }
        [field: SerializeField] public float HPInc { get; private set; }
        [field: SerializeField] public float HPMax { get; private set; }
        [field: SerializeField] public float HPRecoverRatio { get; private set; }


        [field: SerializeField, Header("Battle Data")] public float NumberDropRateBase { get; private set; }
        [field: SerializeField] public float NumberDropRateInc { get; private set; }
        [field: SerializeField] public float NumberDropRateMax { get; private set; }
        [field: SerializeField, Space(5)] public int GoldValueBase { get; private set; }
        [field: SerializeField] public int GoldValueInc { get; private set; }
        [field: SerializeField] public int GoldValueMax { get; private set; }

        [field: SerializeField, Header("Element Cost")] public float MagnitudeCostCoef { get; private set; }
        [field: SerializeField] public float DenoDiscountLimit { get; private set; } = 0.5f;
    }
}