using System;
using System.Collections.Generic;

namespace HighVoltage
{
    public interface ICurrentSource
    {
        public List<ICurrentReciever> Recievers { get; }

        public event EventHandler OnOverload;
        public float Output { get; }
        public void AttachReciever(ICurrentReciever reciever);
        public void RequestPower();
    }
}
