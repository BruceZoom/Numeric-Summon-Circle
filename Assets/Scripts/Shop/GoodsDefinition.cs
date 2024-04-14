using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSC.Shop
{
    [CreateAssetMenu]
    public class GoodsDefinition : ScriptableObject
    {
        [field: SerializeField] public string GoodsName { get; private set; }
        [field: SerializeField, TextArea] public string GoodsDescription { get; private set; }

        [field: SerializeField] public int BaseCost { get; private set; }

        [field: SerializeField, Space(10)] public int RandomWeight { get; private set; }

        

        public int Cost => BaseCost;

        public bool CanSellGoods()
        {
            return true;
        }
    }
}