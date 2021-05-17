using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtorsDbModel.Model
{
    /// <summary>
    /// Представляет собой объект модели безопасности системы.
    /// </summary>
    public class SecurityObject
    {
        /// <summary>
        /// Номер записи в базе данных.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Название данного объекта доступа.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Список всех ролей, имеющих доступ к данному объекту.
        /// </summary>
        public ICollection<RoleObjectAccess> RoleObjectAccesses = new List<RoleObjectAccess>();

        /// <summary>
        /// Словарь для первичной инициализации ролей в системе.
        /// </summary>
        public static readonly Dictionary<string, Guid> ObjectNameToIdTranslator = new()
        {
            { "Доступ к панели администрирования", Guid.Parse("27d4e80b-694e-4085-a117-7b4736defd25") },
            { "Доступ на изменения данных должников, по которым сотрудник не является ответственным", Guid.Parse("c61a83f4-7c54-40c4-87ec-eabf5ff63ccb") },
            { "Добавление или удаление должников", Guid.Parse("e9050402-0d96-4e1a-b4e9-893d6a12a8dd") },
            { "Просмотр не своих должников", Guid.Parse("0b3b2724-c948-4d7d-9b7f-325eb5789a3a") },
            { "Перезакрепление должников на между сотрудниками", Guid.Parse("6b00e2dd-59d2-4eb2-bb7a-9712c361c062") },
            { "Может изменять свой пароль", Guid.Parse("1e6a869c-8c74-413b-b591-637254512853") }

        };

    }
}
