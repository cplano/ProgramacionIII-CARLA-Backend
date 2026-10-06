namespace Entities
{
    // CAPA ENTITIES: contiene solo las clases que representan los datos del sistema.
    // No tiene logica ni acceso a la base: solo propiedades.

    // Person es la CLASE BASE (clase padre) de Player, Trainer y Student.
    // Junta lo que todas las personas tienen en comun, para no repetirlo en cada clase (herencia).
    public class Person
    {
        // { get; set; } = propiedad que se puede leer (get) y modificar (set).
        // "Id" por convencion es la clave primaria cuando EF crea la tabla.
        public long Id { get; set; }

        // "= string.Empty" le da un valor inicial vacio ("") para que nunca sea null.
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Dni { get; set; } = string.Empty;
    }
}
