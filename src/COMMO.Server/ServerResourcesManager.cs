using System.IO;
using System.Reflection;

namespace COMMO.Server {
	public class ServerResourcesManager
    {
		public const string DataFilesDirectory = "COMMO.Server.Data.dat";
		public const string MapFilesDirectory = "COMMO.Server.Data.map";
        public const string MapName = "COMMO.otbm";

		public static byte[] GetMap() {
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream(MapFilesDirectory + "." + MapName))
				return ReadFully(stream);
		}

		public static Stream GetItems(string itemsFileName) {
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream(DataFilesDirectory + "." + itemsFileName))
				return stream;
		}

		private static byte[] ReadFully(Stream input)
		{
			using (var ms = new MemoryStream())
			{
				input.CopyTo(ms);
				return ms.ToArray();
			}
		}
    }
}
