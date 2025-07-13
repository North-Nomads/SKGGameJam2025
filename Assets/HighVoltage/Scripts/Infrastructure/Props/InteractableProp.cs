using UnityEngine;

namespace HighVoltage.Infrastructure.Interactables
{
    [RequireComponent(typeof(BoxCollider))]
    public abstract class InteractableProp : MonoBehaviour
    {
        [SerializeField, Min(0)] private float approachRadius;
        [SerializeField, Min(0)] private float bumpRadius;
        [SerializeField] private BoxCollider interactionBox;

        private Transform _nearestPlayer;
        private float _distanceToPlayer;
        private bool _isPlayerInBumpRadius;

        private void OnValidate()
        {
            interactionBox = GetComponent<BoxCollider>();
            interactionBox.isTrigger = true;
            interactionBox.size = Vector3.one * approachRadius;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;

            _nearestPlayer = other.transform;
            OnPlayerApproach();
        }

        private void OnTriggerExit(Collider other)
        {
            // Trigger only on player
            if (!other.CompareTag("Player"))
                return;

            // Player was near but now not near
            if (_nearestPlayer != null)
                OnPlayerMovedAway();
            
            _nearestPlayer = null;
        }

        private void Update()
        {
            if (_nearestPlayer == null)
                return;

            _distanceToPlayer = Vector3.Distance(_nearestPlayer.position, transform.position);
            if (_distanceToPlayer <= bumpRadius)
            {
                // Don't spam OnPlayerBumped on every update frame
                if (!_isPlayerInBumpRadius)
                    OnPlayerBumped();
                
                _isPlayerInBumpRadius = true;
            }
            else
            {
                _isPlayerInBumpRadius = false;
            }
        }

        protected abstract void OnPlayerApproach();
        protected abstract void OnPlayerMovedAway();
        protected abstract void OnPlayerBumped();
    }
}