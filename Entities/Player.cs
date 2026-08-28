using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Player : Person
    {
        public int Numero { get; set; }
        public long? TeamId { get; set; } // Identificador del equipo al que pertenece

    }
}
