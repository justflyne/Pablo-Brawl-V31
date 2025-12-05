namespace Supercell.Laser.Logic.Command.Home
{
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Logic.Home.Gatcha;
    using Supercell.Laser.Titan.DataStream;

    public class LogicSetTeamChatMutedCommand : Command
    {
        public bool DisabledTeamChat;

        public override void Encode(ByteStream stream)
        {

            stream.WriteBoolean(DisabledTeamChat);
            stream.WriteVInt(0);
            base.Encode(stream);

        }

        public override int Execute(HomeMode homeMode)
        {
            homeMode.Avatar.DisabledTeamChat = DisabledTeamChat;
            return 0;
        }

        public override int GetCommandType()
        {
            return 777;
        }
    }
}
