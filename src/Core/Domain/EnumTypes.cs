namespace Core.Domain
{
    public enum AccountClasses
    {
        Assets = 1,
        Liabilities = 2,
        Equity = 3,
        Revenue = 4,
        Expense = 5
        
    }
    public enum Gender
    {
        Male = 1,
        Female = 2
    }
    public enum TaxOnFreight
    {
        Yes = 1,
        No = 2
    }
    public enum CalculateTax
    {
        SingleRate = 1,
        Formula = 2
    }
    public enum MaritalStatus
    {
        Single = 1,
        Married = 2,
        Divorced = 3,
        Separated = 4
    }
    public enum FileType
    {
        Avatar = 1, Photo
    }
    //public enum ExitType
    //{
    //    Resignation = 1,
    //    Termination = 2,
    //    Transfer = 3
    //}
    //public enum ChangeStatus
    //{
    //    Employee = 1,
    //    Intern = 2,
    //    Contract = 3,
    //    Permanent =  4,
    //    NYSC = 5
    //}
    public enum ItemClass
    {
        StockItem = 1,
        MasterStockItem = 2,
        SerializedStockItem = 3,
        Service = 4,
        Labour = 5,
        Assembly = 6,
        SerializedAssembly = 7,
        ActivityItem = 8,
        ChargeItem = 9,
        NonStockItem = 10,
        DescriptionOnly = 11
    }
    public enum Producttype
    {
        Consumable = 1,
        Service = 2,
        StockableProduct = 3
    }
    public enum ShipVia
    {
        Airbone = 1,
        Courier = 2,
        HandDeliver = 3,
        CustomerPickUp = 4,
        UPS = 5,
        //Temporary = 6
    }
    public enum DocumentTypes
    {
        SalesQuote = 1,
        SalesOrder,
        SalesDelivery,
        SalesInvoice,
        SalesReceipt,
        SalesDebitMemo,
        SalesCreditMemo,
        PurchaseOrder,
        PurchaseReceipt,
        PurchaseInvoice,
        PurchaseDebitMemo,
        PurchaseCreditMemo,
        PurchaseInvoicePayment,
        JournalEntry,
        CustomerAllocation
    }

    public enum AccountTypes
    {
        Posting = 1,
        Heading,
        Total,
        BeginTotal,
        EndTotal
    }

    public enum DrOrCrSide
    {
        //NA = 0,
        Dr = 1,
        Cr = 2
    }

    public enum PartyTypes
    {
        Customer = 1,
        Vendor = 2,
        Contact = 3
    }

    /// <summary>
    /// Journal voucher is prepared for the transactions which does not relate to sales, purchases, cash, bank, material returns
    /// </summary>
    public enum JournalVoucherTypes
    {
        OpeningBalances = 1,
        ClosingEntries = 2,
        AdjustmentEntries = 3,
        CorrectionEntries = 4,
        TransferEntries = 5,
    }
    public enum AccountCategory
    {
        TreasuryBills,
        PropertyAndEquipment,
        TaxPayable,
        FixedAssets,
        LiquidAssets

    }
    public enum PurchaseStatuses
    {
        Open,
        PartiallyReceived,
        FullReceived,
        Invoiced,
        Closed
    }

    public enum PurchaseInvoiceStatuses
    {
        Open,
        Paid
    }

    public enum SequenceNumberTypes
    {
        SalesQuote = 1,
        SalesOrder,
        SalesDelivery,
        SalesInvoice,
        SalesReceipt,
        PurchaseOrder,
        PurchaseReceipt,
        PurchaseInvoice,
        VendorPayment,
        JournalEntry,
        Item,
        Customer,
        Vendor,
        Contact
    }

    public enum AddressTypes
    {
        Office,
        Home
    }

    public enum ContactTypes
    {
        Customer = 1,
        Vendor = 2,
        Company = 3
    }

    public enum ItemTypes
    {
        Manufactured = 1,
        Purchased,
        Service,
        Charge
    }

    public enum PaymentTypes
    {
        Prepayment = 1,
        Cash,
        AfterNoOfDays,
        DayInTheFollowingMonth
    }

    public enum BankTypes
    {
        CheckingAccount = 1,
        SavingsAccount,
        CashAccount
    }

    public enum SalesInvoiceStatus
    {
        //Draft,
        Open,
        Overdue,
        Closed,
        Void
    }
}
