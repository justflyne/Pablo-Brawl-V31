namespace Supercell.Laser.Logic.Notification
{
    public class FloaterTextNotification : BaseNotification
    {
        public override int GetNotificationType()
        {
            return 66;
        }
        public FloaterTextNotification(string text)
        {
            Text = text;
        }
    }
}
