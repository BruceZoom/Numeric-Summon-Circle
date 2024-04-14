using System.Collections;
using System.Collections.Generic;
using NSC.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NSC.Number
{
    public class NumberElementUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private TextMeshProUGUI _numberText;

        [SerializeField] private TextMeshProUGUI _countText;

        [SerializeField] private NumberElementObject _numberElementPrefab;


        [field: SerializeField] public NumberElement Number { get; private set; }

        private NumberElementObject _numberElement;

        private void Awake()
        {
            _numberElement = GameObject.Instantiate(_numberElementPrefab);
            _numberElement.gameObject.SetActive(false);
        }

        public void SetCount(int count)
        {
            if (count < 0)
            {
                _countText.text = "inf";
            }
            else
            {
                _countText.text = count.ToString();
            }
        }

        public void SetNumber(NumberElement number)
        {
            Number = number;
            _numberText.text = number.ToString();
            _numberElement.SetNumber(number);
            _numberElement.SpriteRend.sortingLayerID = SortingLayer.NameToID("Element");
            _numberElement.NumberText.sortingLayerID = SortingLayer.NameToID("Element");
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _numberElement.gameObject.SetActive(true);
            eventData.selectedObject = gameObject;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _numberElement.gameObject.SetActive(false);
        }

        public void OnDrag(PointerEventData eventData)
        {
            _numberElement.transform.position = Camera.main.ScreenToWorldPoint(eventData.position).SetZ(0);
        }
    }
}