 
using System;
using System.Collections.Generic;
using System.Text;

namespace CTI.HI.Business.Entities
{
    public class DeviceInfoModel
    {
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public string Name { get; set; }
        public string VersionString { get; set; }
        public string Platform { get; set; }
        public string Idiom { get; set; }
        public DeviceType DeviceType { get; set; }
        public string Id { get; set; }

    }
}
