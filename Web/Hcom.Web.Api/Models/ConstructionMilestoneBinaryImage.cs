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
    public class ConstructionMilestoneBinaryImage : EntityBase, IIdentifiableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("cmiFileID")]
        public int FileID { get; set; }

        [Column("cmiFileBinary"), DataType("varbinary(max)")]
        public byte[] FileBinary { get; set; }

        [Column("cmiFileName")]
        public string FileName { get; set; }

        [Column("cmiDateCreated"), DataType("datetime")]
        public DateTime DateCreated { get; set; }

        [Column("cmiCreatedBy")]
        public string CreatedBy { get; set; }

        [Column("cmiDateModified"), DataType("datetime")]
        public DateTime? DateModified { get; set; }

        [Column("cmiModifiedBy")]
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
