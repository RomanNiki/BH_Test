using Game.StateMachine.States;
using Mirror;
using PlayerComponents;
using UnityEngine;

namespace Game.StateMachine
{
    public class GameStateMachine : NetworkBehaviour
    {
        [SerializeField] private State _defaultState;
        private State _currentState;
        private GameSystem _gameSystem;
        public State CurrentState => _currentState;
      
        private void Start()
        {
            _gameSystem = GetComponent<GameSystem>();
            ResetState(_defaultState);
        }

        private void Update()
        {
            if (_currentState == null) return;

            var next = _currentState.GetNext();
            if (next != null)
            {
                Transit(next);
            }
        }

        private void ResetState(State startState)
        {
            _currentState = startState;

            if (_currentState != null)
            {
                _currentState.Enter(_gameSystem);
            }
        }

        private void Transit(State nextState)
        {
            if (_currentState != null)
            {
                _currentState.Exit();
            }

            _currentState = nextState;

            if (nextState != null)
            {
                _currentState.Enter(_gameSystem);
            }
        }
    }
}