using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Polyworks
{
	public class SaveLoadItemData
	{
		public void Save(string url, ItemData data) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Create (url);

			bf.Serialize (file, data);
			file.Close ();
		}

		public ItemData Load(string url) {
			if (File.Exists (url)) {
				BinaryFormatter bf = new BinaryFormatter ();
				FileStream file = File.Open (url, FileMode.Open);

				ItemData data = (ItemData)bf.Deserialize (file);
				file.Close ();

				return data;
			} else {
				return null;
			}
		}
	}
}

