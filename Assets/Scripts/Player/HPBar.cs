using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NSC.Player
{
    public class HPBar : MonoBehaviour
    {
        [SerializeField] private float _animTime = 0.5f;
        public Transform FollowTarget { private get; set; }
        [field:SerializeField] private Image Fill { get; set; }
        [field: SerializeField] private TextMeshProUGUI Text { get; set; }

        // Update is called once per frame
        void Update()
        {
            //transform.position = Camera.main.WorldToScreenPoint(FollowTarget.position);
            transform.position = FollowTarget.position;
        }

        public void SetHP(float cur, float max)
        {
            Fill.DOFillAmount(cur / max, _animTime);
            Text.text = $"{cur:#.##} / {max}";
        }
    }
}