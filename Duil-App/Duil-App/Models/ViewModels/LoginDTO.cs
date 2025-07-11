﻿namespace Duil_App.Models.ViewModels
{

    /// <summary>
    /// dados da pessoa para gerar uma autenticação
    /// </summary>
    public class LoginDTO
    {

        /// <summary>
        /// 'username' da pessoa que se quer autenticar
        /// </summary>
        public string Username { get; set; } = "";

        /// <summary>
        /// Password da pessoa que se quer autenticar
        /// </summary>
        public string Password { get; set; } = "";
    }

    public class LoginModel
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = "";

    }
}