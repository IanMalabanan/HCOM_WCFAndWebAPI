using System;
using System.ComponentModel.Composition;
using System.ServiceModel;
using Core.Common.Core;
using System.Configuration;
using System.Security.Principal;
using System.Threading;
using Serilog;
using SerilogWeb.Classic.Enrichers;

namespace CTI.HI.Business.Managers
{
    public class ManagerBase
    {

        public ManagerBase()
        {
            OperationContext context = OperationContext.Current;
            //if (context != null)
            //{
            //    try
            //    {
            //        _LoginName = OperationContext.Current.IncomingMessageHeaders.GetHeader<string>("Authorization", "CTI.Seller.Security"); //System or User Identity                    
            //        if (_LoginName.IndexOf(@"\") > -1)
            //        {
            //            _LoginName = string.Empty;
            //        }
            //        else
            //        {
            //            GenericPrincipal principal = new GenericPrincipal(
            //              new GenericIdentity(_LoginName), new string[] { "Administrators" });
            //            Thread.CurrentPrincipal = principal;

            //        }

            //    }
            //    catch (Exception ex)
            //    {
            //        _LoginName = string.Empty;
            //    }
            //}

            if (ObjectBase.Container != null)
                ObjectBase.Container.SatisfyImportsOnce(this);

            //if (!string.IsNullOrWhiteSpace(_LoginName))
            //    _AuthorizationAccount = LoadAuthorizationValidationAccount(_LoginName);
            
            string con = ConfigurationManager.AppSettings["SeqServer"];

            Log.Logger = new LoggerConfiguration()
                .Enrich.With<HttpRequestIdEnricher>()
                .Enrich.WithProperty("ApplicationName", ConfigurationManager.AppSettings["ApplicationName"])
                .Enrich.WithProperty("ApplicationBuild", ConfigurationManager.AppSettings["ApplicationBuild"])
                .Enrich.WithProperty("ApplicationEnv", ConfigurationManager.AppSettings["ApplicationEnv"])
                .WriteTo.Seq(con)
                .CreateLogger();
        }



        protected T ExecuteFaultHandledOperation<T>(Func<T> codetoExecute)
        {
            try
            {
                return codetoExecute.Invoke();
            }
            catch (ApplicationException ex)
            {
                Log.Error("ApplicationException", ex);
                throw ex;
            }
            catch (FaultException ex)
            {
                Log.Error("FaultException", ex);
                throw ex;
            }
            catch (Exception ex)
            {
                Log.Error("Exception", ex);
                throw new FaultException("");
            }
        }

        protected void ExecuteFaultHandledOperation(Action codetoExecute)
        {
            try
            {
                codetoExecute.Invoke();
            }
            catch (ApplicationException ex)
            {
                Log.Error("ApplicationException", ex);
                throw ex;
            }
            catch (FaultException ex)
            {
                Log.Error("FaultException", ex);
                throw ex;
            }
            catch (Exception ex)
            {
                Log.Error("Exception", ex);
                throw new FaultException(ex.Message);
            }
        }
    }
}
