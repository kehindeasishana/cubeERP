using Core.Domain.Purchases;
using System;
using System.Collections.Generic;

namespace Services.Purchasing
{
    public partial interface IPurchasingService
    {
        void AddPurchaseInvoice(PurchaseInvoiceHeader purchaseIvoice, int? purchaseOrderId);
        void UpdatePurchaseOrder(PurchaseOrderHeader purchaseOrder);
        //PurchaseOrderHeader GetPurchaseOrderId(int id);
        //IEnumerable<PurchaseOrderHeader> GetPurchaseOrders();
        //void AddPurchaseOrder(PurchaseOrderHeader purchaseOrder, bool toSave);
        void DeletePurchaseOrder(PurchaseOrderHeader purchaseOrder);
        void AddPurchaseOrder(PurchaseOrderHeader purchaseOrder);
        void AddPurchaseOrderReceipt(PurchaseReceiptHeader purchaseOrderReceipt);
        IEnumerable<Vendor> GetVendors();
        Vendor GetVendorById(int id);
        IEnumerable<PurchaseOrderHeader> GetPurchaseOrders();
        PurchaseOrderHeader GetPurchaseOrderById(int id);
        PurchaseReceiptHeader GetPurchaseReceiptById(int id);
        void AddVendor(Vendor vendor);
        void UpdateVendor(Vendor vendor);
        void DeleteVendor(Vendor vendor);
        IEnumerable<PurchaseInvoiceHeader> GetPurchaseInvoices();
        PurchaseInvoiceHeader GetPurchaseInvoiceById(int id);
        void SavePayment(int invoiceId, int vendorId, int accountId, decimal amount, DateTime date);
    }
}
