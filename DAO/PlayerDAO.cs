using Entities; // Para usar la clase Player

namespace DAO
{
    // CAPA DAO (Data Access Object = Objeto de Acceso a Datos).
    // Es la UNICA capa que sabe donde y como se guardan los datos.
    // El Controller le pide cosas al DAO ("dame todos los jugadores") sin saber
    // si vienen de una lista en memoria o de MySQL. Asi, si cambia la base,
    // solo se modifica el DAO.
    //
    // Implementa el CRUD completo: Create, Read (GetAll / GetById), Update, Delete.
    public class PlayerDAO
    {
        // READ: devuelve todos los jugadores.
        public List<Player> GetAll()
        {
            return MockDatabase.Players;
        }

        // READ: busca un jugador por su Id.
        // "Player?" = puede devolver null si no lo encuentra.
        // FirstOrDefault (LINQ) recorre la lista y devuelve el primero que cumple
        // la condicion "p => p.Id == id" (se lee: "para cada p, que p.Id sea igual a id").
        // Si ninguno cumple, devuelve null.
        public Player? GetById(long id)
        {
            return MockDatabase.Players.FirstOrDefault(p => p.Id == id);
        }

        // CREATE: agrega un jugador nuevo y lo devuelve con su Id asignado.
        public Player Create(Player player)
        {
            // Genera el Id a mano (simula el AUTO_INCREMENT de una base de datos):
            // si la lista tiene elementos -> el Id mas alto + 1; si esta vacia -> 1.
            // "condicion ? valorSiTrue : valorSiFalse" es el operador ternario (un if en una linea).
            player.Id = MockDatabase.Players.Any() ? MockDatabase.Players.Max(p => p.Id) + 1 : 1;
            MockDatabase.Players.Add(player);
            return player;
        }

        // UPDATE: modifica el jugador con ese id usando los datos de updatedPlayer.
        // Devuelve true si lo modifico, false si no existia.
        public bool Update(long id, Player updatedPlayer)
        {
            var player = GetById(id);
            if (player == null) return false; // No existe -> el Controller responde 404

            // Copia campo por campo los nuevos valores (el Id NO se cambia).
            player.Name = updatedPlayer.Name;
            player.Age = updatedPlayer.Age;
            player.Dni = updatedPlayer.Dni;
            player.Numero = updatedPlayer.Numero;
            player.TeamId = updatedPlayer.TeamId;
            return true;
        }

        // DELETE: borra el jugador con ese id.
        // Devuelve true si lo borro, false si no existia.
        public bool Delete(long id)
        {
            var player = GetById(id);
            if (player == null) return false;
            return MockDatabase.Players.Remove(player); // Remove devuelve true si lo saco de la lista
        }
    }
}
