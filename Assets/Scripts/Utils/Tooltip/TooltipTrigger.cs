using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace NSC.UI
{
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField, TextArea] private string _tooltip;

        [SerializeField] private UnityEvent<PointerEventData> _clickCallback;

        public void OnPointerClick(PointerEventData eventData)
        {
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
    }
}