using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NSC.Audio;
using NSC.Creature;
using NSC.Number;
using NSC.Shop;
using NSC.UI;
using Unity.VisualScripting;
using UnityEngine;

namespace NSC.Player
{
    public class SummonCircle : MonoBehaviour
    {
        [SerializeField] private List<SummonCircleNode> _nodes;
        [SerializeField] private SummonedCreature _summonedCreaturePrefab;
        [SerializeField] private NumberElementObject _numberElementPrefab;
        [SerializeField] private Transform _hpPosition;

        public SpriteRenderer SpriteRend { get; private set; }

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

            SpriteRend = GetComponent<SpriteRenderer>();

            UIManager.Instance.HPBar.FollowTarget = _hpPosition;
        }

        public void Summon(NumberElement number)
        {
            // become money if zero
            if (number.Numerator == 0)
            {
                var reward = GameObject.Instantiate(_numberElementPrefab, transform.position, Quaternion.identity);
                reward.SetNumber(new NumberElement(0));
                reward.StartPickupAnimation(delegate
                {
                    ShopManager.Instance.Money += 1;
                });
            }
            // summon
            else
            {
                var summon = GameObject.Instantiate(_summonedCreaturePrefab, transform.position, Quaternion.identity);
                summon.SetNumber(number);

                GameManager.Instance.CurrentSummons += 1;

                AudioManager.Instance.PlayRandomSFX(AudioManager.Instance.SummonSFX);
            }
        }
    }
}