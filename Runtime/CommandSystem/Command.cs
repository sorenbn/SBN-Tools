using System;

namespace SBN.CommandSystem
{
    public abstract class Command<T>
    {
        public event Action<Command<T>> OnCommandExecute;
        public event Action<Command<T>> OnCommandEnd;

        public bool Interrupted
        {
            get; private set;
        }

        public T Context
        {
            get;
            internal set;
        }

        public void ExecuteCommand()
        {
            OnCommandExecute?.Invoke(this);
            OnExecute();
        }

        public void EndCommand(bool interrupted = false)
        {
            Interrupted = interrupted;

            OnEnd();
            OnCommandEnd?.Invoke(this);
        }

        protected virtual void OnEnd()
        {
        }

        protected abstract void OnExecute();
    }
}