namespace FGSZAMA.Models
{
    public class AktywnośćModel
    {
        public int Id { get; set; }
        public string NazwaUżytkownika { get; set; }
        public DateTime DataAktywności { get; set; }
        public string TypAktywności { get; set; }
        public string Opis { get; set; }
        public bool IsRead { get; set; }

    }
}
