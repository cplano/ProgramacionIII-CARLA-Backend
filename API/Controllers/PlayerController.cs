using Microsoft.AspNetCore.Mvc; // Para ControllerBase, [ApiController], [HttpGet], Ok(), NotFound(), etc.
using DAO;                      // Para usar PlayerDAO
using Entities;                 // Para usar la clase Player

namespace API.Controllers
{
    // CAPA API - CONTROLLER
    // Recibe los pedidos HTTP (desde Swagger, Postman o un frontend), le pide los
    // datos al DAO y devuelve la respuesta con el codigo de estado HTTP correcto.
    // El Controller NO accede a los datos directamente: siempre pasa por el DAO.

    // [Route]: define la URL base. [controller] se reemplaza por el nombre de la clase
    // sin la palabra "Controller" -> PlayerController = /api/Player
    [Route("api/[controller]")]
    // [ApiController]: activa comportamientos de API (por ejemplo, devuelve 400
    // automaticamente si el JSON que llega no se puede convertir a Player).
    [ApiController]
    public class PlayerController : ControllerBase
    {
        // "readonly" = solo se puede asignar en el constructor.
        // El "_" al principio es una convencion para campos privados.
        private readonly PlayerDAO _playerDAO;

        // INYECCION DE DEPENDENCIAS: el Controller pide un PlayerDAO en el constructor
        // y ASP.NET se lo pasa automaticamente (porque lo registramos con AddScoped en Program.cs).
        public PlayerController(PlayerDAO playerDAO)
        {
            _playerDAO = playerDAO;
        }

        // GET /api/Player -> devuelve la lista de todos los jugadores.
        // ActionResult<T> = puede devolver un objeto T o un codigo de estado HTTP.
        [HttpGet]
        public ActionResult<List<Player>> GetAll()
        {
            return Ok(_playerDAO.GetAll()); // 200 OK + la lista en formato JSON
        }

        // GET /api/Player/5 -> devuelve el jugador con Id 5.
        // "{id}" en la ruta se pasa como parametro al metodo.
        [HttpGet("{id}")]
        public ActionResult<Player> GetById(long id)
        {
            var player = _playerDAO.GetById(id);
            if (player == null) return NotFound(); // 404 Not Found: no existe
            return Ok(player); // 200 OK
        }

        // POST /api/Player -> crea un jugador nuevo.
        // [FromBody]: los datos vienen en el cuerpo del pedido, en formato JSON.
        [HttpPost]
        public ActionResult<Player> Create([FromBody] Player player)
        {
            // Validacion: el nombre es obligatorio.
            if (string.IsNullOrEmpty(player.Name))
            {
                return BadRequest("El nombre del jugador es requerido."); // 400 Bad Request: datos invalidos
            }

            var createdPlayer = _playerDAO.Create(player);
            // 201 Created: se creo un recurso nuevo. Se indica su URL y se devuelve el objeto con su Id.
            return Created($"/api/player/{createdPlayer.Id}", createdPlayer);
        }

        // PUT /api/Player/5 -> modifica el jugador con Id 5 con los datos del JSON.
        // IActionResult = solo devuelve un codigo de estado (con o sin datos).
        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Player player)
        {
            bool updated = _playerDAO.Update(id, player);
            if (!updated) return NotFound(); // 404 Not Found

            return Ok(player); // 200 OK
        }

        // DELETE /api/Player/5 -> borra el jugador con Id 5.
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            bool deleted = _playerDAO.Delete(id);
            if (!deleted) return NotFound(); // 404 Not Found

            return NoContent(); // 204 No Content: salio bien y no hay nada que devolver
        }
    }
}
