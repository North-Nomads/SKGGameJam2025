using System;
using System.Linq;
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

        private void OnValidate()
        {
            waypoints = transform.GetComponentsInChildren<Transform>().Where(x => x != transform).ToArray();
        }
    }
}