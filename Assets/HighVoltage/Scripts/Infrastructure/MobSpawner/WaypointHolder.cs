using UnityEngine;

namespace HighVoltage.Infrastructure.MobSpawning
{
    /// <summary>
    /// REPLACE WITH TILE SERVICE 
    /// </summary>
    public class WaypointHolder : MonoBehaviour
    {
        public Transform[] Waypoints => waypoints;
        [SerializeField] private Transform[] waypoints;
    }
}