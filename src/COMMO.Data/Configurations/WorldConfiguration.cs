// <copyright file="UserConfiguration.cs" company="2Dudes">
// Copyright (c) 2018 2Dudes. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using COMMO.Data.Models;

namespace COMMO.Data.Configurations
{
    public class WorldConfiguration : IEntityTypeConfiguration<World>
    {
        public void Configure(EntityTypeBuilder<World> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(t => t.IP)
                .IsRequired();

            builder.Property(t => t.Name)
                .IsRequired();

            builder.Property(t => t.Port)
                .IsRequired();

			 builder.Property(t => t.Secret)
                .IsRequired();
        }
    }
}
