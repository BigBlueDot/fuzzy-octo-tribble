using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDataClasses
{
    public class ClientMessage
    {
        public ClientMessageType type;
        public string message;

        public enum ClientMessageType
        {
            Message,
            RefreshMap
        }
    }
}
