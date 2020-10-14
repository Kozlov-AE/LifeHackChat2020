using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.DialogHandler.Model
{
    class DialogStorage : IDialogStorage
    {
        readonly List<ClientRequest> requests = new List<ClientRequest>();

        /// <summary>Добавить запрос пользователя в базу</summary>
        /// <param name="request">Запрос</param>
        public void AddRequest(ClientRequest request)
        {
            requests.Add(request);
        }

        /// <summary>Удалить запрос пользователя из базы</summary>
        /// <param name="id">Id запроса</param>
        public void RemoveRequest(int id)
        {
            var req = this[id];
            if (req != null) requests.Remove(req);
        }

        /// <summary>Получить объект по Id</summary>
        /// <param name="id">Id объекта</param>
        /// <returns><see cref="ClientRequest"/> или <see langword="null"/> если не id найден</returns>
        public ClientRequest this[int id] => requests.FirstOrDefault(r => r.Id == id);

        /// <summary>Получить коллекцию всех вопросов</summary>
        /// <returns><see cref="IReadOnlyCollection<ClientRequest>"/> Вопросов</returns>
        public IReadOnlyCollection<ClientRequest> GetAllRequests() => requests;
    }
}
