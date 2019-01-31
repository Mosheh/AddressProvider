using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AddressProvider.Models
{
    public class Cidade_info
    {
        public Cidade_info()
        {
            area_km2 = "";
            codigo_ibge = "";
        }
        public string area_km2 { get; set; }
        public string codigo_ibge { get; set; }
    }
}
