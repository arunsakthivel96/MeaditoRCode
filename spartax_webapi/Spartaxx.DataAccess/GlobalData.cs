using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Spartaxx.Common.Reports;
using Spartaxx.Utilities.Logging;
using System.Data;
using System.IO;
using System.Threading;

namespace Spartaxx.DataAccess
{
    public static class GlobalData
    {
        private static readonly object _syncRoot = new object();
        private static ReportDocument _standardInvoice=new ReportDocument();
        private static ReportDocument _taxbillInvoice= new ReportDocument();
        private static ReportDocument _standardInvoicewithoutlineitem = new ReportDocument();
        private static ReportDocument _standardInvoicewithFlatfee = new ReportDocument();

        private static string _tempFilePathstandardInvoice { get; set; }
        private static string _tempFilePathtaxbillInvoice { get; set; }
        private static string _tempFilePathstandardInvoicewithoutlineitem { get; set; }
        private static string _tempFilePathstandardInvoicewithFlatfee { get; set; }

        public static ReportDocument StandardInvoice
        {

            get { 
                if (string.IsNullOrEmpty(_tempFilePathstandardInvoice))
                {
                    Logger.For("GlobalData::StandardInvoice").Invoice("GlobalData::StandardInvoice object creation is done");
                     _tempFilePathstandardInvoice = (Path.GetDirectoryName(typeof(GlobalData).Assembly.CodeBase) + @"\Reports\Invoice\mstrpt_Invoice.rpt").Replace("file:\\", "");
                     _standardInvoice.Load(_tempFilePathstandardInvoice);
                    Logger.For("GlobalData::StandardInvoice").Invoice("GlobalData::StandardInvoice _tempFilePathstandardInvoice::"+ _tempFilePathstandardInvoice);
                }
                return _standardInvoice;
            }//lock (_syncRoot) { return _standardInvoice; }
            set { _standardInvoice = value; }
        }

        public static ReportDocument TaxbillInvoice
        {
            get { 
                if (string.IsNullOrEmpty(_tempFilePathtaxbillInvoice))
                {
                    Logger.For("GlobalData::TaxbillInvoice").Invoice("GlobalData::TaxbillInvoice object creation is done");
                     _tempFilePathtaxbillInvoice = (Path.GetDirectoryName(typeof(GlobalData).Assembly.CodeBase) + @"\Reports\Invoice\mstrpt_Invoice_TaxBill_BPP.rpt").Replace("file:\\", "");
                     _taxbillInvoice.Load(_tempFilePathtaxbillInvoice);
                    Logger.For("GlobalData::TaxbillInvoice").Invoice("GlobalData::TaxbillInvoice _tempFilePathtaxbillInvoice::"+ _tempFilePathtaxbillInvoice);
                }
                return _taxbillInvoice;  }//lock (_syncRoot) { return _taxbillInvoice; }
            set { _taxbillInvoice = value; }
        }
        public static ReportDocument StandardInvoicewithoutlineitem
        {
            get { 
                if (string.IsNullOrEmpty(_tempFilePathstandardInvoicewithoutlineitem))
                {
                    Logger.For("GlobalData::StandardInvoicewithoutlineitem").Invoice("GlobalData::StandardInvoicewithoutlineitem object creation is done");
                     _tempFilePathstandardInvoicewithoutlineitem = (Path.GetDirectoryName(typeof(GlobalData).Assembly.CodeBase) + @"\Reports\Invoice\mstrpt_Invoice_withoutlineitem.rpt").Replace("file:\\", "");
                     _standardInvoicewithoutlineitem.Load(_tempFilePathstandardInvoicewithoutlineitem);
                    Logger.For("GlobalData::StandardInvoicewithoutlineitem").Invoice("GlobalData::StandardInvoicewithoutlineitem _tempFilePathstandardInvoicewithoutlineitem::"+ _tempFilePathstandardInvoicewithoutlineitem);
                }
                return _standardInvoicewithoutlineitem;
              }//lock (_syncRoot) { return _standardInvoicewithoutlineitem; }
            set { _standardInvoicewithoutlineitem = value; }
        }

        public static ReportDocument StandardInvoicewithFlatfee
        {
            get { 
                if (string.IsNullOrEmpty(_tempFilePathstandardInvoicewithFlatfee))
                {
                    Logger.For("GlobalData::StandardInvoicewithFlatfee").Invoice("GlobalData::StandardInvoicewithFlatfee object creation is done");
                     _tempFilePathstandardInvoicewithFlatfee = (Path.GetDirectoryName(typeof(GlobalData).Assembly.CodeBase) + @"\Reports\Invoice\mstrpt_InvoiceFlatFeePreHearing.rpt").Replace("file:\\", "");
                     _standardInvoicewithFlatfee.Load(_tempFilePathstandardInvoicewithFlatfee);
                    Logger.For("GlobalData::StandardInvoicewithFlatfee").Invoice("GlobalData::StandardInvoicewithFlatfee _tempFilePathstandardInvoicewithFlatfee::"+ _tempFilePathstandardInvoicewithFlatfee);
                }
                return _standardInvoicewithFlatfee;
              }// lock (_syncRoot) { return _standardInvoicewithFlatfee; }
            set { _standardInvoicewithFlatfee = value; }
        }

