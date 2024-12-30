using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace FGSZAMA.Models
{
    public class ZamowienieModel
    {
        [Key]
        public int Id { get; set; }

        public string Zestaw { get; set; }  = string.Empty;// Turbo, Pomidor, Drwala
        
        public int Kalorycznosc { get; set; } // 1800, 2200, 2800
        
        public decimal CenaZaDzien { get; set; }
        
        public DateTime DataOd { get; set; }
        
        public DateTime DataDo { get; set; }
        
        public decimal SumaCeny { get; set; } // Podliczona cena


    }
}
