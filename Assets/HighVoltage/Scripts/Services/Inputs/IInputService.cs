using UnityEngine;
using HighVoltage.Infrastructure.Services;

namespace HighVoltage.Services.Inputs
{
    public interface IInputService : IService
    {
        Vector3 MouseRaycastPosition { get; }
    }
}