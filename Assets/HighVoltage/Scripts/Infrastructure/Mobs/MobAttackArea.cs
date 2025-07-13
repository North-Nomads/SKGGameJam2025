using UnityEngine;

namespace HighVoltage.Infrastructure.Mobs
{
    [RequireComponent(typeof(BoxCollider))]
    public class MobAttackArea : MonoBehaviour
    {
        private BoxCollider _boxCollider;

        public BoxCollider Collider => _boxCollider;

        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider>();
            _boxCollider.isTrigger = true;
            _boxCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;

        }

    }
}