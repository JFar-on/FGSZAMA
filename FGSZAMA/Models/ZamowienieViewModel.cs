using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FGSZAMA.Models
{
    public class ZamowienieViewModel
    {

        public int Id { get; set; }
        // Lista zestawów
        public List<string> DostepneZestawy => new List<string> { "Turbo", "Pomidor", "Drwala" };

        // Lista kaloryczności i ceny
        public List<KalorycznoscOpcja> OpcjeKalorycznosci => new List<KalorycznoscOpcja>
        {
        new KalorycznoscOpcja { Kalorycznosc = 1800, Cena = 40 },
        new KalorycznoscOpcja { Kalorycznosc = 2200, Cena = 50 },
        new KalorycznoscOpcja { Kalorycznosc = 2800, Cena = 60 },
        

        };

        
        public string WybranyZestaw { get; set; } = string.Empty;
        
        public int WybranaKalorycznosc { get; set; } 
        
        public DateTime DataOd { get; set; }
        
        public DateTime DataDo { get; set; }
        
        public decimal PodsumowanaCena { get; set; }

    }
    public class KalorycznoscOpcja
    {
        [Key]
        public int Id { get; set; }
        
        public int Kalorycznosc { get; set; }
        
        public decimal Cena { get; set; }
    }
}
