using Core.Common.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Hcom.Web.Api
{
    public class FileUploadDBContext : DbContext
    {
        public FileUploadDBContext(DbContextOptions<FileUploadDBContext> options) : base(options)
        { }

        public DbSet<PunchListBinaryImage> PunchListBinaryImages { get; set; }

        public DbSet<ConstructionMilestoneBinaryImage> ConstructionMilestoneBinaryImages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.Entity<ConstructionMilestoneBinaryImage>(entity =>
            //{
            //    entity.HasKey(e => e.FileName)
            //        .ForSqlServerIsClustered(false)
            //        .HasName("PK_ConstructionMilestoneBinaryImage");

            //});

            builder.Ignore<PropertyChangedEventHandler>();
            builder.Ignore<IIdentifiableEntity>();
            builder.Ignore<ExtensionDataObject>();
        }
    }
}
