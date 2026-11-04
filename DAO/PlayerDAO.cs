using Entities;

namespace DAO
{
    public class PlayerDAO
    {
        // READ: todos los jugadores.
        public List<Player> GetAll()
        {
            return MockDatabase.Players;
        }

        // READ: un jugador por Id (null si no existe).
        public Player? GetById(long id)
        {
            return MockDatabase.Players.FirstOrDefault(p => p.Id == id);
        }

        // CREATE: asigna Id (mayor + 1) y lo agrega.
        public Player Create(Player player)
        {
            player.Id = MockDatabase.Players.Any() ? MockDatabase.Players.Max(p => p.Id) + 1 : 1;
            MockDatabase.Players.Add(player);
            return player;
        }

        // UPDATE: true si lo encontro y modifico, false si no existe.
        public bool Update(long id, Player updatedPlayer)
        {
            var player = GetById(id);
            if (player == null) return false;

            player.Name = updatedPlayer.Name;
            player.Age = updatedPlayer.Age;
            player.Dni = updatedPlayer.Dni;
            player.Numero = updatedPlayer.Numero;
            player.TeamId = updatedPlayer.TeamId;
            return true;
        }

        // DELETE: true si lo borro, false si no existe.
        public bool Delete(long id)
        {
            var player = GetById(id);
            if (player == null) return false;
            return MockDatabase.Players.Remove(player);
        }
    }
}
