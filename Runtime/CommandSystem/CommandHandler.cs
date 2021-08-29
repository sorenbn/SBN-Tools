using System.Collections.Generic;

namespace SBN.CommandSystem
{
    public class CommandHandler<T>
    {
        private Queue<Command<T>> activeCommands = new Queue<Command<T>>();

        private bool busy;

        public bool AutoExecuteChain
        {
            get; set;
        }
        public T Context { get;
            private set; }

        /// <summary>
        /// Default constructor for a CommandHandler
        /// </summary>
        /// <param name="autoExecuteChain">Determines when a command has finished, if the next command in queue would be automatically executed</param>
        public CommandHandler(T context, bool autoExecuteChain)
        {
            Context = context;
            AutoExecuteChain = autoExecuteChain;
        }

        /// <summary>
        /// Add a command to the command queue
        /// </summary>
        /// <param name="command">command</param>
        /// <param name="attemptAutoExecute">Determines if the commandhandler should attempt to execute the command immediately after being added</param>
        public void AddCommand(Command<T> command, bool attemptAutoExecute = true)
        {
            command.Context = Context;
            activeCommands.Enqueue(command);

            if (attemptAutoExecute)
                AttemptExecuteNextCommand();
        }

        /// <summary>
        /// Add multiple commands to the command queue
        /// </summary>
        /// <param name="command">command</param>
        /// <param name="attemptAutoExecute">Determines if the commandhandler should attempt to execute the commands immediately after being added</param>
        public void AddCommands(Queue<Command<T>> commands, bool attemptAutoExecute = true)
        {
            foreach (var command in commands)
                AddCommand(command, false);

            if (attemptAutoExecute)
                AttemptExecuteNextCommand();
        }

        public Command<T> AttemptExecuteNextCommand()
        {
            if (busy || activeCommands.Count == 0)
                return null;

            busy = true;

            var command = activeCommands.Dequeue();
            command.OnCommandEnd += Command_OnCommandEnd;

            command.ExecuteCommand();

            return command;
        }

        public int GetActiveCommandsCount()
        {
            return activeCommands.Count;
        }

        public bool AnyCommandsLeft()
        {
            return activeCommands.Count > 0;
        }

        public Command<T> PeekNextCommand()
        {
            return activeCommands.Peek();
        }

        public void ClearCommands()
        {
            activeCommands.Clear();
        }

        private void Command_OnCommandEnd(Command<T> command)
        {
            command.OnCommandEnd -= Command_OnCommandEnd;
            busy = false;

            if (AutoExecuteChain)
                AttemptExecuteNextCommand();
        }
    }
}