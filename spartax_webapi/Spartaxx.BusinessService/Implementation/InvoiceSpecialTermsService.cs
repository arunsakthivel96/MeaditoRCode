using Spartaxx.BusinessObjects;
using Spartaxx.BusinessObjects.Invoice;
using Spartaxx.BusinessService.Interface;
using Spartaxx.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.BusinessService
{
    //added by saravanan-tfs:47247
   public class InvoiceSpecialTermsService:IInvoiceSpecialTermsService 
    {
        private readonly UnitOfWork _unitOfWork = null;
        public InvoiceSpecialTermsService()
        {
            _unitOfWork = new UnitOfWork();
        }

        public List<PTXboSpecialTermClients> LoadSpecialTermClients(PTXboSpecialTermClientsFilter specialTermClientFilter)
        {
            return _unitOfWork.InvoiceSpecialTermsRepository.LoadSpecialTermClients(specialTermClientFilter);
        }


        public List<PTXboSpecialTermClients> LoadSpecialTermOutofTexasClients(PTXboSpecialTermClientsFilter specialTermClientFilter)
        {
            return _unitOfWork.InvoiceSpecialTermsRepository.LoadSpecialTermOutofTexasClients(specialTermClientFilter);
        }

        public List<PTXboSpecialTermClients> LoadSpecialTermsDonotGenerateInvoices(PTXboSpecialTermClientsFilter objFilter)
        {
            return _unitOfWork.InvoiceSpecialTermsRepository.LoadSpecialTermsDonotGenerateInvoices(objFilter);
        }

        /// <summary>
        /// Added by saravanans.tfs id:55312
        /// </summary>
        /// <param name="objFilter"></param>
        /// <returns></returns>
        public List<PTXboSpecialTermClients> LoadSpecialTermsReGenerateInvoices(PTXboSpecialTermClientsFilter objFilter)
        {
            return _unitOfWork.InvoiceSpecialTermsRepository.LoadSpecialTermsReGenerateInvoices(objFilter);
        }

        public bool UpdateInvoiceDetailsReport(PTXboInvoiceReport objInvoice)
        {
            return _unitOfWork.InvoiceSpecialTermsRepository.UpdateInvoiceDetailsReport(objInvoice);
        }

        public bool UpdateInvoiceProcessStatus(PTXboUpdateInvoiceProcessingStatusInput invoiceProcessingStatus)
        {
            return _unitOfWork.InvoiceSpecialTermsRepository.UpdateInvoiceProcessStatus(invoiceProcessingStatus);
        }

        public PTXboSpecialTermInvoiceDetails SpecialTermInvoicesGeneration(PTXboSpecialTermInput specialInput)
        {
            return _unitOfWork.InvoiceSpecialTermsRepository.SpecialTermInvoicesGeneration(specialInput);
        }

        public PTXboSpecialTermInvoiceDetails SpecialTermOutofTexasInvoicesGeneration(PTXboSpecialTermInput specialInput)
        {
            return _unitOfWork.InvoiceSpecialTermsRepository.SpecialTermOutofTexasInvoicesGeneration(specialInput);
        }

        public bool SubmitForInvoiceGeneration(int currentUserId, int currentUserRoleID, PTXboSpecialTermInvoiceDetails objdetails, out string errorMessage)
        {
            errorMessage = string.Empty;
            return _unitOfWork.InvoiceSpecialTermsRepository.SubmitForInvoiceGeneration(currentUserId, currentUserRoleID, objdetails,out errorMessage);
        }

        public bool GenerateInvoice(PTXboGenerateInvoice_Request request, out string errorMessage)
        {
            errorMessage = string.Empty;
            return _unitOfWork.InvoiceSpecialTermsRepository.GenerateInvoice(request, out errorMessage);
        }

        public double GetTaxrateForTheGivenTaxyear(PTXboSpecialTermInvoiceTaxrate request, out string errorMessage)
        {
            errorMessage = string.Empty;
            return _unitOfWork.InvoiceSpecialTermsRepository.GetTaxrateForTheGivenTaxyear(request, out errorMessage);
        }

    }
}
