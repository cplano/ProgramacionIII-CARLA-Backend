namespace Entities
{
    // Representa un equipo.
    public class Team
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty; // Ej: "Primera A"

        // Relacion UNO A MUCHOS: un equipo tiene muchos jugadores.
        // En la base no es una columna: se arma con la columna TeamId de la tabla Players.
        // "= new List<Player>()" la inicializa vacia para que no sea null.
        public List<Player> Players { get; set; } = new List<Player>();
    }
}
