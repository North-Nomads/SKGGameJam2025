using System;
using HighVoltage.Infrastructure.Services;

namespace HighVoltage.Infrastructure.InGameTime
{
    public interface IInGameTimeService : IService
    {
        event EventHandler<bool> GamePaused;

        void EnablePause();
        void RestoreTimePassage();
    }
}