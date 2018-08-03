// <copyright file="IUser.cs" company="2Dudes">
// Copyright (c) 2018 2Dudes. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace COMMO.Data.Contracts
{
    public interface IWorld
    {
        short Id { get; set; }
        string Name { get; set; }
        string IP { get; set; }
        ushort Port { get; set; }
        string Secret { get; set; }
    }
}
