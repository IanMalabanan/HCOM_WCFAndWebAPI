//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ContractorSvcReference
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Contractor", Namespace="http://schemas.datacontract.org/2004/07/CTI.HI.Business.Entities")]
    public partial class Contractor : object
    {
        
        private string CodeField;
        
        private string NameField;
        
        private ContractorSvcReference.ContractorRepresentative[] RepresentativeField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Code
        {
            get
            {
                return this.CodeField;
            }
            set
            {
                this.CodeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name
        {
            get
            {
                return this.NameField;
            }
            set
            {
                this.NameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public ContractorSvcReference.ContractorRepresentative[] Representative
        {
            get
            {
                return this.RepresentativeField;
            }
            set
            {
                this.RepresentativeField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ContractorRepresentative", Namespace="http://schemas.datacontract.org/2004/07/CTI.HI.Business.Entities")]
    public partial class ContractorRepresentative : object
    {
        
        private string ContactNumberField;
        
        private string ContractorCodeField;
        
        private string EmailField;
        
        private int IdField;
        
        private string NameField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ContactNumber
        {
            get
            {
                return this.ContactNumberField;
            }
            set
            {
                this.ContactNumberField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ContractorCode
        {
            get
            {
                return this.ContractorCodeField;
            }
            set
            {
                this.ContractorCodeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Email
        {
            get
            {
                return this.EmailField;
            }
            set
            {
                this.EmailField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Id
        {
            get
            {
                return this.IdField;
            }
            set
            {
                this.IdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name
        {
            get
            {
                return this.NameField;
            }
            set
            {
                this.NameField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ContractorSvcReference.IContractorService")]
    public interface IContractorService
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IContractorService/GetContractor", ReplyAction="http://tempuri.org/IContractorService/GetContractorResponse")]
        System.Threading.Tasks.Task<ContractorSvcReference.Contractor> GetContractorAsync(string ContractorCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IContractorService/GetContractorRepresentative", ReplyAction="http://tempuri.org/IContractorService/GetContractorRepresentativeResponse")]
        System.Threading.Tasks.Task<ContractorSvcReference.ContractorRepresentative[]> GetContractorRepresentativeAsync(string ContractorCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IContractorService/GetVendorByRepresentative", ReplyAction="http://tempuri.org/IContractorService/GetVendorByRepresentativeResponse")]
        System.Threading.Tasks.Task<ContractorSvcReference.Contractor> GetVendorByRepresentativeAsync(string representative);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    public interface IContractorServiceChannel : ContractorSvcReference.IContractorService, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    public partial class ContractorServiceClient : System.ServiceModel.ClientBase<ContractorSvcReference.IContractorService>, ContractorSvcReference.IContractorService
    {
        
        /// <summary>
        /// Implement this partial method to configure the service endpoint.
        /// </summary>
        /// <param name="serviceEndpoint">The endpoint to configure</param>
        /// <param name="clientCredentials">The client credentials</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public ContractorServiceClient() : 
                base(ContractorServiceClient.GetDefaultBinding(), ContractorServiceClient.GetDefaultEndpointAddress())
        {
            this.Endpoint.Name = EndpointConfiguration.BasicHttpBinding_IContractorService.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public ContractorServiceClient(EndpointConfiguration endpointConfiguration) : 
                base(ContractorServiceClient.GetBindingForEndpoint(endpointConfiguration), ContractorServiceClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public ContractorServiceClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(ContractorServiceClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public ContractorServiceClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(ContractorServiceClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public ContractorServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public System.Threading.Tasks.Task<ContractorSvcReference.Contractor> GetContractorAsync(string ContractorCode)
        {
            return base.Channel.GetContractorAsync(ContractorCode);
        }
        
        public System.Threading.Tasks.Task<ContractorSvcReference.ContractorRepresentative[]> GetContractorRepresentativeAsync(string ContractorCode)
        {
            return base.Channel.GetContractorRepresentativeAsync(ContractorCode);
        }
        
        public System.Threading.Tasks.Task<ContractorSvcReference.Contractor> GetVendorByRepresentativeAsync(string representative)
        {
            return base.Channel.GetVendorByRepresentativeAsync(representative);
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
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IContractorService))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IContractorService))
            {
                return new System.ServiceModel.EndpointAddress("http://localhost:9999/ContractorService.svc");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding()
        {
            return ContractorServiceClient.GetBindingForEndpoint(EndpointConfiguration.BasicHttpBinding_IContractorService);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
        {
            return ContractorServiceClient.GetEndpointAddress(EndpointConfiguration.BasicHttpBinding_IContractorService);
        }
        
        public enum EndpointConfiguration
        {
            
            BasicHttpBinding_IContractorService,
        }
    }
}
