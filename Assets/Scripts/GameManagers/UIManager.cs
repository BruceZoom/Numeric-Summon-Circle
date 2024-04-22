using System.Collections;
using System.Collections.Generic;
using NSC.Player;
using NSC.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NSC.UI
{
    public class UIManager : PassiveSingleton<UIManager>
    {
        [field: SerializeField] public Transform MoneyUI { get; private set; }
        [field: SerializeField] public Transform InventoryUI { get; private set; }
        [field: SerializeField] public TextMeshProUGUI MoneyText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI CapacityText { get; private set; }
        [field: SerializeField] public HPBar HPBar { get; private set; }
        [field: SerializeField] public Tooltip Tooltip { get; private set; }

        [field: SerializeField] public GameObject Help { get; private set; }

        [field: SerializeField] public GameObject GameOverPanel { get; private set; }

        private Button _retryButton;

        public void OpenHelp()
        {
            Help.SetActive(true);
            Time.timeScale = 0f;
        }

        public void CloseHelp()
        {
            Help.SetActive(false);
            Time.timeScale = 1f;
        }

        public override void Initialize()
        {
            base.Initialize();

            _retryButton = GameOverPanel.GetComponentInChildren<Button>();
            GameOverPanel.SetActive(false);
            Help.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _retryButton.onClick.AddListener(GameManager.Instance.Retry);
        }

        private void OnDisable()
        {
            _retryButton.onClick.RemoveListener(GameManager.Instance.Retry);
        }
    }
}