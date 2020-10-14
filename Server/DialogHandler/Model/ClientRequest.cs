using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.DialogHandler.Model
{
    public class ClientRequest
    {
        static int globalId;
        static ClientRequest()
        {
            globalId = 1;
        }

        public int Id { get; private set; }
        public string? Text { get; set; }
        readonly List<ServerAnswer> answers = new List<ServerAnswer>();

        /// <summary>Добавить ответ для этого вопроса</summary>
        /// <param name="answer">Текст ответа</param>
        public void AddAnswer (string answer) 
            => answers.Add(new ServerAnswer(Id, answer));

        /// <summary>Получить случайный ответ (если их несколько)</summary>
        /// <returns><see cref="ServerAnswer"/> или <see langword="null"/>, если ответов нет</returns>
        public ServerAnswer? GetRandomAnswer() 
            => answers.Count > 0 ? answers.ElementAt(new Random().Next(1, answers.Count)) : null; 

    }
}
