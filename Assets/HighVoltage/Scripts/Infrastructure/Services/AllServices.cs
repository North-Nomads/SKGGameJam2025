using HighVoltage.Services.Progress;
using System.Collections.Generic;

namespace HighVoltage.Infrastructure.Services
{
    public class AllServices
    {
        private static AllServices _instance;
        private readonly List<IProgressUpdater> _saveWriterServices = new();
        private readonly List<ISavedProgressReader> _saveReaderServices = new();

        public List<IProgressUpdater> SaveWriterServices
            => _saveWriterServices;

        public List<ISavedProgressReader> SaveReaderServices
            => _saveReaderServices;

        public static AllServices Container => _instance ??= new AllServices();

        public void RegisterSingle<TService>(TService serviceImplementation) where TService : IService
        {
            Implementation<TService>.ServiceInstance = serviceImplementation;

            if (serviceImplementation is ISavedProgressReader progressReader)
            {
                _saveReaderServices.Add(progressReader);
                if (serviceImplementation is IProgressUpdater progressWriter)
                    _saveWriterServices.Add(progressWriter);
            }
        }

        public TService Single<TService>() where TService : IService
            => Implementation<TService>.ServiceInstance;

        /// <summary>
        /// Static generic class would create a new instance on every different generic case
        /// </summary>
        /// <typeparam name="TService">Service type which implementation needs to be registered</typeparam>
        private static class Implementation<TService> where TService : IService
        {
            public static TService ServiceInstance;
        }
    }
}