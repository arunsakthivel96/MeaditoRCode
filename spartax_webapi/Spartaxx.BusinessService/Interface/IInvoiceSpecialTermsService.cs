using Spartaxx.BusinessObjects;
using Spartaxx.BusinessObjects.Invoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.BusinessService.Interface
{
    //added by saravanans-tfs:47247
   public interface IInvoiceSpecialTermsService
    {
        List<PTXboSpecialTermClients> LoadSpecialTermClients(PTXboSpecialTermClientsFilter specialTermClientFilter);
        List<PTXboSpecialTermClients> LoadSpecialTermOutofTexasClients(PTXboSpecialTermClientsFilter specialTermClientFilter);
        List<PTXboSpecialTermClients> LoadSpecialTermsDonotGenerateInvoices(PTXboSpecialTermClientsFilter objFilter);
        bool UpdateInvoiceDetailsReport(PTXboInvoiceReport objInvoice);
        bool UpdateInvoiceProcessStatus(PTXboUpdateInvoiceProcessingStatusInput invoiceProcessingStatus);
        PTXboSpecialTermInvoiceDetails SpecialTermInvoicesGeneration(PTXboSpecialTermInput specialInput);

        PTXboSpecialTermInvoiceDetails SpecialTermOutofTexasInvoicesGeneration(PTXboSpecialTermInput specialInput);

        bool SubmitForInvoiceGeneration(int currentUserId, int currentUserRoleID, PTXboSpecialTermInvoiceDetails objdetails, out string errorMessage);

        List<PTXboSpecialTermClients> LoadSpecialTermsReGenerateInvoices(PTXboSpecialTermClientsFilter objFilter);//Added by saravanans.tfs id:55312
        bool GenerateInvoice(PTXboGenerateInvoice_Request request, out string errorMessage);//Added by SaravananS. tfs id:61899

        double GetTaxrateForTheGivenTaxyear(PTXboSpecialTermInvoiceTaxrate taxrateDetails, out string errorMessage);
    }
}
