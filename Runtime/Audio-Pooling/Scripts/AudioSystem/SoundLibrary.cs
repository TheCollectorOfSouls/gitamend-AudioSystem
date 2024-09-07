using System;
using System.Collections.Generic;
using System.Globalization;
using AudioSystem.Utils;
using UnityEngine;

namespace AudioSystem
{
	[CreateAssetMenu(fileName = "SoundLibrarySO", menuName = "Gitamend/AudioSystem/Libraries/SoundLibrary", order = 0)]
	public class SoundLibrary : ScriptableObject
	{
		[SerializeField] private List<SoundEntry> soundEntries = new List<SoundEntry>();
		[SerializeField]private string lastUpdate;
		[SerializeField] private StringHashKeyDictionary keyRegistries;
		[SerializeField] private HashKeySoundEntryDictionary soundEntriesDictionary;

		public SoundData GetSoundData(string tag)
		{
			return TryGetHashString(tag, out var hashString) ? GetSoundData(hashString) : null;
		}
		
		public SoundData GetSoundData(HashKey hashKey)
		{
			if (soundEntriesDictionary.TryGetValue(hashKey, out var soundEntry))
				return soundEntry.SoundData;
			
			AudioLogger.LogError($"Sound hash of tag {hashKey} does not exist in library.");
			return null;
		}

		public bool TryGetHashString(string tag, out HashKey hashKey)
		{
			bool found = keyRegistries.TryGetValue(tag, out hashKey);
			
			if (found)
				return true;

			AudioLogger.LogError($"Sound tag {tag} does not exist in library.");
			return false;
		}
		
		public void UpdateLibrary()
		{
			keyRegistries = new StringHashKeyDictionary();
			soundEntriesDictionary = new HashKeySoundEntryDictionary();
			lastUpdate = DateTime.Now.ToString(CultureInfo.CurrentCulture);
			
			if (soundEntries.Count <= 0)
			{
				AudioLogger.LogWarning("Sound Library is empty.");
				return;
			}
			
			for (int i = soundEntries.Count - 1; i >= 0; i--)
			{
				var soundEntry = soundEntries[i];

				if (!soundEntry)
				{
					soundEntries.RemoveAt(i);
					continue;
				}
				
				if (string.IsNullOrWhiteSpace(soundEntry.NameTag))
				{
					AudioLogger.LogWarning($"Sound tag missing for entry {soundEntry.name} at index {i}.");
					continue;
				}

				if (keyRegistries.ContainsKey(soundEntry.NameTag))
				{
					AudioLogger.LogWarning($"Sound tag {soundEntry.NameTag} of entry {soundEntry.name} " +
					                  $"already exists in library.");
					continue;
				}
				
				var hashString = new HashKey(soundEntry.NameTag);
				keyRegistries.Add(soundEntry.NameTag, hashString);
				soundEntriesDictionary.Add(hashString, soundEntry);
			}
			
			#if UNITY_EDITOR
			UnityEditor.EditorUtility.SetDirty(this);
			UnityEditor.AssetDatabase.SaveAssetIfDirty(this);
			#endif
		}
	}
}