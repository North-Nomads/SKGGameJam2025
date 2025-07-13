using UnityEngine;

namespace HighVoltage.Services.Inputs
{
    public class DesktopInputService : InputService
    {
        private Vector3 _hitPointLastFrame;

        public override Vector3 MouseRaycastPosition => GetMouseRaycastPosition();

        private Vector3 GetMouseRaycastPosition()
        {
            /*Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity, 1 << GroundLayerIndex))
            {
                _hitPointLastFrame = hit.point;
                return hit.point;
            }
            return _hitPointLastFrame;*/
            return Vector3.zero;
        }
    }

}