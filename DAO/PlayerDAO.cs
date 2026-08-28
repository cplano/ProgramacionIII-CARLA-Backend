using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities; // Se agrega este using

namespace DAO
{
    public class PlayerDAO
    {
        public List<Player> GetAll()
        {
            return MockDatabase.Players;
        }

        public Player? GetById(long id)
        {
            return MockDatabase.Players.FirstOrDefault(p => p.Id == id);
        }

        public Player Create(Player player)
        {
            player.Id = MockDatabase.Players.Any() ? MockDatabase.Players.Max(p => p.Id) + 1 : 1;
            MockDatabase.Players.Add(player);
            return player;
        }

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

        public bool Delete(long id)
        {
            var player = GetById(id);
            if (player == null) return false;
            return MockDatabase.Players.Remove(player);
        }

    }
}
