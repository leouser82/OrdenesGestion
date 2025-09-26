namespace GestionOrdenes.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; // En producción debería estar hasheada
        public string Role { get; set; } = "User";
    }
}