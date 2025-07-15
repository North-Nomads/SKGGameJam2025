using System.Collections.Generic;
using UnityEngine;
using HighVoltage.Infrastructure.Factory;
using HighVoltage.Data;
using HighVoltage.Services.Progress;

namespace HighVoltage.Infrastructure.SaveLoad
{
    public class PlayerPrefsSaveLoadService : ISaveLoadService
    {
        private const string ProgressKey = "Progress";

        private readonly IPlayerProgressService _progressService;
        private readonly IGameFactory _gameFactory;
        private readonly List<IProgressUpdater> _saveWriterServices;

        public PlayerPrefsSaveLoadService(IPlayerProgressService progressService, IGameFactory gameFactory, List<IProgressUpdater> savedServices)
        {
            _progressService = progressService;
            _gameFactory = gameFactory;
            _saveWriterServices = savedServices;
        }

        public PlayerProgress LoadProgress() => PlayerPrefs.GetString(ProgressKey)?.ToDeserialized<PlayerProgress>();

        public void SaveProgress()
        {
            foreach (var progressWriter in _gameFactory.ProgressWriters)
                progressWriter.UpdateProgress(_progressService.Progress);

            foreach (var writerService in _saveWriterServices)
                writerService.UpdateProgress(_progressService.Progress);

            PlayerPrefs.SetString(ProgressKey, _progressService.Progress.ToJson());
        }
    }
}