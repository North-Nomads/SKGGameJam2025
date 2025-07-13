using System;
using UnityEngine;
using HighVoltage.Infrastructure.MobSpawnerService;
using HighVoltage.Infrastructure.Services;

namespace HighVoltage.Infrastructure.Mobs
{
    [RequireComponent(typeof(MobAnimator), typeof(MobCombat))]
    public abstract class MobBrain : MonoBehaviour
    {
        protected MobCombat MobCombat;
        protected MobAnimator MobAnimator;
        protected MobStateMachine StateMachine;
        protected IMobSpawnerService MobSpawner;

        public event EventHandler<MobBrain> OnMobDied = delegate { };

        protected abstract void OnStart();

        private void Awake()
        {
            MobSpawner = AllServices.Container.Single<IMobSpawnerService>();
        }

        private void Start()
        {
            MobCombat = GetComponent<MobCombat>();
            MobCombat.MobHealthBelowZero += HandleMobDeath;
            MobAnimator = GetComponent<MobAnimator>();

            OnStart();
        }

        private void HandleMobDeath(object sender, EventArgs e)
        {
            Destroy(gameObject);
            OnMobDied(null, this);
        }

        private void Update() 
            => StateMachine.Update();
    }
}