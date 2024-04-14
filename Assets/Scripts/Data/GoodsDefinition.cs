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
            return true;
        }
    }
}