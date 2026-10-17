namespace ToysStore.DTOs.Prodotto
{
    public class ProdottoUpdateDto
    {
        public string NomeGiocattolo { get; set; }
        public string DescrizioneGiocattolo { get; set; }
        public decimal PrezzoGiocattolo { get; set; }

        public int CategoriaId { get; set; }
        public int CondizioneId { get; set; }

    }
}
