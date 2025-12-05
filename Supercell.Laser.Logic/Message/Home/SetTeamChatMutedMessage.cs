// I DONT KNOW ACTUALLY HOW LOOKS SetTeamChatMutedMessage, SO I COPIED DoNotDisturbMessage

namespace Supercell.Laser.Logic.Message.Team
{
    public class SetTeamChatMutedMessage : GameMessage
    {
        public bool State;

        public override void Decode()
        {
            State = Stream.ReadBoolean();
        }

        public override int GetMessageType()
        {
            return 14778;
        }

        public override int GetServiceNodeType()
        {
            return 8; // я хз вроде 8 или 9
        }
    }
}
