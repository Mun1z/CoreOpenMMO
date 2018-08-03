// <copyright file="User.cs" company="2Dudes">
// Copyright (c) 2018 2Dudes. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>

using COMMO.Data.Contracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace COMMO.Data.Models {
	[Table("worlds", Schema = "opentibia_classic")]
    public class World : IWorld
    {
        [Key]
        public short Id { get; set; }
        public string Name { get; set; }
        public string IP { get; set; }
        public ushort Port { get; set; }
        public string Secret { get; set; }
    }
}
