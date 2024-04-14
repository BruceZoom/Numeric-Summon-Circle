using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NSC.Inventory;
using NSC.Number;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NSC.Player
{
    public class NumberSlot : MonoBehaviour, IDropHandler, IPointerClickHandler
    {
        [SerializeField] private TextMeshPro _text;

        public NumberElement Number { get; private set; }

        [SerializeField] private NumberElement _forceUpdateNumber;

        private SummonCircleNode _node;

        [SerializeField] private float _animScale;
        [SerializeField] private float _animDuration;
        private Tweener _anim;

        private void Awake()
        {
            _text.transform.rotation = Quaternion.identity;
        }

        public void SetUpSlot(SummonCircleNode node)
        {
            _node = node;
            SetNumber(null);
        }

        public void SetNumber(NumberElement number)
        {
            if (number?.Denominator == 0)
            {
                number = null;
            }

            Number = number;
            _text.text = number != null ? number.ToString() : "";
            //_node.SetIngredient(number);

            if (_anim == null || !_anim.IsActive())
            {
                _anim = transform.DOPunchScale(Vector3.one * _animScale, _animDuration, 1, 0.1f);
            }
        }

        public void ConsumeNumber()
        {
            InventoryManager.Instance.ConsumeNumber(Number);
        }

#if UNITY_EDITOR
        [ContextMenu("Force Set Number")]
        private void ForceSetNumber()
        {
            SetNumber(_forceUpdateNumber);
            if (!Application.isPlaying)
            {
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
                //UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(_node);
            }
        }
#endif

        public void OnDrop(PointerEventData eventData)
        {
            var ui = eventData.selectedObject.GetComponent<NumberElementUI>();
            if (ui == null) return;

            SetNumber(ui.Number);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                if (_anim == null || !_anim.IsActive())
                {
                    _anim = transform.DOPunchScale(-Vector3.one * _animScale, _animDuration, 1, 0.1f);
                }

                SetNumber(null);
            }
        }
    }
}