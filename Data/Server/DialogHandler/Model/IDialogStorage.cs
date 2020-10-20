using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logic.Server.DialogHandler.Model
{
    public interface IDialogStorage
    {
        /// <summary>Добавить запрос пользователя в базу</summary>
        /// <param name="request">Запрос</param>
        public void AddRequest(ClientRequest request);

        /// <summary>Удалить запрос пользователя из базы</summary>
        /// <param name="id">Id запроса</param>
        public void RemoveRequest(int id);

        /// <summary>Получить объект по Id</summary>
        /// <param name="id">Id объекта</param>
        /// <returns><see cref="ClientRequest"/> или <see langword="null"/> если не id найден</returns>
        public ClientRequest this[int id] { get; }

        /// <summary>Получить коллекцию всех вопросов</summary>
        /// <returns><see cref="IReadOnlyCollection<ClientRequest>"/> Вопросов</returns>
        public IReadOnlyCollection<ClientRequest> GetAllRequests();

        /// <summary>Зафиксировать содержимое хранилища</summary>
        public void SaveBase();

        /// <summary>Загрузить базу из хранилища</summary>
        public Task LoadBase();
    }
}