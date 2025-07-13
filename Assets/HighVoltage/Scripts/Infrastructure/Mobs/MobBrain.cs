using System;
using HighVoltage.Enemy;
using UnityEngine;

namespace HighVoltage.Infrastructure.Mobs
{
    public class MobBrain : MonoBehaviour
    {
        public event EventHandler<MobBrain> OnMobDied = delegate { };
        private const float MinDistanceToWaypoint = 0.01f;

        private MobConfig _config;
        private Transform[] _waypoints;
        private Transform _target;
        private int _waypointIndex;

        public void Initialize(Transform[] waypoints, MobConfig config)
        {
            _waypoints = waypoints;
            _target = waypoints[0];
            _waypointIndex = 0;
            _config = config;
        }

        private void Update()
        {
            if (Vector3.Distance(transform.position, _target.position) <= MinDistanceToWaypoint)
                MoveToNextTargetWaypoint();
            else
                transform.Translate(Time.deltaTime * _config.MoveSpeed * (_target.position - transform.position).normalized);
        }

        private void MoveToNextTargetWaypoint()
        {
            _waypointIndex++;
            _target = _waypoints[_waypointIndex];
        }
    }
}