using System;
using System.Collections;
using System.Collections.Generic;
using NSC.Number;
using NSC.Shop;
using UnityEngine;
using VInspector;

namespace NSC.Data
{
    [CreateAssetMenu]
    public class GoodsDefinition : ScriptableObject
    {

        [field: SerializeField, Header("Text")] public string GoodsName { get; private set; }
        [field: SerializeField, TextArea] public string GoodsDescription { get; private set; }

        [field: SerializeField] public int BaseCost { get; private set; }
        [field: SerializeField] public int CostInc { get; private set; }

        [SerializeField, Header("Type")] private ShopOption _type;
        public ShopOption Type { get => _type; private set => _type = value; }

        [field: SerializeField, ShowIf("_type", ShopOption.ImproveNode)] public Operator Op { get; private set; }
        
        [field: SerializeField, Space(10), EndIf] public int RandomWeight { get; private set; }

        [field: SerializeField, ShowIf("_type", ShopOption.NumberElement), Header("Number Element")]
        public NumberElement MinNumber { get; private set; }
        [field: SerializeField] public NumberElement MaxNumber { get; private set; }

        [field: SerializeField] public int MinCount { get; private set; }
        [field: SerializeField] public int MaxCount { get; private set; }


        public int Cost => BaseCost;

        public bool CanSellGoods()
        {
            switch (Type)
            {
                case ShopOption.ImproveNode:
                    switch (Op)
                    {
                        case Operator.Sum:
                            return DataManager.Instance.AddTime > DataManager.Instance.Data.SummonTime;
                        case Operator.Subtract:
                            return DataManager.Instance.SubtractTime > DataManager.Instance.Data.SummonTime;
                        case Operator.Multiply:
                            return DataManager.Instance.MultiplyTime > DataManager.Instance.Data.SummonTime;
                        case Operator.Divide:
                            return DataManager.Instance.DivideTime > DataManager.Instance.Data.SummonTime;
                        default:
                            return false;
                    }
                case ShopOption.ImprovePot:
                    return DataManager.Instance.PotGenerationTime > DataManager.Instance.Data.PotTimeMin;
                case ShopOption.AddPot:
                    return GameManager.Instance.CanAddPot && GameManager.Instance.HasPot;
                case ShopOption.IncreaseCapacity:
                    return GameManager.Instance.SummonCapacity < DataManager.Instance.Data.CapacityMax;
                case ShopOption.IncreaseMaxHP:
                    return GameManager.Instance.MaxHP < DataManager.Instance.Data.HPMax;
                case ShopOption.RecoverHP:
                    return GameManager.Instance.CurHP < GameManager.Instance.MaxHP;
                case ShopOption.ImproveDropRate:
                    return DataManager.Instance.NumberDropRate < DataManager.Instance.Data.NumberDropRateMax;
                case ShopOption.ImproveGoldValue:
                    return DataManager.Instance.GoldValue < DataManager.Instance.Data.GoldValueMax;
                case ShopOption.UnlockShopOption:
                    return ShopManager.Instance.MaxGoods < 5;
                case ShopOption.NumberElement:
                    return true;
                default:
                    return false;
            }
        }

        public void OnPurchase()
        {
            switch (Type)
            {
                case ShopOption.ImproveNode:
                    switch (Op)
                    {
                        case Operator.Sum:
                            DataManager.Instance.AddMultipler -= DataManager.Instance.Data.AddTimeMultiplierDec;
                            break;
                        case Operator.Subtract:
                            DataManager.Instance.SubtractMultipler -= DataManager.Instance.Data.SubtractTimeMultiplierDec;
                            break;
                        case Operator.Multiply:
                            DataManager.Instance.MultiplyMultipler -= DataManager.Instance.Data.MultiplyTimeMultiplierDec;
                            break;
                        case Operator.Divide:
                            DataManager.Instance.DivideMultipler -= DataManager.Instance.Data.DivideTimeMultiplierDec;
                            break;
                        default:
                            break;
                    }
                    break;
                case ShopOption.ImprovePot:
                    DataManager.Instance.PotMultiplier -= DataManager.Instance.Data.PotTimeMultiplierDec;
                    break;
                case ShopOption.AddPot:
                    GameManager.Instance.AddPot();
                    break;
                case ShopOption.IncreaseCapacity:
                    GameManager.Instance.SummonCapacity += DataManager.Instance.Data.CapacityInc;
                    break;
                case ShopOption.IncreaseMaxHP:
                    GameManager.Instance.MaxHP += DataManager.Instance.Data.HPInc;
                    GameManager.Instance.CurHP += DataManager.Instance.Data.HPInc;
                    break;
                case ShopOption.RecoverHP:
                    GameManager.Instance.CurHP += DataManager.Instance.Data.HPRecoverRatio * GameManager.Instance.MaxHP;
                    break;
                case ShopOption.ImproveDropRate:
                    DataManager.Instance.NumberDropRateExtra += DataManager.Instance.Data.NumberDropRateInc;
                    break;
                case ShopOption.ImproveGoldValue:
                    DataManager.Instance.GoldValueExtra += DataManager.Instance.Data.GoldValueInc;
                    break;
                case ShopOption.UnlockShopOption:
                    ShopManager.Instance.MaxGoods += 1;
                    break;
                case ShopOption.NumberElement:
                    // Pass
                    break;
                default:
                    break;
            }
        }
    }
}