using System.Collections.Generic;
using AudioSystem.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

namespace AudioSystem.Editor
{
	[CreateAssetMenu(fileName = "Entries Generator", menuName = "Gitamend/AudioSystem/Entries/Entries Generator", order = 0)]
	public class AudioEntriesGenerator : ScriptableObject
	{
		[SerializeField] private List<AudioClip> audioClips = new List<AudioClip>();
		[SerializeField] private string namePrefix;
		[SerializeField] private string nameSuffix;
		[SerializeField] private string savePath;
		[SerializeField] private SoundData defaultSoundData = new SoundData();
		
		public string SavePath { get => savePath; set => savePath = value; }

		public void GenerateSoundEntries()
		{
			if (!defaultSoundData.mixerGroup)
			{
				AudioLogger.LogError("Sound entries requires mixer group");
				return;
			}
			
			var folderPath = ("Assets" + savePath);
			if(!ValidFolder(folderPath)) return;
			
			foreach (var audioClip in audioClips)
			{
				var soundEntry = ScriptableObject.CreateInstance<SoundEntry>();
				var fileName = $"{namePrefix}{audioClip.name}{nameSuffix}";
				var assetPath = $"{folderPath}/{fileName}.asset";
				var existingSoundAsset = AssetDatabase.LoadAssetAtPath<SoundEntry>(assetPath);
				if (existingSoundAsset)
				{
					AudioLogger.LogWarning($"Sound entry {fileName} already exists. Skipping.");
					continue;
				}
				soundEntry.NameTag = fileName;
				soundEntry.SoundData.clip = audioClip;
				CopySoundDataValues(soundEntry.SoundData);
				AssetDatabase.CreateAsset(soundEntry, assetPath);
				// var soundEntryAsset = AssetDatabase.LoadAssetAtPath<SoundEntry>(assetPath);
				
			}
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}
		
		public void GenerateMusicEntries()
		{
			var folderPath = ("Assets" + savePath);
			if(!ValidFolder(folderPath)) return;
			
			foreach (var audioClip in audioClips)
			{
				var musicEntry = CreateInstance<MusicEntry>();
				var fileName = $"{namePrefix}{audioClip.name}{nameSuffix}";
				var assetPath = $"{folderPath}/{fileName}.asset";
				var existingSoundAsset = AssetDatabase.LoadAssetAtPath<MusicEntry>(assetPath);
				if (existingSoundAsset)
				{
					AudioLogger.LogWarning($"Music entry {fileName} already exists. Skipping.");
					continue;
				}
				musicEntry.AudioClip = audioClip;
				musicEntry.NameTag = fileName;
				AssetDatabase.CreateAsset(musicEntry, assetPath);
			}
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		private void CopySoundDataValues(SoundData soundData)
		{
			if(defaultSoundData == null || soundData == null) return;
			
			soundData.mixerGroup = defaultSoundData.mixerGroup;
			soundData.loop = defaultSoundData.loop;
			soundData.playOnAwake = defaultSoundData.playOnAwake;
			soundData.frequentSound = defaultSoundData.frequentSound;
			soundData.mute = defaultSoundData.mute;
			soundData.bypassEffects = defaultSoundData.bypassEffects;
			soundData.bypassListenerEffects = defaultSoundData.bypassListenerEffects;
			soundData.bypassReverbZones = defaultSoundData.bypassReverbZones;
			soundData.priority = defaultSoundData.priority;
			soundData.volume = defaultSoundData.volume;
			soundData.pitch = defaultSoundData.pitch;
			soundData.panStereo = defaultSoundData.panStereo;
			soundData.spatialBlend = defaultSoundData.spatialBlend;
			soundData.reverbZoneMix = defaultSoundData.reverbZoneMix;
			soundData.dopplerLevel = defaultSoundData.dopplerLevel;
			soundData.spread = defaultSoundData.spread;
			soundData.minDistance = defaultSoundData.minDistance;
			soundData.maxDistance = defaultSoundData.maxDistance;
			soundData.ignoreListenerVolume = defaultSoundData.ignoreListenerVolume;
			soundData.ignoreListenerPause = defaultSoundData.ignoreListenerPause;
			soundData.rolloffMode = defaultSoundData.rolloffMode;
		}

		private bool ValidFolder(string path)
		{
			if(AssetDatabase.IsValidFolder(path))
				return true;
			
			AudioLogger.LogError("Invalid save path");
			return false;
		}
	}
}