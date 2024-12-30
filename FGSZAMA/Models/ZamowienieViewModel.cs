using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FGSZAMA.Models
{
    public class ZamowienieViewModel
    {
        public int Id { get; set; }

        [Required]
        public string WybranyZestaw { get; set; }

        [Required]
        public int WybranaKalorycznosc { get; set; }

        [Required]
        public DateTime DataOd { get; set; }

        [Required]
        public DateTime DataDo { get; set; }

        public decimal PodsumowanaCena { get; set; }

        public List<string> DostepneZestawy { get; set; } = new List<string> { "Zestaw Turbo", "Zestaw Pomidor", "Zestaw Drwala", "Zestaw FitBox","Zestaw Ognisty","Zestaw Komfort","Zestaw Leśny" };

        
        public List<KalorycznoscOpcja> OpcjeKalorycznosci { get; set; } = new List<KalorycznoscOpcja>
            {
                new KalorycznoscOpcja { Kalorycznosc = 1500, Cena = 45 },
                new KalorycznoscOpcja { Kalorycznosc = 1800, Cena = 55 },
                new KalorycznoscOpcja { Kalorycznosc = 2100, Cena = 65 },
                new KalorycznoscOpcja { Kalorycznosc = 2400, Cena = 70 },
                new KalorycznoscOpcja { Kalorycznosc = 2800, Cena = 80 }
            };
    }
    public class KalorycznoscOpcja
    {
        public int Kalorycznosc { get; set; }
        public decimal Cena { get; set; }
    }
}
