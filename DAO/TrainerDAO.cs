using Entities; // Para usar la clase Trainer

namespace DAO
{
    // DAO de entrenadores. Mismo patron CRUD que PlayerDAO (ver comentarios alli),
    // escrito con "=>" en los metodos de una sola linea (ver TeamDAO).
    public class TrainerDAO
    {
        // READ: todos los entrenadores.
        public List<Trainer> GetAll() => MockDatabase.Trainers;

        // READ: un entrenador por Id (null si no existe).
        public Trainer? GetById(long id) => MockDatabase.Trainers.FirstOrDefault(t => t.Id == id);

        // CREATE: asigna Id (mayor + 1) y lo agrega.
        public Trainer Create(Trainer trainer)
        {
            trainer.Id = MockDatabase.Trainers.Any() ? MockDatabase.Trainers.Max(t => t.Id) + 1 : 1;
            MockDatabase.Trainers.Add(trainer);
            return trainer;
        }

        // UPDATE: true si lo encontro y modifico, false si no existe.
        public bool Update(long id, Trainer updatedTrainer)
        {
            var trainer = GetById(id);
            if (trainer == null) return false;

            trainer.Name = updatedTrainer.Name;
            trainer.Age = updatedTrainer.Age;
            trainer.Dni = updatedTrainer.Dni;
            trainer.TeamId = updatedTrainer.TeamId;
            return true;
        }

        // DELETE: true si lo borro, false si no existe.
        public bool Delete(long id)
        {
            var trainer = GetById(id);
            if (trainer == null) return false;
            return MockDatabase.Trainers.Remove(trainer);
        }
    }
}
