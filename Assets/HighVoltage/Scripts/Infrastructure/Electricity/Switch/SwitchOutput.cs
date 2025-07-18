using System;
using System.Collections.Generic;
using UnityEngine;

namespace HighVoltage
{
    public class SwitchOutput : MonoBehaviour, ICurrentSource
    {
        [SerializeField] private SwitchMain switchMain;


        private readonly List<ICurrentReceiver> _receivers = new();

        public IEnumerable<ICurrentReceiver> Receivers => _receivers;

        public bool IsActive { get; private set; }

        public float Output
        {
            get
            {
                float output = 0.0f;
                foreach (var receiver in Receivers)
                    output += receiver.Consumption;
                return output;
            }
        }

        public List<LineRenderer> Wires { get; } = new();

        public event EventHandler OnOverload;

        public void ChangeState(bool enable)
        {
            IsActive = enable;
        }

        public void AttachReceiver(ICurrentReceiver receiver)
        {
            _receivers.Add(receiver);
        }

        public void DetachAllReceivers()
        {
            foreach(var receiver in _receivers)
            {
                receiver.AttachToSource(null);
            }
            _receivers.Clear();
        }

        public void DetachReceiver(ICurrentReceiver receiver)
        {
            throw new NotImplementedException();
        }

        public void Overload() => OnOverload.Invoke(this, null);
    }
}
