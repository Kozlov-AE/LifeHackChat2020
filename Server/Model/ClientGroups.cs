using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Model
{
    public enum ClientGroups
    {
        /// <summary>Обычные пользователи.</summary>
        user,
        /// <summary>Администраторы. Имеет расширенный функционал.</summary>
        admin
    }
}
