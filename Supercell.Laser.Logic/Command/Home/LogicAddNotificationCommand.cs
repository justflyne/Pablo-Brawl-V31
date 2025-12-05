namespace Supercell.Laser.Logic.Command.Home
{
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Logic.Home.Items;
    using Supercell.Laser.Logic.Notification;
    using Supercell.Laser.Titan.DataStream;

    public class LogicAddNotificationCommand : Command
    {
        public Notification Notification;
        public FloaterTextNotification FloaterTextNotification;

        public override void Encode(ByteStream stream)
        {
            if (stream.WriteBoolean(Notification != null))
            {
                //stream.WriteVInt(Notification.GetNotificationType());
                long timestamp = DateTime.Now.Ticks / TimeSpan.TicksPerSecond;
                int timestamps = Convert.ToInt32(timestamp % int.MaxValue);

                Notification.TimePassed = Convert.ToInt32(timestamps);
                Notification.Encode(stream);
            }

            stream.WriteVInt(0);
            base.Encode(stream);
        }

        public override int Execute(HomeMode homeMode)
        {
            return 0;
        }

        public override int GetCommandType()
        {
            return 206;
        }
    }
}
