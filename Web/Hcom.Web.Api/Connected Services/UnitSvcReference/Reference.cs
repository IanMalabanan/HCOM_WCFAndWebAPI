//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UnitSvcReference
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Project", Namespace="http://schemas.datacontract.org/2004/07/CTI.HI.Business.Entities")]
    public partial class Project : object
    {
        
        private string CodeField;
        
        private string ImageUrlField;
        
        private System.Nullable<decimal> LatitudeField;
        
        private string LongNameField;
        
        private System.Nullable<decimal> LongitudeField;
        
        private string ShortNameField;
        
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
        public string ImageUrl
        {
            get
            {
                return this.ImageUrlField;
            }
            set
            {
                this.ImageUrlField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<decimal> Latitude
        {
            get
            {
                return this.LatitudeField;
            }
            set
            {
                this.LatitudeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string LongName
        {
            get
            {
                return this.LongNameField;
            }
            set
            {
                this.LongNameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<decimal> Longitude
        {
            get
            {
                return this.LongitudeField;
            }
            set
            {
                this.LongitudeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ShortName
        {
            get
            {
                return this.ShortNameField;
            }
            set
            {
                this.ShortNameField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.Runtime.Serialization.DataContractAttribute(Name="PhaseBuilding", Namespace="http://schemas.datacontract.org/2004/07/CTI.HI.Business.Entities")]
    public partial class PhaseBuilding : object
    {
        
        private string CodeField;
        
        private double LatitudeField;
        
        private string LongNameField;
        
        private double LongitudeField;
        
        private string ShortNameField;
        
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
        public double Latitude
        {
            get
            {
                return this.LatitudeField;
            }
            set
            {
                this.LatitudeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string LongName
        {
            get
            {
                return this.LongNameField;
            }
            set
            {
                this.LongNameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public double Longitude
        {
            get
            {
                return this.LongitudeField;
            }
            set
            {
                this.LongitudeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ShortName
        {
            get
            {
                return this.ShortNameField;
            }
            set
            {
                this.ShortNameField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Block", Namespace="http://schemas.datacontract.org/2004/07/CTI.HI.Business.Entities")]
    public partial class Block : object
    {
        
        private string CodeField;
        
        private double LatitudeField;
        
        private string LongNameField;
        
        private double LongitudeField;
        
        private string ShortNameField;
        
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
        public double Latitude
        {
            get
            {
                return this.LatitudeField;
            }
            set
            {
                this.LatitudeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string LongName
        {
            get
            {
                return this.LongNameField;
            }
            set
            {
                this.LongNameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public double Longitude
        {
            get
            {
                return this.LongitudeField;
            }
            set
            {
                this.LongitudeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ShortName
        {
            get
            {
                return this.ShortNameField;
            }
            set
            {
                this.ShortNameField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Unit", Namespace="http://schemas.datacontract.org/2004/07/CTI.HI.Business.Entities")]
    public partial class Unit : object
    {
        
        private UnitSvcReference.Block BlockFloorField;
        
        private string[] FloorPlanField;
        
        private string InventoryUnitNumberField;
        
        private System.Nullable<decimal> LatitudeField;
        
        private System.Nullable<decimal> LongitudeField;
        
        private string LotUnitShareNumberField;
        
        private UnitSvcReference.MilestonePercentage MilestonePercentageField;
        
        private UnitSvcReference.PhaseBuilding PhaseBuildingField;
        
        private UnitSvcReference.Project ProjectField;
        
        private string ReferenceObjectField;
        
        private string VendorCodeField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public UnitSvcReference.Block BlockFloor
        {
            get
            {
                return this.BlockFloorField;
            }
            set
            {
                this.BlockFloorField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] FloorPlan
        {
            get
            {
                return this.FloorPlanField;
            }
            set
            {
                this.FloorPlanField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string InventoryUnitNumber
        {
            get
            {
                return this.InventoryUnitNumberField;
            }
            set
            {
                this.InventoryUnitNumberField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<decimal> Latitude
        {
            get
            {
                return this.LatitudeField;
            }
            set
            {
                this.LatitudeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<decimal> Longitude
        {
            get
            {
                return this.LongitudeField;
            }
            set
            {
                this.LongitudeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string LotUnitShareNumber
        {
            get
            {
                return this.LotUnitShareNumberField;
            }
            set
            {
                this.LotUnitShareNumberField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public UnitSvcReference.MilestonePercentage MilestonePercentage
        {
            get
            {
                return this.MilestonePercentageField;
            }
            set
            {
                this.MilestonePercentageField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public UnitSvcReference.PhaseBuilding PhaseBuilding
        {
            get
            {
                return this.PhaseBuildingField;
            }
            set
            {
                this.PhaseBuildingField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public UnitSvcReference.Project Project
        {
            get
            {
                return this.ProjectField;
            }
            set
            {
                this.ProjectField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ReferenceObject
        {
            get
            {
                return this.ReferenceObjectField;
            }
            set
            {
                this.ReferenceObjectField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string VendorCode
        {
            get
            {
                return this.VendorCodeField;
            }
            set
            {
                this.VendorCodeField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.Runtime.Serialization.DataContractAttribute(Name="MilestonePercentage", Namespace="http://schemas.datacontract.org/2004/07/CTI.HI.Business.Entities")]
    public partial class MilestonePercentage : object
    {
        
        private System.Nullable<decimal> BillingPercentageField;
        
        private string ContractorsField;
        
        private string OngoingMilestoneActivityField;
        
        private System.Nullable<decimal> PercentageCompletionField;
        
        private int PunchlistCountField;
        
        private int PunchlistOverdueCountField;
        
        private int PunchlistPendingCountField;
        
        private string ReferenceObjectField;
        
        private System.Nullable<decimal> TargetPercentageCompletionField;
        
        private string VarianceField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<decimal> BillingPercentage
        {
            get
            {
                return this.BillingPercentageField;
            }
            set
            {
                this.BillingPercentageField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Contractors
        {
            get
            {
                return this.ContractorsField;
            }
            set
            {
                this.ContractorsField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string OngoingMilestoneActivity
        {
            get
            {
                return this.OngoingMilestoneActivityField;
            }
            set
            {
                this.OngoingMilestoneActivityField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<decimal> PercentageCompletion
        {
            get
            {
                return this.PercentageCompletionField;
            }
            set
            {
                this.PercentageCompletionField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int PunchlistCount
        {
            get
            {
                return this.PunchlistCountField;
            }
            set
            {
                this.PunchlistCountField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int PunchlistOverdueCount
        {
            get
            {
                return this.PunchlistOverdueCountField;
            }
            set
            {
                this.PunchlistOverdueCountField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int PunchlistPendingCount
        {
            get
            {
                return this.PunchlistPendingCountField;
            }
            set
            {
                this.PunchlistPendingCountField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ReferenceObject
        {
            get
            {
                return this.ReferenceObjectField;
            }
            set
            {
                this.ReferenceObjectField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<decimal> TargetPercentageCompletion
        {
            get
            {
                return this.TargetPercentageCompletionField;
            }
            set
            {
                this.TargetPercentageCompletionField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Variance
        {
            get
            {
                return this.VarianceField;
            }
            set
            {
                this.VarianceField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="UnitSvcReference.IUnitService")]
    public interface IUnitService
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUnitService/GetProjectByUser", ReplyAction="http://tempuri.org/IUnitService/GetProjectByUserResponse")]
        System.Threading.Tasks.Task<UnitSvcReference.Project[]> GetProjectByUserAsync(string username);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUnitService/GetPhaseBuildingByProject", ReplyAction="http://tempuri.org/IUnitService/GetPhaseBuildingByProjectResponse")]
        System.Threading.Tasks.Task<UnitSvcReference.PhaseBuilding[]> GetPhaseBuildingByProjectAsync(string username, string projectCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUnitService/GetBlockByPhaseBuilding", ReplyAction="http://tempuri.org/IUnitService/GetBlockByPhaseBuildingResponse")]
        System.Threading.Tasks.Task<UnitSvcReference.Block[]> GetBlockByPhaseBuildingAsync(string username, string phaseBuildingCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUnitService/GetUnitByBlock", ReplyAction="http://tempuri.org/IUnitService/GetUnitByBlockResponse")]
        System.Threading.Tasks.Task<UnitSvcReference.Unit[]> GetUnitByBlockAsync(string username, string blockCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUnitService/GetUnitByUser", ReplyAction="http://tempuri.org/IUnitService/GetUnitByUserResponse")]
        System.Threading.Tasks.Task<UnitSvcReference.Unit[]> GetUnitByUserAsync(string username, string userRole, double latitude, double longitude);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUnitService/GetUnit", ReplyAction="http://tempuri.org/IUnitService/GetUnitResponse")]
        System.Threading.Tasks.Task<UnitSvcReference.Unit> GetUnitAsync(string username, string userRole, double latitude, double longitude, string referenceObject);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUnitService/GetUnitFloorPlan", ReplyAction="http://tempuri.org/IUnitService/GetUnitFloorPlanResponse")]
        System.Threading.Tasks.Task<string[]> GetUnitFloorPlanAsync(string referenceObject);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUnitService/GetUnitByReferenceObjects", ReplyAction="http://tempuri.org/IUnitService/GetUnitByReferenceObjectsResponse")]
        System.Threading.Tasks.Task<UnitSvcReference.Unit[]> GetUnitByReferenceObjectsAsync(string[] referenceObject);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUnitService/GetUnitByRefObject", ReplyAction="http://tempuri.org/IUnitService/GetUnitByRefObjectResponse")]
        System.Threading.Tasks.Task<UnitSvcReference.Unit[]> GetUnitByRefObjectAsync(string username, string referenceObject);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUnitService/GetNewUnitByUser", ReplyAction="http://tempuri.org/IUnitService/GetNewUnitByUserResponse")]
        System.Threading.Tasks.Task<UnitSvcReference.Unit[]> GetNewUnitByUserAsync(string username);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUnitService/GetNewUnitByProject", ReplyAction="http://tempuri.org/IUnitService/GetNewUnitByProjectResponse")]
        System.Threading.Tasks.Task<UnitSvcReference.Unit[]> GetNewUnitByProjectAsync(string username, string projectCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUnitService/GetUnitByRefObjectByVendor", ReplyAction="http://tempuri.org/IUnitService/GetUnitByRefObjectByVendorResponse")]
        System.Threading.Tasks.Task<UnitSvcReference.Unit[]> GetUnitByRefObjectByVendorAsync(string username, string referenceObject, string vendorCode);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    public interface IUnitServiceChannel : UnitSvcReference.IUnitService, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    public partial class UnitServiceClient : System.ServiceModel.ClientBase<UnitSvcReference.IUnitService>, UnitSvcReference.IUnitService
    {
        
        /// <summary>
        /// Implement this partial method to configure the service endpoint.
        /// </summary>
        /// <param name="serviceEndpoint">The endpoint to configure</param>
        /// <param name="clientCredentials">The client credentials</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public UnitServiceClient() : 
                base(UnitServiceClient.GetDefaultBinding(), UnitServiceClient.GetDefaultEndpointAddress())
        {
            this.Endpoint.Name = EndpointConfiguration.BasicHttpBinding_IUnitService.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public UnitServiceClient(EndpointConfiguration endpointConfiguration) : 
                base(UnitServiceClient.GetBindingForEndpoint(endpointConfiguration), UnitServiceClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public UnitServiceClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(UnitServiceClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public UnitServiceClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(UnitServiceClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public UnitServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public System.Threading.Tasks.Task<UnitSvcReference.Project[]> GetProjectByUserAsync(string username)
        {
            return base.Channel.GetProjectByUserAsync(username);
        }
        
        public System.Threading.Tasks.Task<UnitSvcReference.PhaseBuilding[]> GetPhaseBuildingByProjectAsync(string username, string projectCode)
        {
            return base.Channel.GetPhaseBuildingByProjectAsync(username, projectCode);
        }
        
        public System.Threading.Tasks.Task<UnitSvcReference.Block[]> GetBlockByPhaseBuildingAsync(string username, string phaseBuildingCode)
        {
            return base.Channel.GetBlockByPhaseBuildingAsync(username, phaseBuildingCode);
        }
        
        public System.Threading.Tasks.Task<UnitSvcReference.Unit[]> GetUnitByBlockAsync(string username, string blockCode)
        {
            return base.Channel.GetUnitByBlockAsync(username, blockCode);
        }
        
        public System.Threading.Tasks.Task<UnitSvcReference.Unit[]> GetUnitByUserAsync(string username, string userRole, double latitude, double longitude)
        {
            return base.Channel.GetUnitByUserAsync(username, userRole, latitude, longitude);
        }
        
        public System.Threading.Tasks.Task<UnitSvcReference.Unit> GetUnitAsync(string username, string userRole, double latitude, double longitude, string referenceObject)
        {
            return base.Channel.GetUnitAsync(username, userRole, latitude, longitude, referenceObject);
        }
        
        public System.Threading.Tasks.Task<string[]> GetUnitFloorPlanAsync(string referenceObject)
        {
            return base.Channel.GetUnitFloorPlanAsync(referenceObject);
        }
        
        public System.Threading.Tasks.Task<UnitSvcReference.Unit[]> GetUnitByReferenceObjectsAsync(string[] referenceObject)
        {
            return base.Channel.GetUnitByReferenceObjectsAsync(referenceObject);
        }
        
        public System.Threading.Tasks.Task<UnitSvcReference.Unit[]> GetUnitByRefObjectAsync(string username, string referenceObject)
        {
            return base.Channel.GetUnitByRefObjectAsync(username, referenceObject);
        }
        
        public System.Threading.Tasks.Task<UnitSvcReference.Unit[]> GetNewUnitByUserAsync(string username)
        {
            return base.Channel.GetNewUnitByUserAsync(username);
        }
        
        public System.Threading.Tasks.Task<UnitSvcReference.Unit[]> GetNewUnitByProjectAsync(string username, string projectCode)
        {
            return base.Channel.GetNewUnitByProjectAsync(username, projectCode);
        }
        
        public System.Threading.Tasks.Task<UnitSvcReference.Unit[]> GetUnitByRefObjectByVendorAsync(string username, string referenceObject, string vendorCode)
        {
            return base.Channel.GetUnitByRefObjectByVendorAsync(username, referenceObject, vendorCode);
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
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IUnitService))
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
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IUnitService))
            {
                return new System.ServiceModel.EndpointAddress("http://localhost:9999/UnitService.svc");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding()
        {
            return UnitServiceClient.GetBindingForEndpoint(EndpointConfiguration.BasicHttpBinding_IUnitService);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
        {
            return UnitServiceClient.GetEndpointAddress(EndpointConfiguration.BasicHttpBinding_IUnitService);
        }
        
        public enum EndpointConfiguration
        {
            
            BasicHttpBinding_IUnitService,
        }
    }
}
