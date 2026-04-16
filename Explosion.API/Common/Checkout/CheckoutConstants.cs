namespace Explosion.API.Common
{
    public static class CheckoutStatus
    {
        public const string Success = "success";
        public const string Failed = "failed";
    }

    public static class CheckoutMessage
    {
        public const string EmptyCart = "Carrinho vazio";
        public const string InsufficientStockPrefix = "Estoque insuficiente";
        public const string CheckoutSuccess = "Checkout realizado com sucesso";
        public const string CheckoutInternalError = "Erro interno no checkout";
    }
}
