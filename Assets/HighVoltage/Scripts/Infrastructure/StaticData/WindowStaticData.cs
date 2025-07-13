using System.Collections.Generic;
using UnityEngine;

namespace HighVoltage.StaticData
{
    [CreateAssetMenu(fileName = "Windows", menuName = "Config/Window static data", order = 1)]
    public class WindowStaticData : ScriptableObject
    {
        public List<WindowConfig> Configs;
    }
}