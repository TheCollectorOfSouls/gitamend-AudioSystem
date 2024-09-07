using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AudioSystem.Utils
{
	[Serializable]
	public class StringHashKeyDictionary : UnitySerializedDictionary<string, HashKey>
	{
	}

	[Serializable]
	public class HashKeySoundEntryDictionary : UnitySerializedDictionary<HashKey, SoundEntry>
	{
	}
	
	[Serializable]
	public class HashKeyMusicEntryDictionary : UnitySerializedDictionary<HashKey, MusicEntry>
	{
	}
	
}