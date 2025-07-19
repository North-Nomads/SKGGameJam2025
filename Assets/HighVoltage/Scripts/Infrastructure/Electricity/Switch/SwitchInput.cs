using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HighVoltage
{
    public class SwitchInput : MonoBehaviour, ICurrentReceiver
    {
        [SerializeField] SwitchMain switchMain;

        public ICurrentSource CurrentSource { get; private set; }

        public float Consumption => switchMain.Consumption;

        public LineRenderer Wire { get; set; }
        public SwitchMain SwitchMain => switchMain;

        public void AttachToSource(ICurrentSource currentProvider)
        {
            CurrentSource = currentProvider;
        }
    }
}
