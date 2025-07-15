using UnityEngine;

namespace HighVoltage
{
    public class Building : MonoBehaviour
    {
        private int _hitPoints;
        private int _requiredVoltage;
        private int _currentVoltage;

        public int HitPoints => _hitPoints;
        public int DemandedVoltage => _currentVoltage;

        private void Update()
        {
            if (_requiredVoltage > _currentVoltage)
                return;
            Decay();
        }

        private void Decay()
        {
            
        }
    }
}
