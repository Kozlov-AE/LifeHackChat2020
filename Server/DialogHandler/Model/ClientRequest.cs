using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace Server.DialogHandler.Model
{
    /// <summary>Вопрос пользователя</summary>
    public class ClientRequest
    {
        /// <summary>Статик int для формирования Id</summary>
        static int globalId;
        static ClientRequest()
        {
            globalId = 0;
        }
        public ClientRequest( )
        {
        }

        public ClientRequest(string text)
        {
            Text = text;
            answers = new List<ServerAnswer>();
            Id = NexId();
        }

        public ClientRequest(string? text, List<ServerAnswer> answers)
        {
            Text = text;
            this.answers = answers;
            Id = NexId();
        }

        /// <summary>Id запроса</summary>
        public int Id { get; set; } 
        /// <summary>Текст запроса</summary>
        public string? Text { get; set; }
        /// <summary>Список ответов на запрос</summary>
        public List<ServerAnswer> answers { get; set; }

        /// <summary>Добавить ответ для этого вопроса</summary>
        /// <param name="answer">Текст ответа</param>
        public void AddAnswer(string answer)
        {
            answers.Add(new ServerAnswer(Id, answer));
        }

        /// <summary>Получить случайный ответ (если их несколько)</summary>
        /// <returns><see cref="ServerAnswer"/> или <see langword="null"/>, если ответов нет</returns>
        public ServerAnswer? GetRandomAnswer() 
            => answers.Count > 0 ? answers.ElementAt(new Random().Next(0, answers.Count)) : null;

        /// <summary>Следующий ID</summary>
        static int NexId()
        {
            return ++globalId;
        }

    }
}
