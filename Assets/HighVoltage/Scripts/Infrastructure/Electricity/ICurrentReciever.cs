namespace HighVoltage
{
    public interface ICurrentReciever : ICurrentObject
    {
        public ICurrentSource CurrentProvider { get; }

        public void AttachToSource(ICurrentSource currentProvider);
    }
}
