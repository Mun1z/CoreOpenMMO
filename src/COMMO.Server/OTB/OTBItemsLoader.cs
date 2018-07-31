namespace COMMO.Server.OTB {
	using COMMO.Data.Contracts;
	using COMMO.OTB;
	using COMMO.Server.Items;
	using COMMO.Server.World;
	using COMMO.Utilities;
	using NLog;
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// This class contains the methods necessary to load a .otbm file.
	/// </summary>
	public static partial class OTBItemsLoader {

		/// <summary>
		/// NLog's documentation suggests that we should store a reference to the logger,
		/// instead of asking the LogManager for a new instance everytime we need to log something.
		/// </summary>
		private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

		/// <summary>
		/// This class only support items encoded using this major version.
		/// </summary>
		public const uint SupportedItemEncodingMajorVersion = 3;

		/// <summary>
		/// This class only support items encoded using this minor version.
		/// </summary>
		public const uint SupportedItemEncodingMinorVersion = 8;

		/// <summary>
		/// Loads a .otbm file, parse it's contents and returns a <see cref="COMMO.Server.World.World"/>.
		/// </summary>
		public static Dictionary<ushort, ItemType> LoaderItems(ReadOnlyMemory<byte> serializedWorldData) {

			var items = new Dictionary<ushort, ItemType>();

			var rootNode = OTBDeserializer.DeserializeOTBData(serializedWorldData);
			ParseOTBTreeRootNode(rootNode);

			ushort id = 0;

			foreach (var itemNode in rootNode.Children)
			{
				try 
				{
					var parsingStream = new ReadOnlyMemoryStream(itemNode.Data);

					var flags = (TileFlags)parsingStream.ReadUInt32();
				
					ushort serverId = 0;
					ushort clientId = 0;
					ushort speed = 0;
					ushort wareId = 0;
					byte lightLevel = 0;
					byte lightColor = 0;
					byte alwaysOnTopOrder = 0;

					byte attrib = 0;

					//while (parsingStream.BytesLeftToRead != 0 || speed != )
					while (parsingStream.ReadByte(out attrib))
					{
						//attrib = parsingStream.ReadByte();

						var datalen = parsingStream.ReadUInt16();

						switch ((ItemAttributes)attrib) {
							case ItemAttributes.ServerId:
								serverId = parsingStream.ReadUInt16();

								if (serverId > 30000 && serverId < 30100)
									serverId -= 30000;
								break;

							case ItemAttributes.ClientId:
								clientId = parsingStream.ReadUInt16();
								break;

							case ItemAttributes.Speed:
								speed = parsingStream.ReadUInt16();
								break;

							case ItemAttributes.Light2:
								lightLevel = (byte)parsingStream.ReadUInt16();
								lightColor = (byte)parsingStream.ReadUInt16();
								break;

							case ItemAttributes.TopOrder:
								alwaysOnTopOrder = (byte)parsingStream.ReadByte();
								break;

							case ItemAttributes.WareId:
								wareId = parsingStream.ReadUInt16();
								break;

							default:
								parsingStream.Skip(datalen);
								break;
						}
					}
				
					var itemType = new ItemType();
				
					itemType.DoesBlockSolid = flags.HasFlag(TileFlags.BlocksSolid);
					itemType.DoesBlockProjectile = flags.HasFlag(TileFlags.BlocksProjectile);
					itemType.DoesBlockPathFinding = flags.HasFlag(TileFlags.BlocksPathFinding);
					itemType.HasHeight = flags.HasFlag(TileFlags.HasHeight);
					itemType.IsUseable = flags.HasFlag(TileFlags.Useable);
					itemType.IsPickupable = flags.HasFlag(TileFlags.Pickupable);
					itemType.IsMoveable = flags.HasFlag(TileFlags.Moveable);
					itemType.IsStackable = flags.HasFlag(TileFlags.Stackable);
					itemType.IsAlwaysOnTop = flags.HasFlag(TileFlags.AlwaysOnTop);
					itemType.IsVertical = flags.HasFlag(TileFlags.Vertical);
					itemType.IsHorizontal = flags.HasFlag(TileFlags.Horizontal);
					itemType.IsHangable = flags.HasFlag(TileFlags.Hangable);
					itemType.AllowDistanceRead = flags.HasFlag(TileFlags.AllowDistanceRead);
					itemType.IsRotatable = flags.HasFlag(TileFlags.Rotatable);
					itemType.CanReadText = flags.HasFlag(TileFlags.Readable);
					itemType.CanLookThrough = flags.HasFlag(TileFlags.LookThrough);
					itemType.IsAnimation = flags.HasFlag(TileFlags.Animation);
					// item.WalkStack = flags.HasFlag(TileFlags.FullTile);
					itemType.ForceUse = flags.HasFlag(TileFlags.ForceUse);

					itemType.Group = (ItemGroups)itemNode.Type;
					switch ((ItemGroups)itemNode.Type)
					{
						case ItemGroups.Container:
							itemType.Type = ItemTypes.Container;
							break;
						case ItemGroups.Door:
							itemType.Type = ItemTypes.Door;
							break;
						case ItemGroups.MagicField:
							itemType.Type = ItemTypes.MagicField;
							break;
						case ItemGroups.Teleport:
							itemType.Type = ItemTypes.Teleport;
							break;
					}
				
					itemType.Id = serverId;
					itemType.ClientId2 = clientId;
					itemType.Speed = speed;
					itemType.LightLevel = lightLevel;
					itemType.LightColor = lightColor;
					itemType.WareId = wareId;
					itemType.AlwaysOnTopOrder = alwaysOnTopOrder;

					items.Add(clientId, itemType);
				}
				catch (Exception ex)
				{

				}
				
				Console.WriteLine(id);

				id++;
			}

			//ParseWorldDataNode(worldDataNode, world);

			return items;
		}

		/// <summary>
		/// Logs the information embedded in the root node of the OTB tree.
		/// </summary>
		private static void ParseOTBTreeRootNode(OTBNode rootNode) {

			var parsingStream = new ReadOnlyMemoryStream(rootNode.Data);

			var flags = parsingStream.ReadUInt32();
			var attr = parsingStream.ReadByte();
			
			var datalen = parsingStream.ReadUInt16();

			var vi = new VERSIONINFO();

			vi.DwMajorVersion = parsingStream.ReadUInt32();
			vi.DwMinorVersion = parsingStream.ReadUInt32();
			vi.DwBuildNumber = parsingStream.ReadUInt32();
			

			Console.WriteLine($"Flags: {flags}");
		}
	}

	public class VERSIONINFO
	{
		public uint DwMajorVersion { get; set; }
		public uint DwMinorVersion { get; set; }
		public uint DwBuildNumber { get; set; }
		public byte[] CSDVersion { get; set; }
	}

	public enum ItemAttributes : byte
	{
		ServerId = 0x10,
        ClientId,
        Name,
        Description,
        Speed,
        Slot,
        MaxItems,
        Weight,
        Weapon,
        Ammunition,
        Armor,
        MagicLevel,
        MagicFieldType,
        Writeable,
        RotateTo,
        Decay,
        SpriteHash,
        MiniMapColor,
        Attr07,
        Attr08,
        Light,

        //1-byte aligned
        Decay2, //deprecated
        Weapon2, //deprecated
        Ammunition2, //deprecated
        Armor2, //deprecated
        Writeable2, //deprecated
        Light2,
        TopOrder,
        Writeable3, //deprecated

        WareId
	}

	public enum ItemGroups
    {
        None = 0,
        Ground,
        Container,
        Weapon, //deprecated
        Ammunition, //deprecated
        Armor, //deprecated
        Charges,
        Teleport, //deprecated
        MagicField, //deprecated
        Writeable, //deprecated
        Key, //deprecated
        Splash,
        Fluid,
        Door, //deprecated
        Deprecated,
        Last
    }

	public enum ItemTypes
    {
        None = 0,
        Depot,
        Mailbox,
        TrashHolder,
        Container,
        Door,
        MagicField,
        Teleport,
        Bed,
        Key,
        Rune,
        Last
    }
}
