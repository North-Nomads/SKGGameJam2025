using UnityEngine;

namespace HighVoltage.Infrastructure.CameraService
{
    public class CameraBounds : MonoBehaviour
    {
        [SerializeField] private Transform top;
        [SerializeField] private Transform down;
        [SerializeField] private Transform left;
        [SerializeField] private Transform right;

        public float Top => top.position.y;
        public float Down => down.position.y;
        public float Left => left.position.x;
        public float Right => right.position.x;
    }
}