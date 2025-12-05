namespace Supercell.Laser.Logic.Command.Home
{
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Titan.DataStream;

    public class LogicSetPlayerAgeCommand : Command
    {
        public int Unknown;
        public int Unknown1;
        public int Age;
        
        public override void Decode(ByteStream stream)
        {
            base.Decode(stream);
            stream.ReadVInt();
            
            Unknown = stream.ReadVInt();
            Unknown1 = stream.ReadVInt();
            Age = ProcessValue(stream.ReadVInt());
            
            //Console.WriteLine($"Возраст: {Test}");
        }

        private int ProcessValue(int value)
        {
            if (value >= -64 && value <= -29)
            {
                return value + 128; 
            }
            else if (value >= 1 && value <= 63)
            {
                return value;
            }
            else
            {
                return value;
            }
        }

        public override int Execute(HomeMode homeMode)
        {
            return 0;
        }

        public override int GetCommandType()
        {
            return 530;
        }
    }
}