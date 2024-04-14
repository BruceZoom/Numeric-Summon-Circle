using System.Collections;
using System.Collections.Generic;
using NSC.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NSC.UI
{
    public class UIManager : PassiveSingleton<UIManager>
    {
        [field: SerializeField] public Transform InventoryUI { get; private set; }
        [field: SerializeField] public TextMeshProUGUI MoneyText { get; private set; }


        private void Awake()
        {
            Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();
        }
    }
}