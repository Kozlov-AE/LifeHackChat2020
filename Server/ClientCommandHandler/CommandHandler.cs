using Server.Model;
using Server.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Server.ClientCommandHandler
{
    public class CommandHandler
    {
        public event Action<string> Exception;

        LifeChatServer? server;
        Dictionary<string?, MethodInfo> commands;

        public CommandHandler(LifeChatServer? server)
        {
            this.server = server;
            GetCommandList();
        }

        public void GetCommandList()
        {
            commands = typeof(MessageHandler)
                .GetMethods()
                .Where(m => m.GetCustomAttribute<CommandAttribute>() != null)
                .ToDictionary(m => m.GetCustomAttribute<CommandAttribute>()!.Name);
            //commands = Assembly.GetExecutingAssembly()
            //    .GetTypes()
            //    .SelectMany(t => t.GetMethods())
            //    .Where(m => m.GetCustomAttribute<CommandAttribute>() != null)
            //    .ToDictionary(m => m.GetCustomAttribute<CommandAttribute>()!.Name);
        }

        public void ExecuteCommand(object obj, string command, ClientGroups group, object[] par = null)
        {
            MethodInfo? method;
            if (!commands.TryGetValue(command, out method))
            {
                Exception?.Invoke("Введенная команда не найдена");
                return;
            }

            if (method.GetCustomAttributes<CommandGroupAttribute>().Any())
            {
                if (method.GetCustomAttributes<CommandGroupAttribute>().FirstOrDefault(m => m.Group == group) == null)
                {
                    Exception.Invoke("Нет прав на выполнение команды");
                    return;
                }
            }
            try
            {
                method.Invoke(obj, par);
            }
            catch (Exception ex)
            {
                Exception.Invoke(ex.Message);
            }
        }
        
        public IReadOnlyDictionary<string, string> GetCommands(ClientGroups group)
        {
            Dictionary<string, string?> result = new Dictionary<string, string>();
            foreach (var c in commands.Values)
            {
                if (c.GetCustomAttributes<CommandGroupAttribute>() is IEnumerable<CommandGroupAttribute> atrs
                    && EqualAttribute(atrs, group))
                {

                    result.Add(
                        c.GetCustomAttribute<CommandAttribute>().Name,
                        c.GetCustomAttribute<CommandAttribute>()?.Description);
                }
                if (!c.GetCustomAttributes<CommandGroupAttribute>().Any()) 
                    result.Add(c.GetCustomAttribute<CommandAttribute>().Name,
                    c.GetCustomAttribute<CommandAttribute>()?.Description);
            }
            return result;
        }

        private bool EqualAttribute(IEnumerable<CommandGroupAttribute> attrs, ClientGroups group)
        {
            foreach (var a in attrs)
            {
                if (a.Group == group) return true;
            }
            return false;
        }
    }
}
