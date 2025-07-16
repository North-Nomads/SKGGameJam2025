using System;
using System.Collections.Generic;

namespace HighVoltage
{
    public interface ICurrentSource : ICurrentObject
    {
        public IEnumerable<ICurrentReceiver> Receivers { get; }
        public event EventHandler OnOverload;
        public float Output { get; }
        public void AttachReceiver(ICurrentReceiver receiver);
        public void DetachAllReceivers();
        public void DetachReceiver(ICurrentReceiver receiver);
        public void RequestPower(float configPowerConsumption);
    }
}
