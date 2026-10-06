namespace API.Helpers
{
    // Encapsula el uso de BCrypt (paquete BCrypt.Net-Next).
    // HASH: funcion que transforma la contraseña en un texto de largo fijo.
    //  - Unidireccional: no se puede volver del hash a la contraseña original.
    //  - Un cambio minimo en la contraseña genera un hash completamente distinto.
    // BCrypt ademas agrega un "salt" (dato aleatorio): la misma contraseña
    // genera un hash distinto cada vez, pero Verify igual sabe compararlos.
    public static class PasswordHasher
    {
        // Convierte "123456" en algo como "$2a$11$Zx9...". Esto es lo que se guarda en la base.
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Devuelve true si la contraseña escrita corresponde al hash guardado.
        public static bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
