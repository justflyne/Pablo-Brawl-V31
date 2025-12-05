namespace Supercell.Laser.Logic.Home.Gatcha
{
    using Supercell.Laser.Titan.DataStream;

    public class ForcedDrops
    {

        public void Encode(ByteStream stream)
        {
            stream.WriteBoolean(false);
        }
    }
}
