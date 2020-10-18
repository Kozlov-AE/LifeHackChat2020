using Server.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Parameters
{
    [System.AttributeUsage(System.AttributeTargets.Method)]
    public class CommandAttribute : Attribute
    {
        public string Name { get; }
        public string? Description { get; }

        public CommandAttribute(string name)
        {
            Name = name;
            Description = null;
        }

        public CommandAttribute(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
