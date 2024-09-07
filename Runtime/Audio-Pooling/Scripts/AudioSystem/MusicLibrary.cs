using System;
using System.Collections.Generic;
using System.Globalization;
using AudioSystem.Utils;
using UnityEngine;

namespace AudioSystem
{
	[CreateAssetMenu(fileName = "MusicLibrarySO", menuName = "Gitamend/AudioSystem/Libraries/MusicLibrary", order = 0)]
	public class MusicLibrary : ScriptableObject
	{
		[SerializeField] private List<MusicEntry> musicEntries = new List<MusicEntry>();
		[SerializeField]private string lastUpdate;
		[SerializeField]private StringHashKeyDictionary keyRegistries;
		[SerializeField]private HashKeyMusicEntryDictionary musicEntriesDictionary;

		public AudioClip GetMusicClip(string tag)
		{
			return TryGetHashString(tag, out var hashString) ? GetMusicClip(hashString) : null;
		}
		
		public AudioClip GetMusicClip(HashKey hashKey)
		{
			if (musicEntriesDictionary.TryGetValue(hashKey, out var musicEntry))
				return musicEntry.AudioClip;

			AudioLogger.LogError($"Music hash of tag {hashKey} does not exist in library.");
			return null;
		}

		public bool TryGetHashString(string tag, out HashKey hashKey)
		{
			bool found = keyRegistries.TryGetValue(tag, out hashKey);
			
			if (found)
				return true;

			AudioLogger.LogError($"Music tag {tag} does not exist in library.");
			return false;
		}
		
		public void UpdateLibrary() 
		{
			keyRegistries = new StringHashKeyDictionary();
			musicEntriesDictionary = new HashKeyMusicEntryDictionary();
			lastUpdate = DateTime.Now.ToString(CultureInfo.CurrentCulture);
			
			if (musicEntries.Count <= 0)
			{
				AudioLogger.LogWarning("Music Library is empty.");
				return;
			}
			
			for (int i = musicEntries.Count - 1; i >= 0; i--)
			{
				var musicEntry = musicEntries[i];

				if (!musicEntry)
				{
					musicEntries.RemoveAt(i);
					continue;
				}
				
				if (string.IsNullOrWhiteSpace(musicEntry.NameTag))
				{
					AudioLogger.LogWarning($"Music tag missing for entry {musicEntry.name} at index {i}.");
					continue;
				}

				if (keyRegistries.ContainsKey(musicEntry.NameTag))
				{
					AudioLogger.LogWarning($"Music tag {musicEntry.NameTag} of entry {musicEntry.name} " +
					                 $"already exists in library.");
					continue;
				}
				
				var hashString = new HashKey(musicEntry.NameTag);
				keyRegistries.Add(musicEntry.NameTag, hashString);
				musicEntriesDictionary.Add(hashString, musicEntry);
			}
		}
	}
}