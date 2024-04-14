using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace NSC.VFX
{
    public class DamageVFX : MonoBehaviour
    {
        [SerializeField] private float _animDuration;

        public float AnimScale { private get; set; } = 1f;

        private void Start()
        {
            transform.DOScale(AnimScale, _animDuration)
                .From(0f)
                .SetUpdate(true)
                .OnComplete(delegate
                {
                    Destroy(gameObject, _animDuration);
                });
        }
    }
}