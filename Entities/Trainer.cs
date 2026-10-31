namespace Entities
{
    // Trainer (entrenador) hereda de Person.
    // Id, Name, Age y Dni se heredan automaticamente; aca solo va lo propio del entrenador.
    public class Trainer : Person
    {
        // Identificador del equipo que dirige (clave foranea hacia Team).
        // "long?" = puede ser null, es decir, un entrenador puede no tener equipo asignado.
        public long? TeamId { get; set; }
    }
}
