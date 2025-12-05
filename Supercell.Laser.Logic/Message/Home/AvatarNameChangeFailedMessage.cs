namespace Supercell.Laser.Logic.Message.Home;

public class AvatarNameChangeFailedMessage : GameMessage
{
    private int Reason;

    public void SetReason(int reason)
    {
        Reason = reason;
    }

    public override void Encode()
    {
        Stream.WriteInt(Reason);
    }

    public override int GetMessageType()
    {
        return 20205;
    }
    
    public override int GetServiceNodeType()
    {
        return 9;
    }
}