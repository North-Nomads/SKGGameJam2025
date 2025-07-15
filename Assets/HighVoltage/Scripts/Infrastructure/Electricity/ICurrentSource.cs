using System;
using System.Collections.Generic;

namespace HighVoltage
{
    public interface ICurrentSource : ICurrentObject
    {
        public IEnumerable<ICurrentReciever> Recievers { get; }
        public event EventHandler OnOverload;
        public float Output { get; }
        public void AttachReceiver(ICurrentReciever receiver);
        public void RequestPower(float configPowerConsumption);
    }
}
