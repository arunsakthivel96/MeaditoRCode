using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.Common.BusinessObjects
{
    class PTXdtClientSearch
    {
        public int ClientID { get; set; }

        //public int ClientId { get; set; }       

        public string clientCode { get; set; }

        public string accountCode { get; set; }

        public string phoneNumber { get; set; }

        public string companyName { get; set; }

        public string cadLegalName { get; set; }

        public string projectName { get; set; }

        public string legalDescription { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public string address { get; set; }

        public string email { get; set; }

        public string clientNumber { get; set; }

        public string clientName { get; set; }

        public string accountNumber { get; set; }

        public int UserID { get; set; }

        public int UserRoleID { get; set; }

        public string SearchText { get; set; }

        public Enumerator.PTXenumClientSearchMode SearchMode { get; set; }

        public int? CountyId { get; set; }
        public string CountyName { get; set; }

        public int NavigationId { get; set; }
        public int AssignedQueueID { get; set; }

        public int AccountID { get; set; }
        public bool IsAutoAllocationSelected { get; set; }
        public DateTime? CreatedDateFrom { get; set; }
        public DateTime? CreatedDateTo { get; set; }
        public DateTime? ScannedDateFrom { get; set; }
        public DateTime? ScannedDateTo { get; set; }
        public int DocumentType { get; set; }
        public int DocumentTypeID { get; set; }
        public int PWImageID { get; set; }
        public int IndexClientDocumentId { get; set; }
        public int DoucumentQueueID { get; set; }
        public int DocumentDefectCodeID { get; set; }
        public int GroupID { get; set; }
        public int AgreementStatusCode { get; set; }
        public string CauseNumber { get; set; }
        public string CauseName { get; set; }
        public string ArbitrationID { get; set; }
        public int InvoiceNumber { get; set; }
        public string ParcelID { get; set; }
        public string StatusID { get; set; }
    }
}
