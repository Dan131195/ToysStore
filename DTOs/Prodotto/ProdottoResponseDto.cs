namespace ToysStore.DTOs.Prodotto
{
    public class ProdottoResponseDto
    {
        public Guid GiocattoloId { get; set; }
        public string NomeGiocattolo { get; set; }
        public string DescrizioneGiocattolo { get; set; }
        public decimal PrezzoGiocattolo { get; set; }

        public string CondizioneNome { get; set; }
        public string CategoriaNome { get; set; }

        public string VenditoreId { get; set; }
        public List<string> UrlsImmagini { get; set; } = new List<string>();
    }
}
