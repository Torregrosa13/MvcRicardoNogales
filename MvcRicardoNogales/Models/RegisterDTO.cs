﻿namespace MvcRicardoNogales.Models
{
    public class RegisterDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Rol { get; set; } = "User";
    }
}
