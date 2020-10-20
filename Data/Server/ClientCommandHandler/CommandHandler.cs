using Logic.Server.Model;
using Logic.Server.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Logic.Server.ClientCommandHandler
{
    /// <summary>Класс обрабатывающий команды от клиента</summary>
    public class CommandHandler
    {
        /// <summary>Событие, возникающее при ошибке</summary>
        public event Action<string> Exception;

        /// <summary>Хранит пары Наименование (из аттрибута) - методИнфо</summary>
        Dictionary<string?, MethodInfo> commands;

        public CommandHandler()
        {
            GetCommandList();
        }

        /// <summary>Получить список команд из класса <see cref="MessageHandler"/> и сохранить его в словарь класа</summary>
        void GetCommandList()
        {
            commands = typeof(MessageHandler)
                .GetMethods()
                .Where(m => m.GetCustomAttribute<CommandAttribute>() != null)
                .ToDictionary(m => m.GetCustomAttribute<CommandAttribute>()!.Name);
        }

        /// <summary>Выполнение метода из словаря</summary>
        /// <param name="obj">Объект класса, хранящего метод</param>
        /// <param name="command">Имя класса, описанное в атрибуте</param>
        /// <param name="group">Группа пользователя, инициирующего запуск метода</param>
        /// <param name="par">Параметры необходимые для выполнения метода</param>
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
        
        /// <summary>Получить словарь с командами, доступными только определенной группе пользователей</summary>
        /// <param name="group">Группа пользователей для фильтрации словаря</param>
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

        /// <summary>Проверяет наличие группы в атрибутах метода</summary>
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
