using System.Collections.Generic;
using UnityEngine;
using HighVoltage.StaticData;

namespace HighVoltage.StaticData
{
    [CreateAssetMenu(fileName = "GameWindows", menuName = "Config/Game window static data", order = 2)]
    public class GameWindowStaticData : ScriptableObject
    {
        public List<GameWindowConfig> Configs;
    }
}