﻿using Server.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Parameters
{
    /// <summary>Атрибут разрешающий выполнение команды определенной группе клиентов</summary>
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
