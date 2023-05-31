using Game.StateMachine.States;
using UnityEngine;

namespace Game.StateMachine.Transitions
{
    public abstract class Transition : MonoBehaviour
    {
        [SerializeField] private State _targetState;
        public State TargetState => _targetState;
        public bool NeedTransit { get; protected set; }

        protected virtual void OnEnable()
        {
            NeedTransit = false;
        }
    }
}