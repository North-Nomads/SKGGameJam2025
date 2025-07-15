using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HighVoltage.Infrastructure.Sentry
{
    public class SentryShotgun : SentryTower
    {
        [SerializeField] private Transform bulletSpawnPoint;
        private List<Vector3> _bulletDirections = new();

        protected override void PerformAction()
        {
            for (int i = 0; i < Config.BulletsPerAction; i++)
            {
                Bullet bulletInstance = GameFactory.CreateBullet(at: bulletSpawnPoint);
                Vector3 direction = TargetPositionWithOffset();
                _bulletDirections.Add(direction);
                bulletInstance.Initialize(direction, Damage);
            }
        }

        protected override void Update()
        {
            base.Update();
            Vector3 left = TargetPositionWithOffset(-Config.BulletsAngleOffset / 2);
            Vector3 right = TargetPositionWithOffset(+Config.BulletsAngleOffset / 2);
            Debug.DrawLine(transform.position, left, Color.yellow);
            Debug.DrawLine(transform.position, right, Color.yellow);

            foreach (Vector3 direction in _bulletDirections) 
                Debug.DrawLine(transform.position, direction, Color.red);
        }

        private Vector3 TargetPositionWithOffset(float? betaAngle = null)
        {
            // Получаем угол в радианах
            float betaRadians;
            if (betaAngle is null) 
                betaRadians = Random.Range(-Config.BulletsAngleOffset / 2, Config.BulletsAngleOffset / 2) * Mathf.Deg2Rad;
            else
                betaRadians = betaAngle.Value * Mathf.Deg2Rad;

            // Вектор от текущей позиции к цели
            Vector3 direction = LockedTarget.position - transform.position;
            float deltaX = direction.x;
            float deltaY = direction.y;

            // Поворачиваем вектор на угол betaRadians (правильная матрица поворота)
            float rotatedX = deltaX * Mathf.Cos(betaRadians) - deltaY * Mathf.Sin(betaRadians);
            float rotatedY = deltaX * Mathf.Sin(betaRadians) + deltaY * Mathf.Cos(betaRadians);

            // Новая позиция = текущая позиция + повёрнутое направление
            Vector3 destination = transform.position + new Vector3(rotatedX, rotatedY, 0);
    
            return destination;
        }

        /*private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(LockedTarget.position, transform.position);
            foreach (var direction in _bulletDirections)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawLine(direction, transform.position);
                Gizmos.color = Color.green;
                Gizmos.DrawLine(direction, LockedTarget.position);
            }
        }*/
    }
}