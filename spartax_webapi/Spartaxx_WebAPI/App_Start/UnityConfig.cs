using System.Web.Http;
using Unity;
using Unity.WebApi;
using Spartaxx.BusinessService;
using Spartaxx.BusinessService.Interface;

namespace Spartaxx_WebAPI
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = BuildUnityContainer();
            
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();    
            RegisterTypes(container);

            return container;
        }

        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IHearingNoticeService, HearingNoticeService>();
            container.RegisterType<ITaskAllocationService, TaskAllocationService>();
            container.RegisterType<IInvoiceService, InvoiceService>();
            container.RegisterType<IClientSearchService, ClientSearchService>();
            container.RegisterType<IExistingClientService, ExistingClientService>();
            container.RegisterType<IAccountService, AccountService>();
            container.RegisterType<IHearingResultService, HearingResultService>();
            container.RegisterType<ICommonService, CommonService>();
            container.RegisterType<IInvoiceAdjustmentAuditService, InvoiceAdjustmentAuditService>();
            container.RegisterType<IInvoicePastdueConfigurationService, InvoicePastdueConfigurationService>();
            container.RegisterType<IEditInvoiceService, EditInvoiceService>();
            container.RegisterType<IInvoicePendingService, InvoicePendingService>();
            container.RegisterType<IInvoiceSpecialTermsService, InvoiceSpecialTermsService>();
            container.RegisterType<IInvoiceFlatFeePreService, InvoiceFlatFeePreService>();
            container.RegisterType<IClientIndexService, ClientIndexService>();
            container.RegisterType<IInvoiceDefectsService, InvoiceDefectsService>();
            container.RegisterType<IMainscreenInvoiceService, MainscreenInvoiceService>();
            container.RegisterType<IAffidavitService, AffidavitService>();
            container.RegisterType<IDeferredMaintenanceService, DeferredMaintenanceService>();
        }
    }
}