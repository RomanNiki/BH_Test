using System.Collections.Generic;
using Game.StateMachine.Transitions;
using Mirror;
using UnityEngine;

namespace Game.StateMachine.States
{
    public abstract class State : NetworkBehaviour
    {
        [SerializeField] private List<Transition> _transitions;
        protected GameSystem GameSystem { get; set; }

        public void Enter(GameSystem gameSystem)
        {
            if (enabled) return;
            enabled = true;
            GameSystem = gameSystem;
            foreach (var transition in _transitions)
            {
                transition.enabled = true;
            }
        }

        public void Exit()
        {
            if (enabled == false) return;
            foreach (var transition in _transitions)
                transition.enabled = false;
            enabled = false;
        }

        public State GetNext()
        {
            foreach (var transition in _transitions)
            {
                if (transition.NeedTransit)
                {
                    return transition.TargetState;
                }
            }

            return null;
        }
    }
}