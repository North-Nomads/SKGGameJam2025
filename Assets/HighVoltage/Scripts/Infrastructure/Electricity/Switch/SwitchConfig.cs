using UnityEngine;

namespace HighVoltage
{
    [CreateAssetMenu(fileName = "SwitchConfig", menuName = "Config/Switch Config", order = 1)]
    public class SwitchConfig : BuildingConfig
    { 
        [SerializeField] private SwitchMain switchSwitchPrefab;

        public SwitchMain SwitchPrefab => switchSwitchPrefab;
    }
}