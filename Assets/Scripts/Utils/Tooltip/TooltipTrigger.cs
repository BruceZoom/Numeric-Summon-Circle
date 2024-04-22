using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace NSC.UI
{
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerMoveHandler
    {
        [SerializeField, TextArea] private string _tooltip;

        [SerializeField] private bool _rightClickOnly;

        [SerializeField] private UnityEvent<PointerEventData> _clickCallback;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_rightClickOnly && eventData.button == PointerEventData.InputButton.Left) return;

            _clickCallback?.Invoke(eventData);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            UIManager.Instance.Tooltip.DisplayTooltip(_tooltip, Camera.main.ScreenToWorldPoint(eventData.position));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            UIManager.Instance.Tooltip.HideTooltip();
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            UIManager.Instance.Tooltip.SetToolTipPos(Camera.main.ScreenToWorldPoint(eventData.position));
        }
    }
}