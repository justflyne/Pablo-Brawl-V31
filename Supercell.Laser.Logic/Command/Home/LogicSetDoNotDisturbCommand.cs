namespace Supercell.Laser.Logic.Command.Home
{
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Logic.Home.Gatcha;
    using Supercell.Laser.Titan.DataStream;

    public class LogicSetDoNotDisturbCommand : Command
    {
        public bool DoNotDistrub;

        public override void Encode(ByteStream stream)
        {

            stream.WriteBoolean(DoNotDistrub);
            stream.WriteVInt(0);
            base.Encode(stream);

        }

        public override int Execute(HomeMode homeMode)
        {
            homeMode.Avatar.DoNotDisturb = DoNotDistrub;
            return 0;
        }

        public override int GetCommandType()
        {
            return 213;
        }
    }
}
