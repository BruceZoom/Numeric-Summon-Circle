using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using NSC.Creature;
using NSC.Inventory;
using NSC.Number;
using NSC.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NSC.Player
{
    public class SummonCircleNode : MonoBehaviour, IPointerClickHandler
    {
        [field: SerializeField] public Operator Op { get; private set; }
        public bool HasNoOperator => Op == Operator.None;

        private SummonCircle _circle;
        private List<SummonCircleNode> _targetNodes;
        private List<SummonCircleNode> _sourceQueue;

        private NumberSlot _ingredientSlot;
        public NumberElement Ingredient => _ingredientSlot.Number;

        private SpriteRenderer _sprite;


        [SerializeField] private float _baseProductionTime = 1f;
        public float ProductionTime => _baseProductionTime; // TODO:

        [SerializeField] private float _summonTime = 0.5f;


        [Header("Object References")]
        [SerializeField] private List<SpriteRenderer> _arrows;
        [SerializeField] private NumberElementObject _numberElementPrefab;

        [Header("Animation")]
        [SerializeField] private float _clickAnimScale = 0.9f;
        [SerializeField] private float _clickAnimTime = 0.5f;
        private Tweener _animCoroutine;

        // -1 for terminal nodes
        private int _curDirection = -1;
        public SummonCircleNode CurTargetNode => _curDirection >= 0 ? _targetNodes[_curDirection] : null;

        private NumberElement _product;
        private NumberElementObject _productObject;

        #region Status Checks
        [field: SerializeField, ReadOnly, Header("Readonly Status")] public bool IsWaitingProductionStart { get; private set; }
        [field: SerializeField, ReadOnly] public bool IsProductionFinished { get; private set; }
        public bool HasTarget => CurTargetNode != null;
        [field: SerializeField, ReadOnly] public bool IsProductAvailable { get; private set; }

        // input ready only when ingredient is set and inventory has this number
        // and this should either be source node or has an available source product
        public bool IsInputReady => Ingredient != null && Ingredient.Denominator != 0 &&
                                    InventoryManager.Instance.HasNumber(Ingredient) &&
                                    (HasNoOperator || _sourceQueue.Any(n => n.IsProductAvailable));
        #endregion

        private void Awake()
        {
            _sourceQueue = new List<SummonCircleNode>();

            _sprite = GetComponent<SpriteRenderer>();

            _ingredientSlot = GetComponentInChildren<NumberSlot>();
            _ingredientSlot.SetUpSlot(this);
        }

        private void Start()
        {
            // initialize production status
            IsWaitingProductionStart = true;
            IsProductionFinished = true;
            IsProductAvailable = false;
        }

        public void SetUpNode(SummonCircle circle, List<SummonCircleNode> targetNodes)
        {
            _circle = circle;
            _targetNodes = targetNodes;

            UpdateDisplay();
        }

        private void Update()
        {
            ProductionUpdate();
        }

        #region Edit Methods
        public void OnPointerClick(PointerEventData eventData)
        {
            // click again only when anim finishes
            if (_animCoroutine != null && _animCoroutine.IsActive()) return;

            if (eventData.button == PointerEventData.InputButton.Left)
            {
                ChangeDirection(1);
                _animCoroutine = transform.DOPunchScale(-Vector3.one * _clickAnimScale, _clickAnimTime, elasticity: 0.01f, vibrato: 1);
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                ChangeDirection(-1);
                _animCoroutine = transform.DOPunchScale(-Vector3.one * _clickAnimScale, _clickAnimTime, elasticity: 0.01f, vibrato: 1);
            }
        }

        private void ChangeDirection(int dir)
        {
            // dettach current target from this node
            if (_curDirection >= 0)
            {
                _targetNodes[_curDirection].RemoveSource(this);
            }

            // update direction
            if (dir == 1)
            {
                _curDirection += 1;
            }
            else
            {
                if (_curDirection == -1)
                {
                    _curDirection += _circle.TotalNodes - 1;
                }
                else
                {
                    _curDirection -= 1;
                }
            }
            // exceeds number of other nodes
            if (_curDirection >= _circle.TotalNodes - 1)
            {
                _curDirection = -1;
            }
            // skip if the current target is ultimate source
            if (_curDirection >= 0 && _targetNodes[_curDirection].HasNoOperator)
            {
                ChangeDirection(dir);
                return;
            }

            // attach current target to this node
            if (_curDirection >= 0)
            {
                _targetNodes[_curDirection].AddSource(this);
            }
            
            // update graphics
            UpdateDisplay();
        }

        private void RemoveSource(SummonCircleNode source)
        {
            _sourceQueue.Remove(source);
        }

        private void AddSource(SummonCircleNode source)
        {
            _sourceQueue.Add(source);
        }

        //public void SetIngredient(NumberElement number)
        //{
        //    Ingredient = number;
        //    Debug.Log($"Set ingredient to {number}");
        //}
        #endregion

        #region Summon Methods
        private void ProductionUpdate()
        {
            // not available during production
            if (!IsProductionFinished) return;

            // a production finishes
            if (!IsWaitingProductionStart)
            {
                //Debug.Log("Waiting to consume product.");
                // this is a terminal node and with available product, summon a unit
                if (!HasTarget && IsProductAvailable && _circle.CanSummon)
                {
                    // clear output
                    IsProductAvailable = false;

                    // summon
                    Summon();
                }

                // waiting for output available (i.e. last product taken)
                if (!IsProductAvailable)
                {
                    //Debug.Log("Product consumed.");
                    // start waiting for a new production process
                    IsWaitingProductionStart = true;
                }
            }

            //Debug.Log("Waiting to start new production.");
            // trying to start a new production process
            if (IsWaitingProductionStart && IsInputReady)
            {
                // no longer waiting start
                IsWaitingProductionStart = false;
                // new production not finished
                IsProductionFinished = false;

                // start production
                StartProduction();
            }
        }

        private void StartProduction()
        {
            // consume ingredient
            var operand2 = _ingredientSlot.Number;
            _ingredientSlot.ConsumeNumber();
            // consume source product if not the source node
            if (!HasNoOperator)
            {
                var node = _sourceQueue.Find(n => n.IsProductAvailable);
                if (node == null)
                {
                    Debug.LogError("Cannot start production without source product.");
                    // TODO: return ingredient to inventory
                    return;
                }
                // consume source product
                var operand1 = node.ConsumeProduct();
                if (operand1 == null)
                {
                    Debug.LogError("Get source product failed.");
                    // TODO: return ingredient to inventory
                    return;
                }
                // start production
                StartCoroutine(Production(operand1, operand2));
                // animation
                //StartProductionAnimation(operand1, node.transform.position);
                node.StartProductionAnimation(transform.position, ProductionTime);
                StartProductionAnimation(operand2, _ingredientSlot.transform.position);
            }
            else
            {
                // start production without first operand
                StartCoroutine(Production(null, operand2));
                // animation
                StartProductionAnimation(operand2, _ingredientSlot.transform.position);
            }
        }

        private void StartProductionAnimation(NumberElement number, Vector3 from)
        {
            var ne = GameObject.Instantiate(_numberElementPrefab);
            ne.SetNumber(number);
            ne.StartMoveAnimation(from, transform.position, ProductionTime);
        }

        private void StartProductionAnimation(Vector3 to, float duration)
        {
            _productObject.StartMoveAnimation(transform.position, to, duration);
        }

        IEnumerator Production(NumberElement operand1, NumberElement operand2)
        {
            // TODO: better production time calculation
            float timeRemain = ProductionTime;
            // production loop
            while (timeRemain > 0)
            {
                // TODO: update graphics

                yield return null;
                timeRemain -= Time.deltaTime;
            }
            // production finish
            FinishProduction(NumberElement.Calculate(Op, operand1, operand2));
        }

        private void FinishProduction(NumberElement product)
        {
            IsProductionFinished = true;

            // invalid result
            if (product.Denominator == 0)
            {
                // TODO:
                Debug.Log("Boom!!!");
                _product = null;
                IsProductAvailable = false;
            }
            // valid result
            else
            {
                _product = product;
                IsProductAvailable = true;
                _productObject = GameObject.Instantiate(_numberElementPrefab, transform.position, Quaternion.identity);
                _productObject.SetNumber(product);
            }
        }

        private void Summon()
        {
            var number = _product;
            _product = null;

            _productObject.StartMoveAnimation(transform.position, _circle.transform.position, _summonTime).onComplete += delegate
            {
                _circle.Summon(number);
            };
        }

        private NumberElement ConsumeProduct()
        {
            if (!IsProductAvailable)
            {
                Debug.LogError("Cannot get product because there is none.");
                return null;
            }

            IsProductAvailable = false;
            return _product;
        }
        #endregion

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