using System;
using System.Collections;
using UnityEngine;

namespace HighVoltage.Infrastructure.Interactables
{
    public class NextLevelPortal : InteractableProp
    {
        [SerializeField] private float delayBeforeSwitchingToNextScene;

        public event EventHandler DelayAfterPortalJumpExpired = delegate { };

        protected override void OnPlayerApproach()
        {
            // Play portal open animation
        }

        protected override void OnPlayerBumped()
        {
            // Play player jump in portal animation
            StartCoroutine(WaitJumpAnimation());
        }

        protected override void OnPlayerMovedAway()
        {
            // Play portal close animation
        }

        private IEnumerator WaitJumpAnimation()
        {
            yield return new WaitForSeconds(delayBeforeSwitchingToNextScene);
            DelayAfterPortalJumpExpired(null, null);
        }
    }
}