using UnityEngine;

namespace HighVoltage
{
    public interface ICurrentReceiver : ICurrentObject
    {
        public ICurrentSource CurrentSource { get; }
        public void AttachToSource(ICurrentSource currentProvider);
        public float Consumption { get; }
        public LineRenderer Wire { get; set; }
    }
}
