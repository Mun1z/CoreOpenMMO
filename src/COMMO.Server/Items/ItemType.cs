// <copyright file="ItemType.cs" company="2Dudes">
// Copyright (c) 2018 2Dudes. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using COMMO.Data.Contracts;
using COMMO.Server.Data.Interfaces;
using COMMO.Server.OTB;

namespace COMMO.Server.Items
{
    public class ItemType : IItemType
    {
        public ushort TypeId { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public ISet<ItemFlag> Flags { get; }

        public IDictionary<ItemAttribute, IConvertible> DefaultAttributes { get; }

        public bool Locked { get; private set; }

        public ushort ClientId => Flags.Contains(ItemFlag.Disguise) ? Convert.ToUInt16(DefaultAttributes[ItemAttribute.DisguiseTarget]) : TypeId;

		public ushort Id;
        public ushort ClientId2;

        public string Name2;
        public string Article;
        public string PluralName;
        public string Description2;
        public string RuneSpellName;
        public string VocationString;

        public ItemGroups Group;
        public ItemTypes Type;

        public bool IsStackable;
        public bool IsAnimation;
		
        public uint Weight = 0;
        public uint LevelDoor;
        public uint DecayTime;
        public uint WieldInfo;
        public uint MinReqLevel;
        public uint MinReqMagicLevel;
        public uint Charges;

        public int MaxHitChance;
        public ushort DecayTo = 0;
        public ushort Attack = 0;
        public ushort Defense = 0;
        public short ExtraAttack = 0;
        public short ExtraDefense = 0;
        public ushort Armor = 0;
        public ushort RotateTo = 0;
        public int RuneLevel = 0;
		
        public ushort[] TransformToOnUse; //[2]
        public ushort TransformToFree;
        public ushort DestroyTo;
        public ushort MaxTextLength;
        public ushort WriteOnceItemId;
        public ushort TransformEquipTo;
        public ushort TransformDequipTo;
        public ushort MaxItems;
        public ushort Speed = 0;
        public ushort WareId = 0;
		
        public byte AlwaysOnTopOrder;
        public byte LightLevel;
        public byte LightColor;
        public byte ShootRange;
        public sbyte HitChance;
        
        public bool HasHeight;
        public bool WalkStack;
        public bool ForceUse;

        public bool DoesBlockSolid;
        public bool DoesBlockPickupable;
        public bool DoesBlockProjectile;
        public bool DoesBlockPathFinding;
        public bool DoesAllowPickupable;
        public bool ShowCount;
        public bool ShowDuration;
        public bool ShowCharges;
        public bool ShowAttributes;

        public bool IsReplaceable;
        public bool IsPickupable;
        public bool IsRotatable;
        public bool IsUseable;
        public bool IsMoveable;
        public bool IsAlwaysOnTop = false;
        public bool CanReadText = false;
        public bool CanWriteText = false;
        public bool CanLookThrough = false;
        public bool IsVertical = false;
        public bool IsHorizontal = false;
        public bool IsHangable = false;
        public bool AllowDistanceRead = false;
        public bool StopTime;
        
        public string GetPluralName()
        {
            //Returning cached value if calculated before
            if (!string.IsNullOrEmpty(PluralName))
                return PluralName;

            //Creating a cache not to calculate again next time
            PluralName = Name;
            if (ShowCount)
                PluralName += "s";

            return PluralName;
        }
		
        public bool IsSplash { get { return Group == ItemGroups.Splash; } }
        public bool IsFluidContainer { get { return Group == ItemGroups.Fluid; } }
        public bool HasSubType { get { return (IsFluidContainer || IsSplash || IsStackable || Charges != 0); } }

        public ItemType()
        {
            TypeId = 0;
            Name = string.Empty;
            Description = string.Empty;
            Flags = new HashSet<ItemFlag>();
            DefaultAttributes = new Dictionary<ItemAttribute, IConvertible>();
            Locked = false;
        }

        public void LockChanges()
        {
            Locked = true;
        }

        public void SetId(ushort typeId)
        {
            if (Locked)
            {
                throw new InvalidOperationException("This ItemType is locked and cannot be altered.");
            }

            TypeId = typeId;
        }

        public void SetName(string name)
        {
            if (Locked)
            {
                throw new InvalidOperationException("This ItemType is locked and cannot be altered.");
            }

            Name = name;
        }

        public void SetDescription(string description)
        {
            if (Locked)
            {
                throw new InvalidOperationException("This ItemType is locked and cannot be altered.");
            }

            Description = description.Trim('"');
        }

        public void SetFlag(ItemFlag flag)
        {
            if (Locked)
            {
                throw new InvalidOperationException("This ItemType is locked and cannot be altered.");
            }

            Flags.Add(flag);
        }

        public void SetAttribute(string attributeName, int attributeValue)
        {
            if (Locked)
            {
                throw new InvalidOperationException("This ItemType is locked and cannot be altered.");
            }

            ItemAttribute attribute;

            if (!Enum.TryParse(attributeName, out attribute))
            {
                throw new InvalidDataException($"Attempted to set an unknown Item attribute [{attributeName}].");
            }

            DefaultAttributes[attribute] = attributeValue;
        }
    }
}
