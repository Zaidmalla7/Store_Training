namespace ECApp.Services
{
    public class InventoryService
    {
        public static int GetInventoryStatus(int quantity, int minimumStock)
        {
            if (quantity == 0)
                return 7; // Out of Stock
            else if (quantity < minimumStock)
                return 6; // Low Stock
            else
                return 4; // Available
        }
    }
}
