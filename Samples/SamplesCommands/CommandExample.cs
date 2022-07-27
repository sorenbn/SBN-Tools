using SBN.CommandSystem;

namespace SBN.Examples.CommandSystem
{
    public class CommandExample
    {
        private CommandHandler<CommandExample> commandHandler;

        public CommandExample()
        {
            commandHandler = new CommandHandler<CommandExample>(this, autoExecuteChain: true);

            var speakerCommand1 = new SpeakerCommand("HELLO");
            var speakerCommand2 = new SpeakerCommand("HELLO TO YOU TOO AS WELL");
            var speakerCommand3 = new SpeakerCommand("GOOD CHAT");

            commandHandler.AddCommand(speakerCommand1);
            commandHandler.AddCommand(speakerCommand2);
            commandHandler.AddCommand(speakerCommand3);
        }
    }

    public class SpeakerCommand : Command<CommandExample>
    {
        private readonly string message;

        public SpeakerCommand(string message)
        {
            this.message = message;
        }

        protected override void OnExecute()
        {
            System.Console.WriteLine(message);
            EndCommand();
        }

        protected override void OnEnd()
        {
            System.Console.WriteLine("Message was spoken. Ending commmand");
        }
    }
}
