namespace HighVoltage
{
    public interface ICurrentReceiver : ICurrentObject
    {
        public ICurrentSource CurrentProvider { get; }

        public void AttachToSource(ICurrentSource currentProvider);

        public float Consumption { get; }
    }
}
