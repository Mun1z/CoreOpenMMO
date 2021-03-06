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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(t => t.Login)
                .IsRequired();

            builder.Property(t => t.Email)
                .IsRequired();

            builder.Property(t => t.Passwd)
                .IsRequired();
        }
    }
}
