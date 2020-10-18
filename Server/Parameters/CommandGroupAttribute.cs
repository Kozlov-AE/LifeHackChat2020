using Server.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Parameters
{
    [AttributeUsage(System.AttributeTargets.Method, AllowMultiple = true)]
    public class CommandGroupAttribute : Attribute
    {
        public ClientGroups Group{ get; }

        public CommandGroupAttribute(ClientGroups group)
        {
            Group = group;
        }
    }
}
