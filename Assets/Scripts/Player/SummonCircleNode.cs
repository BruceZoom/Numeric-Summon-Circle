using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NSC.Player
{
    public class SummonCircleNode : MonoBehaviour, IPointerClickHandler
    {
        [field: SerializeField] public bool HasNoOperator { get; private set; }

        private SummonCircle _circle;
        private List<SummonCircleNode> _targetNodes;
        private List<SummonCircleNode> _sourceNodes;

        private SpriteRenderer _sprite;

        [Header("Object References")]
        [SerializeField] private List<SpriteRenderer> _arrows;

        [Header("Animation")]
        [SerializeField] private float _clickAnimScale = 0.9f;
        [SerializeField] private float _clickAnimTime = 0.5f;
        private Tweener _animCoroutine;

        // -1 for terminal nodes
        private int _curDirection = -1;

        private void Awake()
        {
            _sourceNodes = new List<SummonCircleNode>();

            _sprite = GetComponent<SpriteRenderer>();
        }

        public void SetUpNode(SummonCircle circle, List<SummonCircleNode> targetNodes)
        {
            _circle = circle;
            _targetNodes = targetNodes;

            UpdateDisplay();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            // click again only when anim finishes
            if (_animCoroutine != null && _animCoroutine.IsActive()) return;

            ChangeDirection();
            _animCoroutine = transform.DOPunchScale(-Vector3.one * _clickAnimScale, _clickAnimTime, elasticity: 0.01f, vibrato: 1);
        }

        private void ChangeDirection()
        {
            // update direction
            _curDirection += 1;
            // exceeds number of other nodes
            if (_curDirection >= _circle.TotalNodes - 1)
            {
                _curDirection = -1;
            }
            // skip if the current target is ultimate source
            if (_curDirection >= 0 && _targetNodes[_curDirection].HasNoOperator)
            {
                ChangeDirection();
                return;
            }

            // update graphics
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            //Debug.Log($"Change direction to {_curDirection}");
            _arrows.ForEach(a => a.gameObject.SetActive(false));
            if (_curDirection >= 0)
            {
                _arrows[_curDirection].gameObject.SetActive(true);
            }
        }
    }
}