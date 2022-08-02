﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NotificationSvcReference
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.1")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="NotificationSvcReference.INotificationService")]
    public interface INotificationService
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/INotificationService/GetOpenPunchlists", ReplyAction="http://tempuri.org/INotificationService/GetOpenPunchlistsResponse")]
        System.Threading.Tasks.Task<CTI.HI.Business.Entities.NotificationModel[]> GetOpenPunchlistsAsync(string userName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/INotificationService/GetOpenOverduePunchlists", ReplyAction="http://tempuri.org/INotificationService/GetOpenOverduePunchlistsResponse")]
        System.Threading.Tasks.Task<CTI.HI.Business.Entities.NotificationModel[]> GetOpenOverduePunchlistsAsync(string userName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/INotificationService/GetRecentlyClosedPunchlists", ReplyAction="http://tempuri.org/INotificationService/GetRecentlyClosedPunchlistsResponse")]
        System.Threading.Tasks.Task<CTI.HI.Business.Entities.NotificationModel[]> GetRecentlyClosedPunchlistsAsync(string userName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/INotificationService/GetDelayedMilestones", ReplyAction="http://tempuri.org/INotificationService/GetDelayedMilestonesResponse")]
        System.Threading.Tasks.Task<CTI.HI.Business.Entities.NotificationModel[]> GetDelayedMilestonesAsync(string userName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/INotificationService/GetRecentlyClosedMilestones", ReplyAction="http://tempuri.org/INotificationService/GetRecentlyClosedMilestonesResponse")]
        System.Threading.Tasks.Task<CTI.HI.Business.Entities.NotificationModel[]> GetRecentlyClosedMilestonesAsync(string userName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/INotificationService/GetMessagingInformation", ReplyAction="http://tempuri.org/INotificationService/GetMessagingInformationResponse")]
        System.Threading.Tasks.Task<CTI.HI.Business.Entities.MessagingInformation> GetMessagingInformationAsync(int punchlistId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/INotificationService/SendPunchlistNotification", ReplyAction="http://tempuri.org/INotificationService/SendPunchlistNotificationResponse")]
        System.Threading.Tasks.Task SendPunchlistNotificationAsync(int punchlistId);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.1")]
    public interface INotificationServiceChannel : NotificationSvcReference.INotificationService, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.1")]
    public partial class NotificationServiceClient : System.ServiceModel.ClientBase<NotificationSvcReference.INotificationService>, NotificationSvcReference.INotificationService
    {
        
        /// <summary>
        /// Implement this partial method to configure the service endpoint.
        /// </summary>
        /// <param name="serviceEndpoint">The endpoint to configure</param>
        /// <param name="clientCredentials">The client credentials</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public NotificationServiceClient() : 
                base(NotificationServiceClient.GetDefaultBinding(), NotificationServiceClient.GetDefaultEndpointAddress())
        {
            this.Endpoint.Name = EndpointConfiguration.BasicHttpBinding_INotificationService.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public NotificationServiceClient(EndpointConfiguration endpointConfiguration) : 
                base(NotificationServiceClient.GetBindingForEndpoint(endpointConfiguration), NotificationServiceClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public NotificationServiceClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(NotificationServiceClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public NotificationServiceClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(NotificationServiceClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public NotificationServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public System.Threading.Tasks.Task<CTI.HI.Business.Entities.NotificationModel[]> GetOpenPunchlistsAsync(string userName)
        {
            return base.Channel.GetOpenPunchlistsAsync(userName);
        }
        
        public System.Threading.Tasks.Task<CTI.HI.Business.Entities.NotificationModel[]> GetOpenOverduePunchlistsAsync(string userName)
        {
            return base.Channel.GetOpenOverduePunchlistsAsync(userName);
        }
        
        public System.Threading.Tasks.Task<CTI.HI.Business.Entities.NotificationModel[]> GetRecentlyClosedPunchlistsAsync(string userName)
        {
            return base.Channel.GetRecentlyClosedPunchlistsAsync(userName);
        }
        
        public System.Threading.Tasks.Task<CTI.HI.Business.Entities.NotificationModel[]> GetDelayedMilestonesAsync(string userName)
        {
            return base.Channel.GetDelayedMilestonesAsync(userName);
        }
        
        public System.Threading.Tasks.Task<CTI.HI.Business.Entities.NotificationModel[]> GetRecentlyClosedMilestonesAsync(string userName)
        {
            return base.Channel.GetRecentlyClosedMilestonesAsync(userName);
        }
        
        public System.Threading.Tasks.Task<CTI.HI.Business.Entities.MessagingInformation> GetMessagingInformationAsync(int punchlistId)
        {
            return base.Channel.GetMessagingInformationAsync(punchlistId);
        }
        
        public System.Threading.Tasks.Task SendPunchlistNotificationAsync(int punchlistId)
        {
            return base.Channel.SendPunchlistNotificationAsync(punchlistId);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        public virtual System.Threading.Tasks.Task CloseAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginClose(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndClose));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_INotificationService))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.SendTimeout = System.TimeSpan.MaxValue;
                result.ReceiveTimeout = System.TimeSpan.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_INotificationService))
            {
                //return new System.ServiceModel.EndpointAddress("http://10.130.4.58:8088/NotificationService.svc"); //QA & PROD
                return new System.ServiceModel.EndpointAddress("http://localhost:9999/NotificationService.svc");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding()
        {
            return NotificationServiceClient.GetBindingForEndpoint(EndpointConfiguration.BasicHttpBinding_INotificationService);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
        {
            return NotificationServiceClient.GetEndpointAddress(EndpointConfiguration.BasicHttpBinding_INotificationService);
        }
        
        public enum EndpointConfiguration
        {
            
            BasicHttpBinding_INotificationService,
        }
    }
}
