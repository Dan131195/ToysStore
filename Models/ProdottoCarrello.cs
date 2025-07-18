namespace ToysStore.Models
{
    public class ProdottoCarrello
    {
        public Guid ProdottoCarrelloId { get; set; }

        public Guid ProdottoId { get; set; }

        public Prodotto? Prodotto { get; set; }

        public int Quantity { get; set; }

        public Guid CartId { get; set; }

        public Carrello? Carrello { get; set; }
    }
}
