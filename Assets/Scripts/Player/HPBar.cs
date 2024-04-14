using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NSC.Player
{
    public class HPBar : MonoBehaviour
    {
        public Transform FollowTarget { private get; set; }
        [field:SerializeField] public Image Fill { get; private set; }

        // Update is called once per frame
        void Update()
        {
            //transform.position = Camera.main.WorldToScreenPoint(FollowTarget.position);
            transform.position = FollowTarget.position;
        }
    }
}