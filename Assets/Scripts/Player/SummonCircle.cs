using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NSC.Player
{
    public class SummonCircle : MonoBehaviour
    {
        [SerializeField] private List<SummonCircleNode> _nodes;

        public int TotalNodes => _nodes.Count;

        private void Awake()
        {
            // set up nodes
            for(int i = 0; i < _nodes.Count; i++)
            {
                var targets = _nodes.GetRange(i + 1, _nodes.Count - i - 1).ToList().Concat(_nodes.GetRange(0, i)).ToList();
                _nodes[i].SetUpNode(this, targets);
            }
        }
    }
}