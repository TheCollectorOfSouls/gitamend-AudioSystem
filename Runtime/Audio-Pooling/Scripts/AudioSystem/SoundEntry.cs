using System;
using UnityEngine;

namespace AudioSystem
{
	[CreateAssetMenu(fileName = "SoundEntrySO", menuName = "Gitamend/AudioSystem/Entries/SoundEntry", order = 0)]
	public class SoundEntry : ScriptableObject
	{
		[SerializeField] private string nameTag;
		[SerializeField] private SoundData soundData = new SoundData();
		
		public string NameTag { get => nameTag; set => nameTag = value; }
		public SoundData SoundData => soundData;
	}
}