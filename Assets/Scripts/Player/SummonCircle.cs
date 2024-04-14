using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NSC.Creature;
using NSC.Number;
using Unity.VisualScripting;
using UnityEngine;

namespace NSC.Player
{
    public class SummonCircle : MonoBehaviour
    {
        [SerializeField] private List<SummonCircleNode> _nodes;
        [SerializeField] private SummonedCreature _summonedCreaturePrefab;


        public int TotalNodes => _nodes.Count;

        public bool CanSummon => GameManager.Instance.CurrentSummons < GameManager.Instance.SummonCapacity;

        private void Awake()
        {
            // set up nodes
            for(int i = 0; i < _nodes.Count; i++)
            {
                var targets = _nodes.GetRange(i + 1, _nodes.Count - i - 1).ToList().Concat(_nodes.GetRange(0, i)).ToList();
                _nodes[i].SetUpNode(this, targets);
            }
        }

        public void Summon(NumberElement number)
        {
            var summon = GameObject.Instantiate(_summonedCreaturePrefab, transform.position, Quaternion.identity);
            summon.SetNumber(number);

            GameManager.Instance.CurrentSummons += 1;
        }
    }
}