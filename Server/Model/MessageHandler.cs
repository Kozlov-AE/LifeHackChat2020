using Server.ClientCommandHandler;
using Server.DialogHandler;
using Server.Parameters;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace Server.Model
{
    public class MessageHandler
    {
        IDialogHandler dh;
        CommandHandler ch;
        LifeChatServer server;

        public MessageHandler(IDialogHandler dh, CommandHandler ch, LifeChatServer server)
        {
            this.dh = dh;
            dh.LoadData().Wait();
            this.ch = ch;
            this.server = server;
        }

        public void ProcessMessage(ClientMessageHandler? handler)
        {
            void Exc(string str)
            {
                server.SendMessageToClient(str, handler.Client.Id);
            }

            ch.Exception += Exc;
            var words = handler?.Message?.Split(new char[] { ' ', '.', '?', '!' }, StringSplitOptions.RemoveEmptyEntries);
            if (words != null && words[0][0] == '/')
            {
                var args = new object[words.Length];
                if (args.Length == 1) 
                {
                    args[0] = handler;
                }
                else
                {
                    for (int i = 1; i < words.Length; i++)
                    {
                        args[i] = words[i];
                    }
                }
                ch.ExecuteCommand(this, words[0], handler.Client.Group, args);
            }
            else
            {
                string insertMessage = "\n Введите сообщение";
                string noAnswer = "Я не знаю ответ, давайте попробуем еще раз.";

                if (dh.GetAnswer(handler.Message) is string answer)
                {
                    answer.Concat(insertMessage);
                }
                else
                {
                    answer = noAnswer;
                }
                server.SendMessageToClient(answer, handler.Client.Id);
            }
            ch.Exception -= Exc;
        }

        private string ListToString(IEnumerable<string> list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var s in list)
            {
                sb.Append($"{s} \n");
            }
            return sb.ToString();
        }
        #region AppCommands
        [CommandGroup(ClientGroups.admin)]
        [CommandGroup(ClientGroups.user)]
        [CommandAttribute("/KillMySelf", "Отклчиться от сервера")]
        public void CloseUserConnection(ClientMessageHandler? handler)
        {
            server.CloseUserConnection(handler.Client.Id);
        }

        [CommandGroup(ClientGroups.admin)]
        [CommandAttribute("/KillClientByName", "Отключить клиента от сервера. Формат команды: /KillClientByName ClientName")]
        public void CloseUserConnectionByName(ClientMessageHandler? handler)
        {
            server.CloseUserConnectionByName(handler.Client.UserName);
        }

        [CommandGroup(ClientGroups.admin)]
        [CommandAttribute("/GetAllClients", "Получить список подключенных клиентов")]
        public void GetClientsNamesList(ClientMessageHandler? handler)
        {
            server.SendMessageToClient(ListToString(server.GetClientsNamesList()), handler.Client.Id);
        }

        [CommandGroup(ClientGroups.admin)]
        [CommandGroup(ClientGroups.user)]
        [CommandAttribute("/GetQuestions", "Получить список вопросов, на которые сервер знает ответы")]
        public void GetQuestionList(ClientMessageHandler? handler)
        {
            server.SendMessageToClient(ListToString(dh.AllQuestions()), handler.Client.Id);
        }

        [CommandGroup(ClientGroups.admin)]
        [CommandGroup(ClientGroups.user)]
        [CommandAttribute("/Help", "Выводит Выводит список всех команд с описаниями")]
        public void GetHelp(ClientMessageHandler? handler)
        {
            var comms = ch.GetCommands(handler.Client.Group);
            StringBuilder sb = new StringBuilder();
            foreach (var c in comms)
            {
                sb.Append($"{c.Key} - {c.Value} \n");
            }
            server.SendMessageToClient(sb.ToString(), handler.Client.Id);
        }

        [CommandAttribute("/РусскийТест", "Просто тестовый метод, который вызывается русскими символами и не обозначен атрибутами пользователя")]
        public void WithoutGrouAttributeCommand(ClientMessageHandler? handler)
        {
            server.SendMessageToClient("Тестовый метод, в атрибутах не указаны права запуска \n" +
                "Значит виден любому пользователю)", handler.Client.Id);
        }

        #endregion

    }
}
