using Hcom.App.Core.Enums;
//using Hcom.App.Entities;
//using Hcom.App.Entities.HCOM;
//using Hcom.App.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hcom.Mock
{
    public static class MockData
    {
        /*
        public static Coordinates CurrentCoordinates()
        {
            return new Coordinates
            {

                Latitude = 14.182178,
                Longitude = 121.118163
            };



        }

        public static List<Address> GetNearbyUnits()
        {


            List<Address> output = new List<Address>();

            //foreach (var item in GetProjectByUser().ToList())
            //{
            //    output.Add(item.Address);
            //}

            foreach (var item in GetActualOnGoingConstructionsByProjectAsync("0000000056").ToList())
            {
                item.Unit.Address.Address1 = item.Unit.BlockFloorCode + ", " + item.Unit.LotUnitShareNumber + " of " + item.Unit.Address.Address1;
                output.Add(item.Unit.Address);
            }

            foreach (var item in GetActualOnGoingConstructionsByProjectAsync("0000000060").ToList())
            {
                item.Unit.Address.Address1 = item.Unit.BlockFloorCode + ", " + item.Unit.LotUnitShareNumber + " of " + item.Unit.Address.Address1;
                output.Add(item.Unit.Address);
            }

            return output;
        }
        public static List<Address> GetNearbyProjects()
        {


            List<Address> output = new List<Address>();

            foreach (var item in GetProjectByUser().ToList())
            {


                output.Add(item.Address);
            }



            return output;
        }
        public static IEnumerable<Project> GetProjectByUser()
        {

            return new List<Project>
            {
                new Project
                {

                    ProjectCode = "0000000056",
                    ShortName ="MNTB",
                    LongName = "MONTEBELLO",
                    ImageUrl = "https://filinvest.com/storage/imageable/project/9778d5d219c5080b9a6a17bef029331c/aspire-c1ca246ee5f59d2cf1e42b24b0d7ea0f.png",
                       Distance = 0,
                    Address = new Address
                    {

                        City = "Calamba",
                        Address1 = "Barangay Punta at Ciudad de Calamba",
                        Province = "Laguna",
                        PostalCode = "4027",
                        Country = "Philippines",
                        Latitude = 14.179893,
                        Longitude = 121.1217662,
                        AddressStatus = new AddressStatus { Description = "Not Visited", Icon = "montebello.png", Name = "Not Visited" }
                    }
                },
                new Project
                {
                    ProjectCode = "0000000060",
                    ShortName ="ALRE",
                    LongName = "ALDEA REAL",
                    ImageUrl = "https://filinvest.com/storage/imageable/project/8613985ec49eb8f757ae6439e879bb2a/futura-406214fb0c7fa63ae18dabb004124491.png",
                    Distance = 0,
                    Address = new Address
                    {
                        City = "Calamba",
                        Address1 = "Barangay Punta at Ciudad de Calamba",
                        Province = "Laguna",
                        PostalCode = "4027",
                        Country = "Philippines",
                        Latitude = 14.1831648,
                        Longitude = 121.1135387,
                        AddressStatus = new AddressStatus { Description = "Not Visited", Icon = "aldeareal.png", Name = "Not Visited" }

                    }
                },
                new Project
                {
                    ProjectCode = "0000000058",
                    ShortName ="PUAL",
                    LongName = "PUNTA ALTEZZA",
                    ImageUrl = "https://filinvest.com/storage/imageable/project/6974ce5ac660610b44d9b9fed0ff9548/futura-5d5c3a5fb6988b571beb70a6baf9882d.jpg",
                    Distance = 0,
                    Address = new Address
                    {
                        City = "Calamba",
                        Address1 = "Barangay Punta at Ciudad de Calamba",
                        Province = "Laguna",
                        PostalCode = "4027",
                        Country = "Philippines",
                        Latitude = 14.1862512,
                        Longitude = 121.1169383,

                        AddressStatus = new AddressStatus { Description = "Not Visited", Icon = "altezza.png", Name = "Not Visited" }
                    }
                }
            };

        }

        public static IEnumerable<ProjectDetail> GetProjectDetails()
        {

            var sourceData = new List<ProjectDetail>()
            {
               new  ProjectDetail { ProjectCode =  "0000000220", ProjectName = "Activa", Phase = "Flex" , Block = "Floor 9" , InventoryName = "9A" , UnitNumber = "0000000229" , ReferenceCode = "2018INVUNT0000009765" },
                new  ProjectDetail { ProjectCode =  "0000000220", ProjectName = "Activa", Phase = "Flex" , Block = "Floor 9" , InventoryName = "9B" , UnitNumber = "0000000230" , ReferenceCode = "2018INVUNT0000009766" },
                new  ProjectDetail { ProjectCode =  "0000000220", ProjectName = "Activa", Phase = "Flex" , Block = "Floor 9" , InventoryName = "9C" , UnitNumber = "0000000231" , ReferenceCode = "2018INVUNT0000009767" },
                new  ProjectDetail { ProjectCode =  "0000000220", ProjectName = "Activa", Phase = "Flex" , Block = "Floor 9" , InventoryName = "9D" , UnitNumber = "0000000232" , ReferenceCode = "2018INVUNT0000009768" },
                new  ProjectDetail { ProjectCode =  "0000000220", ProjectName = "Activa", Phase = "Flex" , Block = "Floor 8" , InventoryName = "8A" , UnitNumber = "0000000224" , ReferenceCode = "2018INVUNT0000009760" },
                new  ProjectDetail { ProjectCode =  "0000000220", ProjectName = "Activa", Phase = "Flex" , Block = "Floor 8" , InventoryName = "8B" , UnitNumber = "0000000225" , ReferenceCode = "2018INVUNT0000009761" },
                new  ProjectDetail { ProjectCode =  "0000000220", ProjectName = "Activa", Phase = "Flex" , Block = "Floor 8" , InventoryName = "8C" , UnitNumber = "0000000226" , ReferenceCode = "2018INVUNT0000009762" },
                new  ProjectDetail { ProjectCode =  "0000000220", ProjectName = "Activa", Phase = "Flex" , Block = "Floor 8" , InventoryName = "8D" , UnitNumber = "0000000227" , ReferenceCode = "2018INVUNT0000009763" },
                new  ProjectDetail { ProjectCode =  "0000000220", ProjectName = "Activa", Phase = "Flex" , Block = "Floor 7" , InventoryName = "7A" , UnitNumber = "0000000219" , ReferenceCode = "2018INVUNT0000009755" },
                new  ProjectDetail { ProjectCode =  "0000000220", ProjectName = "Activa", Phase = "Flex" , Block = "Floor 7" , InventoryName = "7B" , UnitNumber = "0000000220" , ReferenceCode = "2018INVUNT0000009756" },

                new  ProjectDetail { ProjectCode =  "0000000021", ProjectName = "Alta Vida", Phase = "PHASE III" , Block = "B9" , InventoryName = "L1" , UnitNumber = "0000000567" , ReferenceCode = "2009INVUNT0000012139" },
                new  ProjectDetail { ProjectCode =  "0000000021", ProjectName = "Alta Vida", Phase = "PHASE III" , Block = "B9" , InventoryName = "L2" , UnitNumber = "0000000568" , ReferenceCode = "2009INVUNT0000012140" },
                new  ProjectDetail { ProjectCode =  "0000000021", ProjectName = "Alta Vida", Phase = "PHASE III" , Block = "B9" , InventoryName = "L3" , UnitNumber = "0000000569" , ReferenceCode = "2009INVUNT0000012141" },
                new  ProjectDetail { ProjectCode =  "0000000021", ProjectName = "Alta Vida", Phase = "PHASE III" , Block = "B9" , InventoryName = "L4" , UnitNumber = "0000000570" , ReferenceCode = "2009INVUNT0000012142" },
                new  ProjectDetail { ProjectCode =  "0000000021", ProjectName = "Alta Vida", Phase = "PHASE III" , Block = "B9" , InventoryName = "L5" , UnitNumber = "0000000571" , ReferenceCode = "2009INVUNT0000012143" },
                new  ProjectDetail { ProjectCode =  "0000000021", ProjectName = "Alta Vida", Phase = "PHASE III" , Block = "B9" , InventoryName = "L6" , UnitNumber = "0000000572" , ReferenceCode = "2009INVUNT0000012144" },
                new  ProjectDetail { ProjectCode =  "0000000021", ProjectName = "Alta Vida", Phase = "PHASE III" , Block = "B9" , InventoryName = "L7" , UnitNumber = "0000000573" , ReferenceCode = "2009INVUNT0000012145" },
                new  ProjectDetail { ProjectCode =  "0000000021", ProjectName = "Alta Vida", Phase = "PHASE II" , Block = "B9" , InventoryName = "L1" , UnitNumber = "0000000193" , ReferenceCode = "2009INVUNT0000011767" },
                new  ProjectDetail { ProjectCode =  "0000000021", ProjectName = "Alta Vida", Phase = "PHASE II" , Block = "B9" , InventoryName = "L10" , UnitNumber = "0000000202" , ReferenceCode = "2009INVUNT0000011776" },
                new  ProjectDetail { ProjectCode =  "0000000021", ProjectName = "Alta Vida", Phase = "PHASE II" , Block = "B9" , InventoryName = "L2" , UnitNumber = "0000000237" , ReferenceCode = "2009INVUNT0000011810" },

                new  ProjectDetail { ProjectCode =  "0000000095", ProjectName = "One Oasis Cebu", Phase = "BUILDING 9" , Block = "Roof Deck" , InventoryName = "LC 9101" , UnitNumber = "0000003854" , ReferenceCode = "2016INVUNT0000005398" },
                new  ProjectDetail { ProjectCode =  "0000000095", ProjectName = "One Oasis Cebu", Phase = "BUILDING 9" , Block = "Roof Deck" , InventoryName = "LC 9102" , UnitNumber = "0000003855" , ReferenceCode = "2016INVUNT0000005399" },
                new  ProjectDetail { ProjectCode =  "0000000095", ProjectName = "One Oasis Cebu", Phase = "BUILDING 9" , Block = "Roof Deck" , InventoryName = "LC 9103" , UnitNumber = "0000003856" , ReferenceCode = "2016INVUNT0000005400" },
                new  ProjectDetail { ProjectCode =  "0000000095", ProjectName = "One Oasis Cebu", Phase = "BUILDING 9" , Block = "Roof Deck" , InventoryName = "LC 9104" , UnitNumber = "0000003857" , ReferenceCode = "2016INVUNT0000005401" },
                new  ProjectDetail { ProjectCode =  "0000000095", ProjectName = "One Oasis Cebu", Phase = "BUILDING 9" , Block = "Roof Deck" , InventoryName = "LC 9105" , UnitNumber = "0000003858" , ReferenceCode = "2016INVUNT0000005402" },
                new  ProjectDetail { ProjectCode =  "0000000095", ProjectName = "One Oasis Cebu", Phase = "BUILDING 9" , Block = "Roof Deck" , InventoryName = "LC 9106" , UnitNumber = "0000003859" , ReferenceCode = "2016INVUNT0000005403" },
                new  ProjectDetail { ProjectCode =  "0000000095", ProjectName = "One Oasis Cebu", Phase = "BUILDING 9" , Block = "Roof Deck" , InventoryName = "LC 9107" , UnitNumber = "0000003860" , ReferenceCode = "2016INVUNT0000005404" },
                new  ProjectDetail { ProjectCode =  "0000000095", ProjectName = "One Oasis Cebu", Phase = "BUILDING 9" , Block = "Roof Deck" , InventoryName = "LC 9108" , UnitNumber = "0000003861" , ReferenceCode = "2016INVUNT0000005405" },
                new  ProjectDetail { ProjectCode =  "0000000095", ProjectName = "One Oasis Cebu", Phase = "BUILDING 9" , Block = "Roof Deck" , InventoryName = "LC 9109" , UnitNumber = "0000003862" , ReferenceCode = "2016INVUNT0000005406" },
                new  ProjectDetail { ProjectCode =  "0000000095", ProjectName = "One Oasis Cebu", Phase = "BUILDING 9" , Block = "Roof Deck" , InventoryName = "LC 9110" , UnitNumber = "0000003863" , ReferenceCode = "2016INVUNT0000005407" },

                new  ProjectDetail { ProjectCode =  "0000000104", ProjectName = "One Oasis Davao", Phase = "BUILDING 4-STREET PARKING" , Block = "SP" , InventoryName = "PL75" , UnitNumber = "0000002189" , ReferenceCode = "2010INVUNT0000010543" },
                new  ProjectDetail { ProjectCode =  "0000000104", ProjectName = "One Oasis Davao", Phase = "BUILDING 4-STREET PARKING" , Block = "SP" , InventoryName = "PL76" , UnitNumber = "0000002190" , ReferenceCode = "2010INVUNT0000010544" },
                new  ProjectDetail { ProjectCode =  "0000000104", ProjectName = "One Oasis Davao", Phase = "BUILDING 4-STREET PARKING" , Block = "SP" , InventoryName = "PL77" , UnitNumber = "0000002191" , ReferenceCode = "2010INVUNT0000010545" },
                new  ProjectDetail { ProjectCode =  "0000000104", ProjectName = "One Oasis Davao", Phase = "BUILDING 4-STREET PARKING" , Block = "SP" , InventoryName = "PL78" , UnitNumber = "0000002192" , ReferenceCode = "2010INVUNT0000010546" },
                new  ProjectDetail { ProjectCode =  "0000000104", ProjectName = "One Oasis Davao", Phase = "BUILDING 4-STREET PARKING" , Block = "SP" , InventoryName = "PL79" , UnitNumber = "0000002193" , ReferenceCode = "2010INVUNT0000010547" },
                new  ProjectDetail { ProjectCode =  "0000000104", ProjectName = "One Oasis Davao", Phase = "BUILDING 4-STREET PARKING" , Block = "SP" , InventoryName = "PL80" , UnitNumber = "0000002194" , ReferenceCode = "2010INVUNT0000010548" },
                new  ProjectDetail { ProjectCode =  "0000000104", ProjectName = "One Oasis Davao", Phase = "BUILDING 4-STREET PARKING" , Block = "SP" , InventoryName = "PL81" , UnitNumber = "0000002195" , ReferenceCode = "2010INVUNT0000010549" },
                new  ProjectDetail { ProjectCode =  "0000000104", ProjectName = "One Oasis Davao", Phase = "BUILDING 4-STREET PARKING" , Block = "SP" , InventoryName = "PL82" , UnitNumber = "0000002196" , ReferenceCode = "2010INVUNT0000010550" },
                new  ProjectDetail { ProjectCode =  "0000000104", ProjectName = "One Oasis Davao", Phase = "BUILDING 4-STREET PARKING" , Block = "SP" , InventoryName = "PL83" , UnitNumber = "0000002197" , ReferenceCode = "2010INVUNT0000010551" },
                new  ProjectDetail { ProjectCode =  "0000000104", ProjectName = "One Oasis Davao", Phase = "BUILDING 4-STREET PARKING" , Block = "SP" , InventoryName = "PL84" , UnitNumber = "0000002198" , ReferenceCode = "2010INVUNT0000010552" },


                new  ProjectDetail { ProjectCode =  "0000000144", ProjectName = "The Vinia Residences", Phase = "Main Building" , Block = "Street Parking" , InventoryName = "SP1" , UnitNumber = "0000000503" , ReferenceCode = "2012INVUNT0000000785" },
                new  ProjectDetail { ProjectCode =  "0000000144", ProjectName = "The Vinia Residences", Phase = "Main Building" , Block = "Street Parking" , InventoryName = "SP2" , UnitNumber = "0000000504" , ReferenceCode = "2012INVUNT0000000786" },
                new  ProjectDetail { ProjectCode =  "0000000144", ProjectName = "The Vinia Residences", Phase = "Main Building" , Block = "Street Parking" , InventoryName = "SP3" , UnitNumber = "0000000505" , ReferenceCode = "2012INVUNT0000000787" },
                new  ProjectDetail { ProjectCode =  "0000000144", ProjectName = "The Vinia Residences", Phase = "Main Building" , Block = "Street Parking" , InventoryName = "SP4" , UnitNumber = "0000000506" , ReferenceCode = "2012INVUNT0000000788" },
                new  ProjectDetail { ProjectCode =  "0000000144", ProjectName = "The Vinia Residences", Phase = "Main Building" , Block = "Street Parking" , InventoryName = "SP5" , UnitNumber = "0000000507" , ReferenceCode = "2012INVUNT0000000789" },
                new  ProjectDetail { ProjectCode =  "0000000144", ProjectName = "The Vinia Residences", Phase = "Main Building" , Block = "Street Parking" , InventoryName = "SP6" , UnitNumber = "0000000508" , ReferenceCode = "2012INVUNT0000000790" },
                new  ProjectDetail { ProjectCode =  "0000000144", ProjectName = "The Vinia Residences", Phase = "Main Building" , Block = "Street Parking" , InventoryName = "SP7" , UnitNumber = "0000000509" , ReferenceCode = "2012INVUNT0000000791" },
                new  ProjectDetail { ProjectCode =  "0000000144", ProjectName = "The Vinia Residences", Phase = "Main Building" , Block = "Street Parking" , InventoryName = "SP8" , UnitNumber = "0000000510" , ReferenceCode = "2012INVUNT0000000792" },
                new  ProjectDetail { ProjectCode =  "0000000144", ProjectName = "The Vinia Residences", Phase = "Main Building" , Block = "Street Parking" , InventoryName = "SP9" , UnitNumber = "0000000511" , ReferenceCode = "2012INVUNT0000000793" },
                new  ProjectDetail { ProjectCode =  "0000000144", ProjectName = "The Vinia Residences", Phase = "Main Building" , Block = "Street Parking" , InventoryName = "SP10" , UnitNumber = "0000000512" , ReferenceCode = "2012INVUNT0000000794" },

                        };


            return sourceData;



        }


        public static IEnumerable<ActualOnGoingConstruction> GetActualOnGoingConstructionsByProjectAsync(string projectCode)
        {
            //Montebello
            if (projectCode == "0000000056")
            {

                return new List<ActualOnGoingConstruction>()
                {
                    //1st House Shell Finish
                    new ActualOnGoingConstruction
                    {
                        ContractorNumber = "2019CONNUM0000000072",
                        ManagingContractorID = 1129,
                        OTCNumber = "2019OTCNUM0000000097",
                        ReferenceObject = "2009INVUNT0000035075",

                        Unit = new Unit
                        {
                            ReferenceObject = "2009INVUNT0000035075",
                            ProjectCode = "0000000056",
                            Project = new Project()
                            {
                                 ProjectCode = "0000000056",
                                 ShortName = "MNTB",
                                 LongName = "MONTEBELLO"
                            },
                            PhaseBuildingCode = "0000000161",
                            PhaseBuilding = new PhaseBuilding()
                            {
                                Id = "0000000161",
                                ShortName = "D-511200",
                                LongName = "PHASE II"
                            },
                            BlockFloorCode = "0000002057",
                            BlockFloor = new BlockFloorCluster
                            {
                                Id = "0000002057",
                                ShortName = "B4C",
                                LongName = "B4C"
                            },
                            LotUnitShareNumber = "L10",
                            InventoryUnitNumber = "0000000001",
                            Address = new Address
                            {
                                City = "Calamba",
                                Address1 = "Barangay Punta at Ciudad de Calamba",
                                Province = "Laguna",
                                PostalCode = "4027",
                                Country = "Philippines",
                                Latitude = 14.1801373,
                                Longitude = 121.1215108,
                                 AddressStatus = new AddressStatus { Description = "Inspected", Icon = "GreenMarker.png", Name = "Inspected" }
                            }
                        },
                        OTCTypeCode = "BSSM",
                        ManagingContractor = "GENCON",
                        ContractorCode = "0000101276",
                        ConstructionStart = new DateTime(2018,2,15),
                        ConstructionEnd = new DateTime(2018,08,14),
                        ContractorPercentageCompletion = 97,
                    },
                    new ActualOnGoingConstruction
                    {
                        ContractorNumber = "2019CONNUM0000000073",
                        ManagingContractorID = 1130,
                        OTCNumber = "2019OTCNUM0000000098",
                        ReferenceObject = "2009INVUNT0000035075",
                        Unit = new Unit
                        {
                            ReferenceObject = "2009INVUNT0000035075",
                            ProjectCode = "0000000056",
                            Project = new Project()
                            {
                                 ProjectCode = "0000000056",
                                 ShortName = "MNTB",
                                 LongName = "MONTEBELLO"
                            },
                            PhaseBuildingCode = "0000000161",
                            PhaseBuilding = new PhaseBuilding()
                            {
                                Id = "0000000161",
                                ShortName = "D-511200",
                                LongName = "PHASE II"
                            },
                            BlockFloorCode = "0000002057",
                            BlockFloor = new BlockFloorCluster
                            {
                                Id = "0000002057",
                                ShortName = "B4C",
                                LongName = "B4C"
                            },
                            LotUnitShareNumber = "L10",
                            InventoryUnitNumber = "0000000001",
                            Address = new Address
                            {
                                City = "Calamba",
                                Address1 = "Barangay Punta at Ciudad de Calamba",
                                Province = "Laguna",
                                PostalCode = "4027",
                                Country = "Philippines",
                                Latitude = 14.1801373,
                                Longitude = 121.1215108,
                                 AddressStatus = new AddressStatus { Description = "Inspected", Icon = "GreenMarker.png", Name = "Inspected" }
                            }
                        },
                        OTCTypeCode = "OSSF",
                        ManagingContractor = "GENCON",
                        ContractorCode = "0000101277",
                        ConstructionStart = new DateTime(2018,08,24),
                        ConstructionEnd = new DateTime(2019,03,14),
                        ContractorPercentageCompletion = 0
                    },

                
                    //2nd House Shell Finish
                    new ActualOnGoingConstruction
                    {
                        ContractorNumber = "2019CONNUM0000000081",
                        ManagingContractorID = 1138,
                        OTCNumber = "2019OTCNUM0000000110",
                        ReferenceObject = "2009INVUNT0000035168",
                        Unit = new Unit
                        {
                            ReferenceObject = "2009INVUNT0000035168",
                            ProjectCode = "0000000056",
                            Project = new Project()
                            {
                                 ProjectCode = "0000000056",
                                 ShortName = "MNTB",
                                 LongName = "MONTEBELLO"
                            },
                            PhaseBuildingCode = "0000000161",
                            PhaseBuilding = new PhaseBuilding()
                            {
                                Id = "0000000161",
                                ShortName = "D-511200",
                                LongName = "PHASE II"
                            },
                            BlockFloorCode = "0000002063",
                            BlockFloor = new BlockFloorCluster
                            {
                                Id = "0000002063",
                                ShortName = "B6C",
                                LongName = "B6C"
                            },
                            LotUnitShareNumber = "L13",
                            InventoryUnitNumber = "0000000002",
                            Address = new Address
                            {
                                City = "Calamba",
                                Address1 = "Barangay Punta at Ciudad de Calamba",
                                Province = "Laguna",
                                PostalCode = "4027",
                                Country = "Philippines",
                                Latitude = 14.1800203,
                                Longitude = 121.1217552,
                                AddressStatus = new AddressStatus { Description = "Inspected", Icon = "GreenMarker.png", Name = "Inspected" }
                            }
                        },
                        OTCTypeCode = "BSSM",
                        ManagingContractor = "GENCON",
                        ContractorCode = "0000101278",
                        ConstructionStart = new DateTime(2018,2,15),
                        ConstructionEnd = new DateTime(2018,08,14),
                        ContractorPercentageCompletion = 45
                    },
                    new ActualOnGoingConstruction
                    {
                        ContractorNumber = "2019CONNUM0000000082",
                        ManagingContractorID = 1139,
                        OTCNumber = "2019OTCNUM0000000111",
                        ReferenceObject = "2009INVUNT0000035168",
                       Unit = new Unit
                        {
                            ReferenceObject = "2009INVUNT0000035168",
                            ProjectCode = "0000000056",
                            Project = new Project()
                            {
                                 ProjectCode = "0000000056",
                                 ShortName = "MNTB",
                                 LongName = "MONTEBELLO"
                            },
                            PhaseBuildingCode = "0000000161",
                            PhaseBuilding = new PhaseBuilding()
                            {
                                Id = "0000000161",
                                ShortName = "D-511200",
                                LongName = "PHASE II"
                            },
                            BlockFloorCode = "0000002063",
                            BlockFloor = new BlockFloorCluster
                            {
                                Id = "0000002063",
                                ShortName = "B6C",
                                LongName = "B6C"
                            },
                            LotUnitShareNumber = "L13",
                            InventoryUnitNumber = "0000000002",
                            Address = new Address
                            {
                                City = "Calamba",
                                Address1 = "Barangay Punta at Ciudad de Calamba",
                                Province = "Laguna",
                                PostalCode = "4027",
                                Country = "Philippines",
                                Latitude = 14.1800203,
                                Longitude = 121.1217552,
                                AddressStatus = new AddressStatus { Description = "Inspected", Icon = "GreenMarker.png", Name = "Inspected" }
                            }
                        },
                        OTCTypeCode = "OSSF",
                        ManagingContractor = "GENCON",
                        ContractorCode = "0000101279",
                        ConstructionStart = new DateTime(2018,08,24),
                        ConstructionEnd = new DateTime(2019,03,14),
                        ContractorPercentageCompletion = 0
                    },

                    
                    //3rd House Complete
                    new ActualOnGoingConstruction
                    {
                        ContractorNumber = "2019CONNUM0000000102",
                        ManagingContractorID = 1140,
                        OTCNumber = "2019OTCNUM0000000134",
                        ReferenceObject = "2009INVUNT0000035018",
                        Unit = new Unit
                        {
                            ReferenceObject = "2009INVUNT0000035018",
                            ProjectCode = "0000000056",
                            Project = new Project()
                            {
                                 ProjectCode = "0000000056",
                                 ShortName = "MNTB",
                                 LongName = "MONTEBELLO"
                            },
                            PhaseBuildingCode = "0000000160",
                            PhaseBuilding = new PhaseBuilding()
                            {
                                Id = "0000000160",
                                ShortName = "D-511199",
                                LongName = "PHASE I"
                            },
                            BlockFloorCode = "0000002039",
                            BlockFloor = new BlockFloorCluster
                            {
                                Id = "0000002039",
                                ShortName = "B1C",
                                LongName = "B1C"
                            },
                            LotUnitShareNumber = "L6",
                            InventoryUnitNumber = "0000000003",
                            Address = new Address
                            {
                                City = "Calamba",
                                Address1 = "Barangay Punta at Ciudad de Calamba",
                                Province = "Laguna",
                                PostalCode = "4027",
                                Country = "Philippines",
                                Latitude = 14.1797403,
                                Longitude = 121.1200568,
                                 AddressStatus = new AddressStatus { Description = "Inspected", Icon = "GreenMarker.png", Name = "Inspected" }

                            }
                        },
                        OTCTypeCode = "OSCM",
                        ManagingContractor = "GENCON",
                        ContractorCode = "0000101288",
                        ConstructionStart = new DateTime(2018,2,15),
                        ConstructionEnd = new DateTime(2018,08,14),
                        ContractorPercentageCompletion = 45
                    }
                };

            }
            else if (projectCode == "0000000060")
            {

                return new List<ActualOnGoingConstruction>()
                {
                    //1st House Shell Finish
                    new ActualOnGoingConstruction
                    {
                        ContractorNumber = "2019CONNUM0000001261",
                        ManagingContractorID = 2318,
                        OTCNumber = "2019OTCNUM0000003143",
                        ReferenceObject = "2009INVUNT0000036819",
                        Unit = new Unit
                        {
                            ReferenceObject = "2009INVUNT0000036819",
                            ProjectCode = "0000000060",
                            Project = new Project()
                            {
                                 ProjectCode = "0000000060",
                                 ShortName = "ALRE",
                                 LongName = "ALDEA REAL"
                            },
                            PhaseBuildingCode = "0000000169",
                            PhaseBuilding = new PhaseBuilding()
                            {
                                 Id = "0000000169",
                                 ShortName = "ALRE2",
                                 LongName = "PHASE II"
                            },
                            BlockFloorCode = "0000002160",
                            BlockFloor = new BlockFloorCluster()
                            {
                                 Id = "0000000169",
                                 ShortName = "B2",
                                 LongName = "B2"
                            },
                            LotUnitShareNumber = "L6",
                            InventoryUnitNumber = "0000000001",
                            Address = new Address
                            {
                                City = "Calamba",
                                Address1 = "Barangay Punta at Ciudad de Calamba",
                                Province = "Laguna",
                                PostalCode = "4027",
                                Country = "Philippines",
                                Latitude = 14.1824973,
                                Longitude = 121.1131778,
                                 AddressStatus = new AddressStatus { Description = "Inspected", Icon = "GreenMarker.png", Name = "Inspected" }
                            }
                        },
                        OTCTypeCode = "BSSM",
                        ManagingContractor = "GENCON",
                        ContractorCode = "0000101376",
                        ConstructionStart = new DateTime(2018,2,15),
                        ConstructionEnd = new DateTime(2018,08,14),
                        ContractorPercentageCompletion = 45
                    },
                   new ActualOnGoingConstruction
                    {
                        ContractorNumber = "2019CONNUM0000001262",
                        ManagingContractorID = 2319,
                        OTCNumber = "2019OTCNUM0000003143",
                        ReferenceObject = "2009INVUNT0000036819",
                         Unit = new Unit
                        {
                            ReferenceObject = "2009INVUNT0000036819",
                            ProjectCode = "0000000060",
                            Project = new Project()
                            {
                                 ProjectCode = "0000000060",
                                 ShortName = "ALRE",
                                 LongName = "ALDEA REAL"
                            },
                            PhaseBuildingCode = "0000000169",
                            PhaseBuilding = new PhaseBuilding()
                            {
                                 Id = "0000000169",
                                 ShortName = "ALRE2",
                                 LongName = "PHASE II"
                            },
                            BlockFloorCode = "0000002160",
                            BlockFloor = new BlockFloorCluster()
                            {
                                 Id = "0000000169",
                                 ShortName = "B2",
                                 LongName = "B2"
                            },
                            LotUnitShareNumber = "L6",
                            InventoryUnitNumber = "0000000001",
                            Address = new Address
                            {
                                City = "Calamba",
                                Address1 = "Barangay Punta at Ciudad de Calamba",
                                Province = "Laguna",
                                PostalCode = "4027",
                                Country = "Philippines",
                                Latitude = 14.1824973,
                                Longitude = 121.1131778,
                                 AddressStatus = new AddressStatus { Description = "Inspected", Icon = "GreenMarker.png", Name = "Inspected" }
                            }
                        },
                        OTCTypeCode = "BSSM",
                        ManagingContractor = "GENCON",
                        ContractorCode = "0000101377",
                        ConstructionStart = new DateTime(2018,2,15),
                        ConstructionEnd = new DateTime(2018,08,14),
                        ContractorPercentageCompletion = 0
                    },

                
                    //2nd House Complete
                    new ActualOnGoingConstruction
                    {
                        ContractorNumber = "2019CONNUM0000001361",
                        ManagingContractorID = 2418,
                        OTCNumber = "2019OTCNUM0000003777",
                        ReferenceObject = "2009INVUNT0000036819",
                        Unit = new Unit
                        {
                            ReferenceObject = "2009INVUNT0000036819",
                            ProjectCode = "0000000060",
                            Project = new Project()
                            {
                                 ProjectCode = "0000000060",
                                 ShortName = "ALRE",
                                 LongName = "ALDEA REAL"
                            },
                            PhaseBuildingCode = "0000000169",
                            PhaseBuilding = new PhaseBuilding()
                            {
                                 Id = "0000000169",
                                 ShortName = "ALRE2",
                                 LongName = "PHASE II"
                            },
                            BlockFloorCode = "0000002160",
                            BlockFloor = new BlockFloorCluster()
                            {
                                 Id = "0000002160",
                                 ShortName = "B12",
                                 LongName = "B12"
                            },
                            LotUnitShareNumber = "L6",
                            InventoryUnitNumber = "0000000005",
                            Address = new Address
                            {
                                City = "Calamba",
                                Address1 = "Barangay Punta at Ciudad de Calamba",
                                Province = "Laguna",
                                PostalCode = "4027",
                                Country = "Philippines",
                                Latitude = 14.1822173,
                                Longitude = 121.1133718,
                                AddressStatus = new AddressStatus { Description = "Inspected", Icon = "GreenMarker.png", Name = "Inspected" }
                            }
                        },
                        OTCTypeCode = "BSSM",
                        ManagingContractor = "GENCON",
                        ContractorCode = "0000101379",
                        ConstructionStart = new DateTime(2018,2,15),
                        ConstructionEnd = new DateTime(2018,08,14),
                        ContractorPercentageCompletion = 15
                    },
                    
                    //3rd House Complete
                    new ActualOnGoingConstruction
                    {
                        ContractorNumber = "2019CONNUM0000001596",
                        ManagingContractorID = 2653,
                        OTCNumber = "2019OTCNUM0000004090",
                        ReferenceObject = "2011INVUNT0000008864",
                        Unit = new Unit
                        {
                            ReferenceObject = "2011INVUNT0000008864",
                            ProjectCode = "0000000060",
                            Project = new Project()
                            {
                                 ProjectCode = "0000000060",
                                 ShortName = "ALRE",
                                 LongName = "ALDEA REAL"
                            },
                            PhaseBuildingCode = "0000000410",
                            PhaseBuilding = new PhaseBuilding()
                            {
                                 Id = "0000000410",
                                 ShortName = "ALRE4",
                                 LongName = "PHASE IV"
                            },
                            BlockFloorCode = "0000005974",
                            BlockFloor = new BlockFloorCluster()
                            {
                                 Id = "0000005974",
                                 ShortName = "B6",
                                 LongName = "B6"
                            },
                            LotUnitShareNumber = "Lot 2",
                            InventoryUnitNumber = "0000000006",
                            Address = new Address
                            {
                                City = "Calamba",
                                Address1 = "Barangay Punta at Ciudad de Calamba",
                                Province = "Laguna",
                                PostalCode = "4027",
                                Country = "Philippines",
                                Latitude = 14.1822303,
                                Longitude = 121.1127198,
                                 AddressStatus = new AddressStatus { Description = "Inspected", Icon = "RedMarker.png", Name = "Inspected" }

                            }
                        },
                        OTCTypeCode = "OSCM",
                        ManagingContractor = "GENCON",
                        ContractorCode = "0000101389",
                        ConstructionStart = new DateTime(2018,08,24),
                        ConstructionEnd = new DateTime(2019,03,14),
                        ContractorPercentageCompletion = 56
                    },

                    
                    //4th House Complete
                    new ActualOnGoingConstruction
                    {
                        ContractorNumber = "2019CONNUM0000001318",
                        ManagingContractorID = 1138,
                        OTCNumber = "2019OTCNUM0000003409",
                        ReferenceObject = "2011INVUNT0000008864",
                        Unit = new Unit
                        {
                            ReferenceObject = "2011INVUNT0000008864",
                            ProjectCode = "0000000060",
                            Project = new Project()
                            {
                                 ProjectCode = "0000000060",
                                 ShortName = "ALRE",
                                 LongName = "ALDEA REAL"
                            },
                            PhaseBuildingCode = "0000000410",
                            PhaseBuilding = new PhaseBuilding()
                            {
                                 Id = "0000000410",
                                 ShortName = "ALRE4",
                                 LongName = "PHASE IV"
                            },
                            BlockFloorCode = "0000005974",
                            BlockFloor = new BlockFloorCluster()
                            {
                                 Id = "0000005974",
                                 ShortName = "B42",
                                 LongName = "B42"
                            },
                            LotUnitShareNumber = "Lot 2",
                            InventoryUnitNumber = "0000000007",
                            Address = new Address
                            {
                                City = "Calamba",
                                Address1 = "Barangay Punta at Ciudad de Calamba",
                                Province = "Laguna",
                                PostalCode = "4027",
                                Country = "Philippines",
                                Latitude = 14.1838593,
                                Longitude = 121.1142774,
                                 AddressStatus = new AddressStatus { Description = "Inspected", Icon = "RedMarker.png", Name = "Inspected" }
                            }
                        },
                        OTCTypeCode = "OSCM",
                        ManagingContractor = "GENCON",
                        ContractorCode = "0000101289",
                        ConstructionStart = new DateTime(2018,2,15),
                        ConstructionEnd = new DateTime(2018,08,14),
                        ContractorPercentageCompletion = 12
                    }
                };
            }
            else
            {
                return null;
            }
        }

        public static ActualOnGoingConstruction GetActualOnGoingConstructionsAsync(string contractorNumber, string otcNumber, int managingId)
        {

            return new ActualOnGoingConstruction
            {
                ContractorNumber = "2019CONNUM0000001261",
                ManagingContractorID = 2318,
                OTCNumber = "2019OTCNUM0000003143",
                ReferenceObject = "2009INVUNT0000036819",
                Unit = new Unit
                {
                    ReferenceObject = "2009INVUNT0000036819",
                    ProjectCode = "0000000060",
                    Project = new Project()
                    {
                        ProjectCode = "0000000060",
                        ShortName = "ALRE",
                        LongName = "ALDEA REAL"
                    },
                    PhaseBuildingCode = "0000000169",
                    PhaseBuilding = new PhaseBuilding()
                    {
                        Id = "0000000169",
                        ShortName = "ALRE2",
                        LongName = "PHASE II"
                    },
                    BlockFloorCode = "0000002160",
                    BlockFloor = new BlockFloorCluster()
                    {
                        Id = "0000000169",
                        ShortName = "B2",
                        LongName = "B2"
                    },
                    LotUnitShareNumber = "L6",
                    InventoryUnitNumber = "0000000001",
                    Address = new Address
                    {
                        City = "Calamba",
                        Address1 = "Barangay Punta at Ciudad de Calamba",
                        Province = "Laguna",
                        PostalCode = "4027",
                        Country = "Philippines",
                        Latitude = 14.1824973,
                        Longitude = 121.1131778,
                        AddressStatus = new AddressStatus { Description = "Inspected", Icon = "GreenMarker.png", Name = "Inspected" }
                    }
                },
                OTCTypeCode = "BSSM",
                ManagingContractor = "GENCON",
                ContractorCode = "0000101376",
                Contractor = new Contractor
                {
                    Name = "Dream Builders"
                },
                Representative = new List<Representative>
                {
                    new Representative
                    {
                         Name = "JJ",
                         ContactNumber = "+639164785047"
                    }
                },
                ConstructionStart = new DateTime(2018, 2, 15),
                ConstructionEnd = new DateTime(2018, 08, 14),
                ContractorPercentageCompletion = 45
            };
        }

        public static IEnumerable<ActualOnGoingConstructionMilestone> GetActualOnGoingConstructionsMilestoneAsync(string otcNumber, string contractorNumber, int managingContractorID)
        {
            //return new List<ActualOnGoingConstructionMilestone>();

            var list = new List<ActualOnGoingConstructionMilestone>();
            //OTC 1
            list.Add(new ActualOnGoingConstructionMilestone
            {
                OTCNumber = "2019OTCNUM0000000097",
                ContractorNumber = "2019CONNUM0000000072",
                ManagingContractorID = 1129,
                ConstructionMilestoneCode = "00001",
                ConstructionMilestoneDescription = "Hoarding Fence",
                Weight = 25,
                Sequence = 1,
                PercentageCompletion = 80,
                PercentageReferenceNumber = "2018CONMIL0000000001",
                PONumber = "PO 654321"
            });
            list.Add(new ActualOnGoingConstructionMilestone
            {
                OTCNumber = "2019OTCNUM0000000097",
                ContractorNumber = "2019CONNUM0000000072",
                ManagingContractorID = 1129,
                ConstructionMilestoneCode = "00002",
                ConstructionMilestoneDescription = "Concreting-Footing (w/ 7D Concrete Test)",
                Weight = 25,
                Sequence = 2,
                PercentageCompletion = 20,
                PercentageReferenceNumber = "2018CONMIL0000000002",
                PONumber = "PO 654321"
            });
            list.Add(new ActualOnGoingConstructionMilestone
            {
                OTCNumber = "2019OTCNUM0000000097",
                ContractorNumber = "2019CONNUM0000000072",
                ManagingContractorID = 1129,
                ConstructionMilestoneCode = "00003",
                ConstructionMilestoneDescription = "Concrete Test-Footing @ 28 days",
                Weight = 25,
                Sequence = 1,
                PercentageCompletion = 0,
                PercentageReferenceNumber = "2018CONMIL0000000003",
                PONumber = "PO 654321"
            });
            list.Add(new ActualOnGoingConstructionMilestone
            {
                OTCNumber = "2019OTCNUM0000000097",
                ContractorNumber = "2019CONNUM0000000072",
                ManagingContractorID = 1129,
                ConstructionMilestoneCode = "00004",
                ConstructionMilestoneDescription = "Rough Ins-Drainage GF",
                Weight = 25,
                Sequence = 2,
                PercentageCompletion = 20,
                PercentageReferenceNumber = "2018CONMIL0000000004",
                PONumber = "PO 654321"
            });

            //OTC 2
            list.Add(new ActualOnGoingConstructionMilestone
            {
                OTCNumber = "2019OTCNUM0000000098",
                ContractorNumber = "2019CONNUM0000000073",
                ManagingContractorID = 1130,
                ConstructionMilestoneCode = "00005",
                ConstructionMilestoneDescription = "Rough Ins-Sewer Line GF",
                Weight = 25,
                Sequence = 1,
                PercentageCompletion = 80,
                PercentageReferenceNumber = "2018CONMIL0000000005",
                PONumber = "PO 654322"
            });
            list.Add(new ActualOnGoingConstructionMilestone
            {
                OTCNumber = "2019OTCNUM0000000098",
                ContractorNumber = "2019CONNUM0000000073",
                ManagingContractorID = 1130,
                ConstructionMilestoneCode = "00006",
                ConstructionMilestoneDescription = "Rough Ins-Water Line GF",
                Weight = 25,
                Sequence = 2,
                PercentageCompletion = 20,
                PercentageReferenceNumber = "2018CONMIL0000000006",
                PONumber = "PO 654322"
            });
            list.Add(new ActualOnGoingConstructionMilestone
            {
                OTCNumber = "2019OTCNUM0000000098",
                ContractorNumber = "2019CONNUM0000000073",
                ManagingContractorID = 1130,
                ConstructionMilestoneCode = "00007",
                ConstructionMilestoneDescription = "Rough Ins-Electrical GF",
                Weight = 25,
                Sequence = 1,
                PercentageCompletion = 0,
                PercentageReferenceNumber = "2018CONMIL0000000007",
                PONumber = "PO 654322"
            });
            list.Add(new ActualOnGoingConstructionMilestone
            {
                OTCNumber = "2019OTCNUM0000000098",
                ContractorNumber = "2019CONNUM0000000073",
                ManagingContractorID = 1130,
                ConstructionMilestoneCode = "00008",
                ConstructionMilestoneDescription = "Rough Ins-Vent line",
                Weight = 25,
                Sequence = 2,
                PercentageCompletion = 20,
                PercentageReferenceNumber = "2018CONMIL0000000008",
                PONumber = "PO 654322"
            });

            //OTC 3
            list.Add(new ActualOnGoingConstructionMilestone
            {
                OTCNumber = "2019OTCNUM0000000110",
                ContractorNumber = "2019CONNUM0000000081",
                ManagingContractorID = 1138,
                ConstructionMilestoneCode = "00001",
                ConstructionMilestoneDescription = "Hoarding Fence",
                Weight = 25,
                Sequence = 1,
                PercentageCompletion = 80,
                PercentageReferenceNumber = "2018CONMIL0000000001",
                PONumber = "PO 654321"
            });
            list.Add(new ActualOnGoingConstructionMilestone
            {
                OTCNumber = "2019OTCNUM0000000110",
                ContractorNumber = "2019CONNUM0000000081",
                ManagingContractorID = 1138,
                ConstructionMilestoneCode = "00002",
                ConstructionMilestoneDescription = "Concreting-Footing (w/ 7D Concrete Test)",
                Weight = 25,
                Sequence = 2,
                PercentageCompletion = 20,
                PercentageReferenceNumber = "2018CONMIL0000000002",
                PONumber = "PO 654321"
            });
            list.Add(new ActualOnGoingConstructionMilestone
            {
                OTCNumber = "2019OTCNUM0000000110",
                ContractorNumber = "2019CONNUM0000000081",
                ManagingContractorID = 1138,
                ConstructionMilestoneCode = "00003",
                ConstructionMilestoneDescription = "Concrete Test-Footing @ 28 days",
                Weight = 25,
                Sequence = 1,
                PercentageCompletion = 0,
                PercentageReferenceNumber = "2018CONMIL0000000003",
                PONumber = "PO 654321"
            });
            list.Add(new ActualOnGoingConstructionMilestone
            {
                OTCNumber = "2019OTCNUM0000000110",
                ContractorNumber = "2019CONNUM0000000081",
                ManagingContractorID = 1138,
                ConstructionMilestoneCode = "00004",
                ConstructionMilestoneDescription = "Rough Ins-Drainage GF",
                Weight = 25,
                Sequence = 2,
                PercentageCompletion = 20,
                PercentageReferenceNumber = "2018CONMIL0000000004",
                PONumber = "PO 654321"
            });

            //OTC 4
            list.Add(new ActualOnGoingConstructionMilestone
            {
                OTCNumber = "2019OTCNUM0000000111",
                ContractorNumber = "2019CONNUM0000000082",
                ManagingContractorID = 1139,
                ConstructionMilestoneCode = "00005",
                ConstructionMilestoneDescription = "Rough Ins-Sewer Line GF",
                Weight = 25,
                Sequence = 1,
                PercentageCompletion = 80,
                PercentageReferenceNumber = "2018CONMIL0000000005",
                PONumber = "PO 654322"
            });
            list.Add(new ActualOnGoingConstructionMilestone
            {
                OTCNumber = "2019OTCNUM0000000111",
                ContractorNumber = "2019CONNUM0000000082",
                ManagingContractorID = 1139,
                ConstructionMilestoneCode = "00006",
                ConstructionMilestoneDescription = "Rough Ins-Water Line GF",
                Weight = 25,
                Sequence = 2,
                PercentageCompletion = 20,
                PercentageReferenceNumber = "2018CONMIL0000000006",
                PONumber = "PO 654322"
            });
            list.Add(new ActualOnGoingConstructionMilestone
            {
                OTCNumber = "2019OTCNUM0000000111",
                ContractorNumber = "2019CONNUM0000000082",
                ManagingContractorID = 1139,
                ConstructionMilestoneCode = "00007",
                ConstructionMilestoneDescription = "Rough Ins-Electrical GF",
                Weight = 25,
                Sequence = 1,
                PercentageCompletion = 0,
                PercentageReferenceNumber = "2018CONMIL0000000007",
                PONumber = "PO 654322"
            });
            list.Add(new ActualOnGoingConstructionMilestone
            {
                OTCNumber = "2019OTCNUM0000000111",
                ContractorNumber = "2019CONNUM0000000082",
                ManagingContractorID = 1139,
                ConstructionMilestoneCode = "00008",
                ConstructionMilestoneDescription = "Rough Ins-Vent line",
                Weight = 25,
                Sequence = 2,
                PercentageCompletion = 20,
                PercentageReferenceNumber = "2018CONMIL0000000008",
                PONumber = "PO 654322"
            });

            //OTC 5
            list.Add(new ActualOnGoingConstructionMilestone
            {
                OTCNumber = "2019OTCNUM0000000134",
                ContractorNumber = "2019CONNUM0000000102",
                ManagingContractorID = 1140,
                ConstructionMilestoneCode = "00001",
                ConstructionMilestoneDescription = "Hoarding Fence",
                Weight = 25,
                Sequence = 1,
                PercentageCompletion = 80,
                PercentageReferenceNumber = "2018CONMIL0000000001",
                PONumber = "PO 654321"
            });
            list.Add(new ActualOnGoingConstructionMilestone
            {
                OTCNumber = "2019OTCNUM0000000134",
                ContractorNumber = "2019CONNUM0000000102",
                ManagingContractorID = 1140,
                ConstructionMilestoneCode = "00002",
                ConstructionMilestoneDescription = "Concreting-Footing (w/ 7D Concrete Test)",
                Weight = 25,
                Sequence = 2,
                PercentageCompletion = 20,
                PercentageReferenceNumber = "2018CONMIL0000000002",
                PONumber = "PO 654321"
            });
            list.Add(new ActualOnGoingConstructionMilestone
            {
                OTCNumber = "2019OTCNUM0000000134",
                ContractorNumber = "2019CONNUM0000000102",
                ManagingContractorID = 1140,
                ConstructionMilestoneCode = "00003",
                ConstructionMilestoneDescription = "Concrete Test-Footing @ 28 days",
                Weight = 25,
                Sequence = 1,
                PercentageCompletion = 0,
                PercentageReferenceNumber = "2018CONMIL0000000003",
                PONumber = "PO 654321"
            });
            list.Add(new ActualOnGoingConstructionMilestone
            {
                OTCNumber = "2019OTCNUM0000000134",
                ContractorNumber = "2019CONNUM0000000102",
                ManagingContractorID = 1140,
                ConstructionMilestoneCode = "00004",
                ConstructionMilestoneDescription = "Rough Ins-Drainage GF",
                Weight = 25,
                Sequence = 2,
                PercentageCompletion = 20,
                PercentageReferenceNumber = "2018CONMIL0000000004",
                PONumber = "PO 654321"
            });

            //OTC 6
            list.Add(new ActualOnGoingConstructionMilestone
            {
                OTCNumber = "2019OTCNUM0000003143",
                ContractorNumber = "2019CONNUM0000001261",
                ManagingContractorID = 2318,
                ConstructionMilestoneCode = "00001",
                ConstructionMilestoneDescription = "Hoarding Fence",
                Weight = 25,
                Sequence = 1,
                PercentageCompletion = 80,
                PercentageReferenceNumber = "2018CONMIL0000000001",
                PONumber = "PO 654321"
            });
            list.Add(new ActualOnGoingConstructionMilestone
            {
                OTCNumber = "2019OTCNUM0000003143",
                ContractorNumber = "2019CONNUM0000001261",
                ManagingContractorID = 2318,
                ConstructionMilestoneCode = "00002",
                ConstructionMilestoneDescription = "Concreting-Footing (w/ 7D Concrete Test)",
                Weight = 25,
                Sequence = 2,
                PercentageCompletion = 20,
                PercentageReferenceNumber = "2018CONMIL0000000002",
                PONumber = "PO 654321"
            });
            list.Add(new ActualOnGoingConstructionMilestone
            {
                OTCNumber = "2019OTCNUM0000003143",
                ContractorNumber = "2019CONNUM0000001261",
                ManagingContractorID = 2318,
                ConstructionMilestoneCode = "00003",
                ConstructionMilestoneDescription = "Concrete Test-Footing @ 28 days",
                Weight = 25,
                Sequence = 1,
                PercentageCompletion = 0,
                PercentageReferenceNumber = "2018CONMIL0000000003",
                PONumber = "PO 654321"
            });
            list.Add(new ActualOnGoingConstructionMilestone
            {
                OTCNumber = "2019OTCNUM0000003143",
                ContractorNumber = "2019CONNUM0000001261",
                ManagingContractorID = 2318,
                ConstructionMilestoneCode = "00004",
                ConstructionMilestoneDescription = "Rough Ins-Drainage GF",
                Weight = 25,
                Sequence = 2,
                PercentageCompletion = 20,
                PercentageReferenceNumber = "2018CONMIL0000000004",
                PONumber = "PO 654321"
            });

            //OTC 7
            list.Add(new ActualOnGoingConstructionMilestone
            {
                OTCNumber = "2019OTCNUM0000003143",
                ContractorNumber = "2019CONNUM0000001262",
                ManagingContractorID = 2319,
                ConstructionMilestoneCode = "00005",
                ConstructionMilestoneDescription = "Rough Ins-Sewer Line GF",
                Weight = 25,
                Sequence = 1,
                PercentageCompletion = 80,
                PercentageReferenceNumber = "2018CONMIL0000000005",
                PONumber = "PO 654322"
            });
            list.Add(new ActualOnGoingConstructionMilestone
            {
                OTCNumber = "2019OTCNUM0000003143",
                ContractorNumber = "2019CONNUM0000001262",
                ManagingContractorID = 2319,
                ConstructionMilestoneCode = "00006",
                ConstructionMilestoneDescription = "Rough Ins-Water Line GF",
                Weight = 25,
                Sequence = 2,
                PercentageCompletion = 20,
                PercentageReferenceNumber = "2018CONMIL0000000006",
                PONumber = "PO 654322"
            });
            list.Add(new ActualOnGoingConstructionMilestone
            {
                OTCNumber = "2019OTCNUM0000003143",
                ContractorNumber = "2019CONNUM0000001262",
                ManagingContractorID = 2319,
                ConstructionMilestoneCode = "00007",
                ConstructionMilestoneDescription = "Rough Ins-Electrical GF",
                Weight = 25,
                Sequence = 1,
                PercentageCompletion = 0,
                PercentageReferenceNumber = "2018CONMIL0000000007",
                PONumber = "PO 654322"
            });
            list.Add(new ActualOnGoingConstructionMilestone
            {
                OTCNumber = "2019OTCNUM0000003143",
                ContractorNumber = "2019CONNUM0000001262",
                ManagingContractorID = 2319,
                ConstructionMilestoneCode = "00008",
                ConstructionMilestoneDescription = "Rough Ins-Vent line",
                Weight = 25,
                Sequence = 2,
                PercentageCompletion = 20,
                PercentageReferenceNumber = "2018CONMIL0000000008",
                PONumber = "PO 654322"
            });

            //OTC 8
            list.Add(new ActualOnGoingConstructionMilestone
            {
                OTCNumber = "2019OTCNUM0000003777",
                ContractorNumber = "2019CONNUM0000001361",
                ManagingContractorID = 2418,
                ConstructionMilestoneCode = "00001",
                ConstructionMilestoneDescription = "Hoarding Fence",
                Weight = 25,
                Sequence = 1,
                PercentageCompletion = 80,
                PercentageReferenceNumber = "2018CONMIL0000000001",
                PONumber = "PO 654321"
            });
            list.Add(new ActualOnGoingConstructionMilestone
            {
                OTCNumber = "2019OTCNUM0000003777",
                ContractorNumber = "2019CONNUM0000001361",
                ManagingContractorID = 2418,
                ConstructionMilestoneCode = "00002",
                ConstructionMilestoneDescription = "Concreting-Footing (w/ 7D Concrete Test)",
                Weight = 25,
                Sequence = 2,
                PercentageCompletion = 20,
                PercentageReferenceNumber = "2018CONMIL0000000002",
                PONumber = "PO 654321"
            });
            list.Add(new ActualOnGoingConstructionMilestone
            {
                OTCNumber = "2019OTCNUM0000003777",
                ContractorNumber = "2019CONNUM0000001361",
                ManagingContractorID = 2418,
                ConstructionMilestoneCode = "00003",
                ConstructionMilestoneDescription = "Concrete Test-Footing @ 28 days",
                Weight = 25,
                Sequence = 1,
                PercentageCompletion = 0,
                PercentageReferenceNumber = "2018CONMIL0000000003",
                PONumber = "PO 654321"
            });
            list.Add(new ActualOnGoingConstructionMilestone
            {
                OTCNumber = "2019OTCNUM0000003777",
                ContractorNumber = "2019CONNUM0000001361",
                ManagingContractorID = 2418,
                ConstructionMilestoneCode = "00004",
                ConstructionMilestoneDescription = "Rough Ins-Drainage GF",
                Weight = 25,
                Sequence = 2,
                PercentageCompletion = 20,
                PercentageReferenceNumber = "2018CONMIL0000000004",
                PONumber = "PO 654321"
            });

            //OTC 9
            list.Add(new ActualOnGoingConstructionMilestone
            {
                OTCNumber = "2019OTCNUM0000004090",
                ContractorNumber = "2019CONNUM0000001596",
                ManagingContractorID = 2653,
                ConstructionMilestoneCode = "00001",
                ConstructionMilestoneDescription = "Hoarding Fence",
                Weight = 25,
                Sequence = 1,
                PercentageCompletion = 80,
                PercentageReferenceNumber = "2018CONMIL0000000001",
                PONumber = "PO 654321"
            });
            list.Add(new ActualOnGoingConstructionMilestone
            {
                OTCNumber = "2019OTCNUM0000004090",
                ContractorNumber = "2019CONNUM0000001596",
                ManagingContractorID = 2653,
                ConstructionMilestoneCode = "00002",
                ConstructionMilestoneDescription = "Concreting-Footing (w/ 7D Concrete Test)",
                Weight = 25,
                Sequence = 2,
                PercentageCompletion = 20,
                PercentageReferenceNumber = "2018CONMIL0000000002",
                PONumber = "PO 654321"
            });
            list.Add(new ActualOnGoingConstructionMilestone
            {
                OTCNumber = "2019OTCNUM0000004090",
                ContractorNumber = "2019CONNUM0000001596",
                ManagingContractorID = 2653,
                ConstructionMilestoneCode = "00003",
                ConstructionMilestoneDescription = "Concrete Test-Footing @ 28 days",
                Weight = 25,
                Sequence = 1,
                PercentageCompletion = 0,
                PercentageReferenceNumber = "2018CONMIL0000000003",
                PONumber = "PO 654321"
            });
            list.Add(new ActualOnGoingConstructionMilestone
            {
                OTCNumber = "2019OTCNUM0000004090",
                ContractorNumber = "2019CONNUM0000001596",
                ManagingContractorID = 2653,
                ConstructionMilestoneCode = "00004",
                ConstructionMilestoneDescription = "Rough Ins-Drainage GF",
                Weight = 25,
                Sequence = 2,
                PercentageCompletion = 20,
                PercentageReferenceNumber = "2018CONMIL0000000004",
                PONumber = "PO 654321"
            });

            //OTC 10
            list.Add(new ActualOnGoingConstructionMilestone
            {
                OTCNumber = "2019OTCNUM0000003409",
                ContractorNumber = "2019CONNUM0000001318",
                ManagingContractorID = 1138,
                ConstructionMilestoneCode = "00001",
                ConstructionMilestoneDescription = "Hoarding Fence",
                Weight = 25,
                Sequence = 1,
                PercentageCompletion = 80,
                PercentageReferenceNumber = "2018CONMIL0000000001",
                PONumber = "PO 654321"
            });
            list.Add(new ActualOnGoingConstructionMilestone
            {
                OTCNumber = "2019OTCNUM0000003409",
                ContractorNumber = "2019CONNUM0000001318",
                ManagingContractorID = 1138,
                ConstructionMilestoneCode = "00002",
                ConstructionMilestoneDescription = "Concreting-Footing (w/ 7D Concrete Test)",
                Weight = 25,
                Sequence = 2,
                PercentageCompletion = 20,
                PercentageReferenceNumber = "2018CONMIL0000000002",
                PONumber = "PO 654321"
            });
            list.Add(new ActualOnGoingConstructionMilestone
            {
                OTCNumber = "2019OTCNUM0000003409",
                ContractorNumber = "2019CONNUM0000001318",
                ManagingContractorID = 1138,
                ConstructionMilestoneCode = "00003",
                ConstructionMilestoneDescription = "Concrete Test-Footing @ 28 days",
                Weight = 25,
                Sequence = 1,
                PercentageCompletion = 0,
                PercentageReferenceNumber = "2018CONMIL0000000003",
                PONumber = "PO 654321"
            });
            list.Add(new ActualOnGoingConstructionMilestone
            {
                OTCNumber = "2019OTCNUM0000003409",
                ContractorNumber = "2019CONNUM0000001318",
                ManagingContractorID = 1138,
                ConstructionMilestoneCode = "00004",
                ConstructionMilestoneDescription = "Rough Ins-Drainage GF",
                Weight = 25,
                Sequence = 2,
                PercentageCompletion = 20,
                PercentageReferenceNumber = "2018CONMIL0000000004",
                PONumber = "PO 654321"
            });

            return list;
        }

        public static IEnumerable<ActualOnGoingConstructionMilestonePunchlist> GetActualOnGoingConstructionsMilestonePunchListAsync(string otcNumber, string contractorNumber, int managingContractorID, string milestoneCode)
        {
            return new List<ActualOnGoingConstructionMilestonePunchlist>();
        }


        public static User GetUser(string Type)
        {
            if (Type == " ")
            {
                return new User()
                {
                    Email = "QA@cti.com",
                    FirstName = "QA",
                    LastName = "Inspector",
                    Password = "Test",
                    Token = "TestToken",
                    AgentID = "2452524",
                    inspectionRadius = 1000,
                    Role = new Role { Radius = 100M }
                };
            }
            else
                return new User()
                {
                    Email = "ptg@cti.com",
                    FirstName = "PTG",
                    LastName = "Inspector",
                    Password = "Test",
                    Token = "TestToken",
                    AgentID = "2452524",
                    inspectionRadius = 1000,
                    Role = new Role { Radius = 0.5M }
                };
        }

        public static IEnumerable<CostImpact> GetCostImpactsAsync()
        {
            return new List<CostImpact> {
                    new CostImpact {  Code = "Yes" , Name =  "Yes" } ,
                    new CostImpact { Code = "No", Name = "No" },
                    new CostImpact { Code = "TBD", Name = "TBD" },
                };
        }

        public static IEnumerable<ScheduleImpact> GetScheduleImpactsAsync()    
        {
            return new List<ScheduleImpact> {
                    new ScheduleImpact {  Code = "Yes" , Name =  "Yes" } ,
                    new ScheduleImpact { Code = "No", Name = "No" },
                    new ScheduleImpact { Code = "TBD", Name = "TBD" },
                };
        }

        public static IEnumerable<NonComplianceTo> GetNonCompliancesToAsync()
        {
            return new List<NonComplianceTo> {
                    new NonComplianceTo {  Code = "Plans" , Name =  "Plans" } ,
                    new NonComplianceTo { Code = "Specifications", Name = "Specifications" },
                };
        }

        public static IEnumerable<PunchlistCategory> GetPunchlistCategoriesAsync()
        {
            return new List<PunchlistCategory> {
                    new PunchlistCategory {  Code = "1" , Name =  "Workmanship" , IsNonCompliance = false } ,
                    new PunchlistCategory { Code = "2", Name = "Non - Compliance", IsNonCompliance = true },
                    new PunchlistCategory { Code = "3", Name = "Defect" , IsNonCompliance = false},
                    new PunchlistCategory { Code = "4", Name = "Incomplete Work", IsNonCompliance = false },
                };
        }

        public static IEnumerable<PunchlistSubCategory> GetPunchlistSubCategoriesAsync()
        {
            return new List<PunchlistSubCategory> {
                    new PunchlistSubCategory {  Code = "Critical" , Name =  "Critical" , OfficeDays = 0 } ,
                    new PunchlistSubCategory { Code = "Major", Name = "Major", OfficeDays = 5 },
                    new PunchlistSubCategory { Code = "Minor", Name = "Minor" , OfficeDays = 2},
            };
        }

        public static IEnumerable<PunchlistLocation> GetPunchlistLocationsAsync()
        {
            return new List<PunchlistLocation> {
                    new PunchlistLocation { Code = "1" , Name =  "Living" } ,
                    new PunchlistLocation { Code = "2" , Name =  "Dining" } ,
                    new PunchlistLocation { Code = "3" , Name =  "Kitchen" } ,
                    new PunchlistLocation { Code = "4" , Name =  "Bedroom 1" } ,
                    new PunchlistLocation { Code = "5" , Name =  "Bedroom 2" } ,
                    new PunchlistLocation { Code = "6" , Name =  "Master Bedroom" } ,
                    new PunchlistLocation { Code = "7" , Name =  "Toilet 1" } ,
                    new PunchlistLocation { Code = "8" , Name =  "Toilet 2" } ,
                    new PunchlistLocation { Code = "9" , Name =  "Toilet 3" } ,
                    new PunchlistLocation { Code = "10" , Name =  "Master Bedroom Toilet" } ,
                     new PunchlistLocation { Code = "11" , Name =  "Powder Room 1" } ,
                     new PunchlistLocation { Code = "12" , Name =  "Power Room 2" } ,
                     new PunchlistLocation { Code = "13" , Name =  "Maid's Room" } ,
                     new PunchlistLocation { Code = "14" , Name =  "Maid's Toilet" } ,
                     new PunchlistLocation { Code = "15" , Name =  "Maid's Toilet" } ,
                     new PunchlistLocation { Code = "16" , Name =  "Service Area" } ,
            };
        }


        //public static IEnumerable<PunchlistStatus> GetPunchlistStatusesAsync(UserRoleType userRole)
        //{
        //    var userPunchlistStatus =  UserPunchlistStatus().ToList();

        //    return new List<PunchlistStatus>(userPunchlistStatus.Where(x => x.UserRoleType == userRole).Select(s => s.UserPunchlistStatus).ToList() );
        //}




        public static IEnumerable<UserTypePunchlistStatus> UserPunchlistStatusAsync(UserRoleType userRole)
        {
            var userPunchlistStatus = new List<UserTypePunchlistStatus> {

                new UserTypePunchlistStatus { UserRoleType = UserRoleType.QA, IsDefault = true,  PunchlistCode = "Open"  },
                new UserTypePunchlistStatus { UserRoleType = UserRoleType.QA, IsDefault = false, PunchlistCode =  "Closed" },
                new UserTypePunchlistStatus { UserRoleType = UserRoleType.QA, IsDefault = false, PunchlistCode =  "Void"},
                new UserTypePunchlistStatus { UserRoleType = UserRoleType.Engineer, IsDefault = true, PunchlistCode =  "Open"  },
                new UserTypePunchlistStatus { UserRoleType = UserRoleType.Engineer, IsDefault = false, PunchlistCode =  "Closed" },
                new UserTypePunchlistStatus { UserRoleType = UserRoleType.Engineer, IsDefault = false, PunchlistCode =  "Void" },
                new UserTypePunchlistStatus { UserRoleType = UserRoleType.Contractor, IsDefault = false, PunchlistCode =  "InProgress" },
                new UserTypePunchlistStatus { UserRoleType = UserRoleType.Contractor, IsDefault = false, PunchlistCode =  "Completed" },
                new UserTypePunchlistStatus { UserRoleType = UserRoleType.Contractor, IsDefault = false, PunchlistCode =  "Rejected" },
            };

            return userPunchlistStatus.Where(x => x.UserRoleType == userRole).ToList();

        }

        public static IEnumerable<PunchlistStatus> GetPunchlistStatusesAsync()
        {
            return new List<PunchlistStatus> {

                new PunchlistStatus { Code = "Open" , Name = "Open" , IsOpen = true, IsClosed = false },
                new PunchlistStatus { Code = "Closed" , Name = "Closed" , IsOpen = false, IsClosed = true },
                new PunchlistStatus { Code = "Void" , Name = "Void" , IsOpen = false, IsClosed = false },
                new PunchlistStatus { Code = "InProgress" , Name = "In Progress" , IsOpen = false, IsClosed = false },
                new PunchlistStatus { Code = "Completed" , Name = "Completed", IsOpen = false, IsClosed = false },
                new PunchlistStatus { Code = "Rejected" , Name = "Rejected" , IsOpen = false, IsClosed = false },

            };

        }

        public static IEnumerable<Punchlist> GetPunchList()
        {
            return new List<Punchlist> {

                new Punchlist {PunchListID = 1, PunchListDescription = "Unpainted Area"},
                new Punchlist {PunchListID = 2, PunchListDescription = "Remove Paint Drips"},
                new Punchlist {PunchListID = 3, PunchListDescription = "Remove Waste materials"},
                new Punchlist {PunchListID = 4, PunchListDescription = "Re-do Primer in dining area"}

            };

        }

        public static Punchlist GetPunchlistDetailsByIdAsync(int punchlistId)
        {

            var listPunch = new List<Punchlist> {

                new Punchlist {PunchListID = 1,
                    PunchListDescription = "Unpainted Area",
                    PunchListCategory = "1",
                    PunchListSubCategory = "Critical",

                     Comments = new List<Comment> {

                             new Comment  {
                               CreatedBy = new User

                                {
                                   Email = "QA@cti.com",
                                   FirstName = "Nestor",
                                   LastName = "Contractor",
                                   Password = "Test",
                                   Token = "TestToken",
                                   AgentID = "2452524",
                                   inspectionRadius = 1000,
                                   Role = new Role { Radius = 100M , Type  =  UserRoleType.Contractor }

                               },
                                AttachmentFileName = new List<string> { "image 1"},
                                created_at = DateTime.Now.AddMinutes(-30),
                                Message = "What's the color?",
                                CommentId = 1
                            }
                             , new Comment  {
                               CreatedBy = new User

                                {
                                   Email = "QA@cti.com",
                                   FirstName = "QA",
                                   LastName = "Inspector",
                                   Password = "Test",
                                   Token = "TestToken",
                                   AgentID = "2452524",
                                   inspectionRadius = 1000,
                                   Role = new Role { Radius = 100M , Type  =  UserRoleType.Engineer }

                               },
                                AttachmentFileName = null,
                                created_at = DateTime.Now.AddMinutes(-20),
                                Message = "Cerulean",
                                CommentId = 2

                            }

                     }

                },
                new Punchlist {PunchListID = 2, PunchListDescription = "Remove Paint Drips"},
                new Punchlist {PunchListID = 3, PunchListDescription = "Remove Waste materials"},
                new Punchlist {PunchListID = 4, PunchListDescription = "Re-do Primer in dining area"}

            };

            return listPunch.Where(x => x.PunchListID == punchlistId).FirstOrDefault();

        }
    */
    
    }
}
