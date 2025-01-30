using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.DataAccess
{
    public class UnitOfWork
    {
        private readonly DapperConnection _Connection;

        public UnitOfWork()
        {
            if (_Connection == null)
            {
                _Connection = new DapperConnection();
            }
        }


        private IHearingNoticeRepocitory _hearingNoticeRepocitory;
        public IHearingNoticeRepocitory HearingNoticeRepocitory
        {
            get
            {
                if (this._hearingNoticeRepocitory == null)
                {
                    this._hearingNoticeRepocitory = new HearingNoticeRepocitory(_Connection);
                }
                return _hearingNoticeRepocitory;
            }
        }

        private IHearingResultRepocitory _hearingResultRepocitory;
        public IHearingResultRepocitory HearingResultRepocitory
        {
            get
            {
                if (this._hearingResultRepocitory == null)
                {
                    this._hearingResultRepocitory = new HearingResultRepocitory(_Connection);
                }
                return _hearingResultRepocitory;
            }
        }

        private ITaskAllocationRepocitory _TaskAllocationRepocitory;
        public ITaskAllocationRepocitory TaskAllocationRepocitory
        {
            get
            {
                if (this._TaskAllocationRepocitory == null)
                {
                    this._TaskAllocationRepocitory = new TaskAllocationRepocitory(_Connection);
                }
                return _TaskAllocationRepocitory;
            }
        }

        private IInvoiceRepository _invoiceRepository;
        public IInvoiceRepository InvoiceRepository
        {
            get
            {
                if(this._invoiceRepository==null)
                {
                    this._invoiceRepository = new InvoiceRepository(_Connection);
                }
                return _invoiceRepository;
            }
        }

        private IClientSearchRepository _ClientSearchRepository;
        public IClientSearchRepository ClientSearchRepository
        {
            get
            {
                if (this._ClientSearchRepository == null)
                {
                    this._ClientSearchRepository = new ClientSearchRepository(_Connection);
                }
                return _ClientSearchRepository;
            }
        }

        private IExistingClientRepository _ExistingClientRepository;
        public IExistingClientRepository ExistingClientRepository
        {
            get
            {
                if (this._ExistingClientRepository == null)
                {
                    this._ExistingClientRepository = new ExistingClientRepository(_Connection);
                }
                return _ExistingClientRepository;
            }
        }

        private IAccountRepository _AccountRepository;
        public IAccountRepository AccountRepository
        {
            get
            {
                if (this._AccountRepository == null)
                {
                    this._AccountRepository = new AccountRepository(_Connection);
                }
                return _AccountRepository;
            }
        }

        private ICommonRepocitory _commonRepocitory;
        public ICommonRepocitory CommonRepocitory
        {
            get
            {
                if (this._commonRepocitory == null)
                {
                    this._commonRepocitory = new CommonRepocitory(_Connection);
                }
                return _commonRepocitory;
            }
        }

        private IInvoiceAdjustmentAuditRepository _invoiceAdjustmentAuditRepository;
        public IInvoiceAdjustmentAuditRepository InvoiceAdjustmentAuditRepository
        {
            get
            {
                if(this._invoiceAdjustmentAuditRepository==null)
                {
                    this._invoiceAdjustmentAuditRepository = new InvoiceAdjustmentAuditRepository(_Connection);
                }
                return _invoiceAdjustmentAuditRepository;
            }
        }

        private IInvoicePastdueConfigurationRepository _invoicePastdueConfigurationRepository;

        public IInvoicePastdueConfigurationRepository InvoicePastdueConfigurationRepository
        {
            get
            {
                if(this._invoicePastdueConfigurationRepository==null)
                {
                    this._invoicePastdueConfigurationRepository = new InvoicePastdueConfigurationRepository(_Connection);
                }
                return _invoicePastdueConfigurationRepository;
            }
        }


        private IEditInvoiceRepository _editInvoiceRepository;
        public IEditInvoiceRepository EditInvoiceRepository
        {
            get
            {
                if(this._editInvoiceRepository==null)
                {
                    this._editInvoiceRepository = new EditInvoiceRepository(_Connection);
                }
                return _editInvoiceRepository;
            }
        }

        private IInvoicePendingRepository _invoicePendingRepository;
        public IInvoicePendingRepository InvoicePendingRepository
        {
            get
            {
                if(this._invoicePendingRepository==null)
                {
                    
                    this._invoicePendingRepository = new InvoicePendingRepository(_Connection);
                }
                return _invoicePendingRepository;
            }
        }

        private IInvoiceSpecialTermsRepository _invoiceSpecialTermsRepository;
        public IInvoiceSpecialTermsRepository InvoiceSpecialTermsRepository
        {
            get
            {
                if(this._invoiceSpecialTermsRepository==null)
                {
                    
                    this._invoiceSpecialTermsRepository = new InvoiceSpecialTermsRepository(_Connection);
                }
                return _invoiceSpecialTermsRepository;
            }
        }

        private IInvoiceFlatFeePreRepository _invoiceFlatFeePreRepository;
        public IInvoiceFlatFeePreRepository InvoiceFlatFeePreRepository
        {
            get
            {
                if(this._invoiceFlatFeePreRepository==null)
                {
                    this._invoiceFlatFeePreRepository = new InvoiceFlatFeePreRepository(_Connection);
                }
                return _invoiceFlatFeePreRepository;
            }
        }

        private IClientIndexRepository _clientIndexRepository;
        public IClientIndexRepository ClientIndexRepository
        {
            get
            {
                if (this._clientIndexRepository == null)
                {
                    this._clientIndexRepository = new ClientIndexRepository(_Connection);
                }
                return _clientIndexRepository;
            }
        }

        private IInvoiceDefectsRepository _invoiceDefectsRepository;
        public IInvoiceDefectsRepository InvoiceDefectsRepository
        {
            get
            {
                if(this._invoiceDefectsRepository==null)
                {
                    this._invoiceDefectsRepository = new InvoiceDefectsRepository(_Connection);
                }
                return _invoiceDefectsRepository;
            }
        }

        private IMainscreenInvoiceRepository _mainscreenInvoiceRepository;
        
        public IMainscreenInvoiceRepository MainscreenInvoiceRepository
        {
            get
            {
                if(this._mainscreenInvoiceRepository==null)
                {
                    this._mainscreenInvoiceRepository = new MainscreenInvoiceRepository(_Connection);
                }
                return _mainscreenInvoiceRepository;
            }
        }
        private IAffidavitRepocitory _AffidavitRepository;
        public IAffidavitRepocitory AffidavitRepository
        {
            get
            {
                if (this._AffidavitRepository == null)
                {
                    this._AffidavitRepository = new AffidavitRepocitory(_Connection);
                }
                return _AffidavitRepository;
            }
        }


        private IDeferredMaintenanceRepository _DeferredRepository;
        public IDeferredMaintenanceRepository DeferredRepository
        {
            get
            {
                if (this._DeferredRepository == null)
                {
                    this._DeferredRepository = new DeferredMaintenanceRepository(_Connection);
                }
                return _DeferredRepository;
            }
        }

    }
}
