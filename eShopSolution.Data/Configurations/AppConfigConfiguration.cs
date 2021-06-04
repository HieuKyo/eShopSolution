using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Configurations
{
    //Phương thức Config
    public class AppConfigConfiguration : IEntityTypeConfiguration<AppConfig>
    {
        public void Configure(EntityTypeBuilder<AppConfig> builder)
        {
            //Cấu hình cho bảng, các thuộc tính
            builder.ToTable("AppConfigs");
            //HasKey Key là khoá chính
            builder.HasKey(x => x.Key);
            //IsRequired(true) để bắt phải nhập
            builder.Property(x => x.Value).IsRequired(true);
        }
    }
}

