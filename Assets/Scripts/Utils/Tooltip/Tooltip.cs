using System.Collections;
using System.Collections.Generic;
using NSC.Utils;
using TMPro;
using UnityEngine;

namespace NSC.UI
{
    public class Tooltip : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private RectTransform _panelRect;
        [SerializeField] private float _maxWidth;

        private void Awake()
        {
            //_text = GetComponentInChildren<TextMeshProUGUI>();

            //_maxWidth = _panelRect.sizeDelta.y;
            //_maxWidth = 400;

            gameObject.SetActive(false);
        }

        public void SetToolTipPos(Vector3 position)
        {
            transform.position = position.SetZ(0);

            float width = Mathf.Min(_text.preferredWidth, _maxWidth);
            _panelRect.sizeDelta.Set(_panelRect.sizeDelta.x, width);

            //Vector2 min = Camera.main.WorldToViewportPoint(_panelRect.rect.min + _panelRect.position);
            //Vector2 max = Camera.main.WorldToViewportPoint(_panelRect.rect.max + _panelRect.anchoredPosition);

            Vector2 center = Camera.main.WorldToViewportPoint(position);

            // adjust x
            if (center.x < 0.5)
            {
                _panelRect.pivot = new Vector2(0, _panelRect.pivot.y);
            }
            else
            {
                _panelRect.pivot = new Vector2(1, _panelRect.pivot.y);
            }
            //if (min.x < 0)
            //{
            //    _panelRect.pivot = new Vector2(0, _panelRect.pivot.y);
            //}
            //else if (max.x > 1)
            //{
            //    _panelRect.pivot = new Vector2(1, _panelRect.pivot.y);
            //}
            // adjust y
            if (center.y < 0.5)
            {
                _panelRect.pivot = new Vector2(_panelRect.pivot.x, 0);
            }
            else
            {
                _panelRect.pivot = new Vector2(_panelRect.pivot.x, 1);
            }
            //if (min.y < 0)
            //{
            //    _panelRect.pivot = new Vector2(_panelRect.pivot.x, 0);
            //}
            //else if (max.y > 1)
            //{
            //    _panelRect.pivot = new Vector2(_panelRect.pivot.x, 1);
            //}

            _panelRect.anchoredPosition = Vector2.zero;
        }

        public void DisplayTooltip(string tip, Vector3 position)
        {
            gameObject.SetActive(true);

            _text.text = tip;

            SetToolTipPos(position);
        }

        public void HideTooltip()
        {
            gameObject.SetActive(false);
        }
    }
}