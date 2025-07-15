namespace HighVoltage
{
    public interface ICurrentReciever
    {
        public ICurrentSource CurrentProvider { get; }

        public void AttachToSource(ICurrentSource currentProvider);
    }
}
