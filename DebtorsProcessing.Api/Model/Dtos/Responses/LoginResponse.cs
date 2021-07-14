using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtorsProcessing.Api.Model.Dtos.Responses
{
    /// <summary>
    /// Ответ на запрос авторизаци со стороны пользователя.
    /// </summary>
    public class LoginResponse
    {
        /// <summary>
        /// Булева переменная, отображающая "успешность" выполнения операции.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Список ошибок, возникших в ходе выполнения операции.
        /// </summary>
        public List<string> Errors { get; set; }

        /// <summary>
        /// Токен авторизации, который затем должен будет использоваться при работе с основных функционалом системы.
        /// </summary>
        public string AuthToken { get; set; }

        /// <summary>
        /// Токен обновления, который позволит обновить токен авторизации после истечения срока его действия.
        /// </summary>
        public string RefreshToken { get; set; }
    }
}