        public static ReportGenResult SetDataSource(int ReportFilePathType, object Model, ReportGenType ReportType)
        {
            Logger.For("GlobalData::SetDataSource").Invoice("GlobalData::SetDataSource");
            ReportGenResult reportResult;
            bool lockWasTaken = false;
            var temp = _syncRoot;
            
                try
                {
                        Monitor.Enter(temp, ref lockWasTaken);
                        Logger.For("GlobalData::SetDataSource").Invoice("GlobalData::SetDataSource-inside locked");
                        //setting datasource
                        if (Model != null && Model.GetType() != typeof(string))
                        {
                            if (Model.GetType() == typeof(DataSet))
                            {
                                if (((System.Data.DataSet)Model).Tables.Count > 0)
                                {
                                    if (ReportFilePathType == 1)//flatfee
                                    {
                                        StandardInvoicewithFlatfee.SetDataSource(((System.Data.DataSet)Model));
                                        // StandardInvoicewithFlatfee.Refresh();
                                    }
                                    else if (ReportFilePathType == 2)//standard
                                    {
                                        StandardInvoice.SetDataSource(((System.Data.DataSet)Model));
                                        //StandardInvoice.Refresh();
                                    }
                                    else if (ReportFilePathType == 3)//standardwithoutlineitem
                                    {
                                        StandardInvoicewithoutlineitem.SetDataSource(((System.Data.DataSet)Model));
                                        // StandardInvoicewithoutlineitem.Refresh();
                                    }
                                    else if (ReportFilePathType == 4)//taxbill
                                    {
                                        TaxbillInvoice.SetDataSource(((System.Data.DataSet)Model));
                                        //TaxbillInvoice.Refresh();
                                    }
                                    // rd.SetDataSource(((System.Data.DataSet)Model));
                                }

                            }
                            else
                            {
                                // rd.SetDataSource(new[] { Model });
                                if (ReportFilePathType == 1)//flatfee
                                {
                                    StandardInvoicewithFlatfee.SetDataSource(new[] { Model });
                                    //StandardInvoicewithFlatfee.Refresh();
                                }
                                else if (ReportFilePathType == 2)//standard
                                {
                                    StandardInvoice.SetDataSource(new[] { Model });
                                    //StandardInvoice.Refresh();
                                }
                                else if (ReportFilePathType == 3)//standardwithoutlineitem
                                {
                                    StandardInvoicewithoutlineitem.SetDataSource(new[] { Model });
                                    //StandardInvoicewithoutlineitem.Refresh();
                                }
                                else if (ReportFilePathType == 4)//taxbill
                                {
                                    TaxbillInvoice.SetDataSource(new[] { Model });
                                    //TaxbillInvoice.Refresh();
                                }
                            }
                        }
                    //setting datasource ends here.

                    //filestream
                    Logger.For("GlobalData::SetDataSource").Invoice("GlobalData::SetDataSource-filestream generation starts");
                    if (ReportType == ReportGenType.Pdf)
                        {
                            if (ReportFilePathType == 1)//flatfee
                            {
                                Stream _CrystalReportObject = StandardInvoicewithFlatfee.ExportToStream(ExportFormatType.PortableDocFormat);
                                reportResult = new ReportGenResult() { MimeType = "application/pdf", FileNameExtension = "pdf", CrystalReportObject = _CrystalReportObject };
                            }
                            else if (ReportFilePathType == 2)//standard
                            {
                                Stream _CrystalReportObject = StandardInvoice.ExportToStream(ExportFormatType.PortableDocFormat);
                                reportResult = new ReportGenResult() { MimeType = "application/pdf", FileNameExtension = "pdf", CrystalReportObject = _CrystalReportObject };
                            }
                            else if (ReportFilePathType == 3)//standardwithoutlineitem
                            {
                                Stream _CrystalReportObject = StandardInvoicewithoutlineitem.ExportToStream(ExportFormatType.PortableDocFormat);
                                reportResult = new ReportGenResult() { MimeType = "application/pdf", FileNameExtension = "pdf", CrystalReportObject = _CrystalReportObject };
                            }
                            else if (ReportFilePathType == 4)//taxbill
                            {
                                Stream _CrystalReportObject = TaxbillInvoice.ExportToStream(ExportFormatType.PortableDocFormat);
                                reportResult = new ReportGenResult() { MimeType = "application/pdf", FileNameExtension = "pdf", CrystalReportObject = _CrystalReportObject };
                            }
                            else
                            {
                                reportResult = new ReportGenResult() { CrystalReportObject = null };
                            }

                        }
                        else
                        {
                            reportResult = new ReportGenResult() { CrystalReportObject = null };
                        }
                    return reportResult;
                  }

                catch (SynchronizationLockException SyncEx)
                {
                Logger.For("GlobalData::SetDataSource").Invoice("GlobalData::SetDataSource:A SynchronizationLockException occurred. Message:" + SyncEx.Message);
                reportResult = new ReportGenResult() { CrystalReportObject = null };
                return reportResult;
                 }
                finally
                {
                    Logger.For("GlobalData::SetDataSource").Invoice("GlobalData::SetDataSource-inside finally block");
                    if (lockWasTaken)
                    {
                        Monitor.Exit(temp);
                    }
                }
                
            

    
                //ends here

            

        }

    }

}
