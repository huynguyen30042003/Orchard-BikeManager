namespace BikeManagerV3.Suppliers.DTOs.PurchaseOrder
{
    public class ReceivePurchaseOrderRequest
    {
        public List<ReceivedSerialRequest> Serials { get; set; }
            = [];
    }
}
