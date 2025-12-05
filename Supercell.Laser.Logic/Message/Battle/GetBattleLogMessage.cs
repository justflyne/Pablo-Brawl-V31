using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supercell.Laser.Logic.Message.Club
{
    public class GetBattleLogMessage : GameMessage
    {
        public override int GetMessageType()
        {
            return 14114;
        }

        public override int GetServiceNodeType()
        {
            return 11;
        }
    }
}