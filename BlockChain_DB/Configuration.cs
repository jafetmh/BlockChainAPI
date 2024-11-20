using System;

namespace BlockChain_DB
{
    internal class Configuration
    {
        public int Id { get; set; } // Identificador único
        public string Key { get; set; } // Clave única para la configuración
        public string Value { get; set; } // Valor de la configuración
    }
}
