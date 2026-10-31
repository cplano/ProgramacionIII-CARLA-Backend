using Entities; // Para usar la clase Activity

namespace DAO
{
    // DAO de actividades. Mismo patron CRUD que PlayerDAO (ver comentarios alli),
    // escrito con "=>" en los metodos de una sola linea (ver TeamDAO).
    public class ActivityDAO
    {
        // READ: todas las actividades.
        public List<Activity> GetAll() => MockDatabase.Activities;

        // READ: una actividad por Id (null si no existe).
        public Activity? GetById(long id) => MockDatabase.Activities.FirstOrDefault(a => a.Id == id);

        // CREATE: asigna Id (mayor + 1) y la agrega.
        public Activity Create(Activity activity)
        {
            activity.Id = MockDatabase.Activities.Any() ? MockDatabase.Activities.Max(a => a.Id) + 1 : 1;
            MockDatabase.Activities.Add(activity);
            return activity;
        }

        // UPDATE: true si la encontro y modifico, false si no existe.
        public bool Update(long id, Activity updatedActivity)
        {
            var activity = GetById(id);
            if (activity == null) return false;

            activity.Title = updatedActivity.Title;
            activity.Description = updatedActivity.Description;
            activity.Date = updatedActivity.Date;
            activity.Type = updatedActivity.Type;
            return true;
        }

        // DELETE: true si la borro, false si no existe.
        public bool Delete(long id)
        {
            var activity = GetById(id);
            if (activity == null) return false;
            return MockDatabase.Activities.Remove(activity);
        }
    }
}
