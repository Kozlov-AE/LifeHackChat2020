using Logic.Server.Model;
using System;
using System.Collections.Generic;

namespace Logic.Server.Mediator
{
    public interface IConnectionStorage
    {
        /// <summary>Добавить подключение</summary>
        /// <param name="clientModel">экземпляр подключения</param>
        public void AddConnection(ClientModel clientModel);

        /// <summary>Удалить подключение по id</summary>
        /// <param name="id">Id подключения</param>
        public void RemoveConnection(string id);

        /// <summary>Получить все id кроме указанного</summary>
        /// <param name="id">id который не нужно получать</param>
        public IReadOnlyCollection<ClientModel> GetAllExceptId(string id);

        /// <summary>Получить все <see cref="ClientModel"/> из хранилища</summary>
        public IReadOnlyCollection<ClientModel> GetAllClients();

        /// <summary>Заурыть и удалить все соединения</summary>
        public void CloseAndRemoveAll();

        /// <summary>Получить соединение по id</summary>
        public ClientModel this[string id] { get; }

        /// <summary>Получить первый найденный элемент или <see langword="null"/> по условию</summary>
        /// <param name="predicate">Условная конструкция для поиска</param>
        public ClientModel FirstOrDefault(Func<ClientModel, bool> predicate);
    }
}