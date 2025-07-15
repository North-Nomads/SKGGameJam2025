using System;
using System.Collections.Generic;

namespace HighVoltage
{
    public interface ICurrentSource
    {
        public List<ICurrentReciever> Receivers { get; }
        public event EventHandler OnOverload;
        public float Output { get; }
        public void AttachReceiver(ICurrentReciever receiver);
        public void RequestPower(float configPowerConsumption);
    }
}
