namespace Entities
{
    // Player HEREDA de Person (": Person"): ya tiene Id, Name, Age y Dni sin escribirlos.
    // Aca solo se agregan las propiedades propias de un jugador.
    public class Player : Person
    {
        public int Numero { get; set; } // Numero de camiseta

        // Identificador del equipo al que pertenece (clave foranea hacia Team).
        // "long?" = puede ser null, es decir, un jugador puede no tener equipo.
        public long? TeamId { get; set; }
    }
}
