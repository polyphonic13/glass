﻿using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class DataController : MonoBehaviour {

	public void Save(string url, GameData data) {
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (url);

		bf.Serialize (file, data);
		file.Close ();
	}

	public GameData Load(string url) {
		if (File.Exists (url)) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (url, FileMode.Open);

			GameData data = (GameData)bf.Deserialize (file);
			file.Close ();

			return data;
		} else {
			return null;
		}
	}
}
