using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace DAO
{
    public class TeamDAO
    {
        public List<Team> GetAll() => MockDatabase.Teams;

        public Team? GetById(long id) => MockDatabase.Teams.FirstOrDefault(t => t.Id == id);

        public Team Create(Team team)
        {
            team.Id = MockDatabase.Teams.Any() ? MockDatabase.Teams.Max(t => t.Id) + 1 : 1;
            MockDatabase.Teams.Add(team);
            return team;
        }

        public bool Update(long id, Team updatedTeam)
        {
            var team = GetById(id);
            if (team == null) return false;

            team.Name = updatedTeam.Name;
            team.Category = updatedTeam.Category;
            return true;
        }

        public bool Delete(long id)
        {
            var team = GetById(id);
            if (team == null) return false;
            return MockDatabase.Teams.Remove(team);
        }
    }
}
