using Core.Common.Contracts;
using Core.Common.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace Hcom.Web.Api
{
    public class PunchListBinaryImage : EntityBase, IIdentifiableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("cimFileID", Order = 1)]
        public int FileID { get; set; }

        [Column("cimFileName")]
        public string FileName { get; set; }

        [Column("cimFileBinary"), DataType("varbinary(max)")]
        public byte[] FileBinary { get; set; }

        [Column("cimDateCreated"), DataType("datetime")]
        public DateTime DateCreated { get; set; }

        [Column("cimCreatedBy")]
        public string CreatedBy { get; set; }

        [Column("cimDateModified"), DataType("datetime")]
        public DateTime? DateModified { get; set; }

        [Column("cimModifiedBy")]
        public string ModifiedBy { get; set; }

        [IgnoreDataMember]
        [NotMapped]
        public int EntityId
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
