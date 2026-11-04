using Entities;

namespace DAO
{
    public class TeamDAO
    {
        // READ: todos los equipos.
        public List<Team> GetAll() => MockDatabase.Teams;

        // READ: un equipo por Id (null si no existe).
        public Team? GetById(long id) => MockDatabase.Teams.FirstOrDefault(t => t.Id == id);

        // CREATE: asigna Id (mayor + 1) y lo agrega.
        public Team Create(Team team)
        {
            team.Id = MockDatabase.Teams.Any() ? MockDatabase.Teams.Max(t => t.Id) + 1 : 1;
            MockDatabase.Teams.Add(team);
            return team;
        }

        // UPDATE: true si lo encontro y modifico, false si no existe.
        public bool Update(long id, Team updatedTeam)
        {
            var team = GetById(id);
            if (team == null) return false;

            team.Name = updatedTeam.Name;
            team.Category = updatedTeam.Category;
            return true;
        }

        // DELETE: true si lo borro, false si no existe.
        public bool Delete(long id)
        {
            var team = GetById(id);
            if (team == null) return false;
            return MockDatabase.Teams.Remove(team);
        }
    }
}
