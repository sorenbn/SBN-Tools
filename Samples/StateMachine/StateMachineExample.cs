using SBN.StateMachines;
using UnityEngine;

namespace SBN.Examples.StateMachine
{
    public class StateMachineExample : MonoBehaviour
    {
        private StateMachine<PlayerMock> playerStateMachine;
        private PlayerMock player;

        private void Start()
        {
            player = new PlayerMock { Health = 100, IsDead = false };

            playerStateMachine = new StateMachine<PlayerMock>(player, new PlayerMock.IdleState());
            playerStateMachine.AddState(new PlayerMock.MoveState());
        }

        private void Update()
        {
            playerStateMachine.Update(Time.deltaTime);
        }
    }

    public class PlayerMock
    {
        public int Health
        {
            get; set;
        }
        public bool IsDead
        {
            get; set;
        }

        public void Idle()
        {
            // Doing idle stuff
        }

        public void Move()
        {
            // Doing move stuff
        }

        public class IdleState : State<PlayerMock>
        {
            public override void EnterState()
            {
                Debug.Log($"Entered idle state");
            }

            public override void ExitState()
            {
                Debug.Log($"Exit idle state");
            }

            public override void UpdateState(float deltaTime)
            {
                Debug.Log($"Updating idle state");
                Context.Idle();
            }

            public override void CheckTransitions()
            {
                if (Input.GetKeyDown(KeyCode.M))
                {
                    StateMachine.ChangeState<MoveState>();
                }
            }
        }

        public class MoveState : State<PlayerMock>
        {
            public override void EnterState()
            {
                Debug.Log($"Entered move state");
            }

            public override void ExitState()
            {
                Debug.Log($"Exit move state");
            }

            public override void UpdateState(float deltaTime)
            {
                Debug.Log($"Update move state");
                Context.Move();
            }

            public override void CheckTransitions()
            {
                if (Input.GetKeyDown(KeyCode.I))
                {
                    StateMachine.ChangeState<IdleState>();
                }
            }
        }
    }
}
