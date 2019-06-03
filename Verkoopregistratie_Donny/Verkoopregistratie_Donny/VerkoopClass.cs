using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verkoopregistratie_Donny
{
    class VerkoopClass
    {
        public int Id { get; set; }
        public string Product { get; set; }
        public string Klant { get; set; }
        public Decimal Prijs { get; set; }
        public DateTime Datum { get; set; }
    }
}
