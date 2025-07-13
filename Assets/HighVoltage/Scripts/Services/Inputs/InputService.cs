using UnityEngine;

namespace HighVoltage.Services.Inputs
{
    public abstract class InputService : IInputService  
    {
        protected int GroundLayerIndex => 6;
        public abstract Vector3 MouseRaycastPosition { get; }
    }
}