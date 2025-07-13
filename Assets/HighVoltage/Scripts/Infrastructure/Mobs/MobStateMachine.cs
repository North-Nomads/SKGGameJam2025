using System;
using System.Collections.Generic;
using UnityEngine;

namespace HighVoltage.Infrastructure.Mobs
{
    public abstract class MobStateMachine
    {
        protected Dictionary<Type, IMobState> States;
        protected IMobState CurrentState;
        protected MobCombat MobCombat { get; }
        protected MobAnimator Animator { get; }

        public MobStateMachine(MobCombat mobCombat, MobAnimator animator)
        {
            MobCombat = mobCombat;
            Animator = animator;
        }

        public void Enter<TState>() where TState : class, IMobState
        {
            Debug.Log($"Entered {typeof(TState)} state from {CurrentState}");
            IMobState state = ChangeState<TState>();
            state.Enter();
        }

        private TState ChangeState<TState>() where TState : class, IMobState
        {
            // The first state could be null on programm start 
            CurrentState?.Exit();
            TState state = GetState<TState>();
            CurrentState = state;
            return state;
        }

        private TState GetState<TState>() where TState : class, IMobState
            => States[typeof(TState)] as TState;

        public void Update() 
            => CurrentState.Update();
    }
}