using System;
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
            if (currentProvider != null)
                currentProvider.OnOverload += Overload;
        }

        private void Overload(object sender, EventArgs e)
        {
            
        }
    }
}
