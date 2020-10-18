using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Server.DialogHandler
{
    public interface IDialogHandler
    {
        /// <summary>Получить ответ от сервера, или <see langword="null"/> если ответ или вопрос не найден</summary>
        /// <param name="message">Сообщение от пользователя</param>
        public string? GetAnswer(string message);

        /// <summary>Загрузка данных для диалога</summary>
        public Task LoadData();

        /// <summary>Получить список всех известных вопросов</summary>
        public IReadOnlyCollection<string> AllQuestions();
    }
}
