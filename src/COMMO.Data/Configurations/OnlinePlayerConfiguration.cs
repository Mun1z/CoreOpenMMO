// <copyright file="OnlinePlayerConfiguration.cs" company="2Dudes">
// Copyright (c) 2018 2Dudes. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using COMMO.Data.Models;

namespace COMMO.Data.Configurations
{
    public class OnlinePlayerConfiguration : IEntityTypeConfiguration<OnlinePlayer>
    {
        public void Configure(EntityTypeBuilder<OnlinePlayer> builder)
        {
            builder.HasKey(b => b.Name);

            builder.Property(t => t.Level)
                .IsRequired();

            builder.Property(t => t.Vocation)
                .IsRequired();
        }
    }
}
