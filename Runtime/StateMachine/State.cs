namespace SBN.StateMachines
{
    public abstract class State<T> where T : class
    {
        public StateMachine<T> StateMachine
        {
            get;
            set;
        }

        /// <summary>
        /// Context object, which is the shared object between all the states.
        /// </summary>
        public T Context
        {
            get;
            set;
        }

        /// <summary>
        /// Called whenever a state is being entered
        /// </summary>
        public virtual void EnterState()
        {

        }

        /// <summary>
        /// Called before update state. Used to determine if need to change
        /// to a new state via StateMachine.ChangeState<AnotherState>()
        /// </summary>
        public virtual void CheckTransitions()
        {

        }

        /// <summary>
        /// Called every frame
        /// </summary>
        /// <param name="deltaTime"></param>
        public virtual void UpdateState(float deltaTime)
        {

        }

        /// <summary>
        /// Called whenever a state is being exited
        /// </summary>
        public virtual void ExitState()
        {

        }
    }
}
