using UnityEngine;

namespace AudioSystem
{
	[CreateAssetMenu(fileName = "MusicEntrySO", menuName = "Gitamend/AudioSystem/Entries/MusicEntry", order = 0)]
	public class MusicEntry : ScriptableObject
	{
		[SerializeField] private string nameTag;
		[SerializeField] private AudioClip audioClip;
		
		public string NameTag  { get => nameTag; set => nameTag = value;}
		public AudioClip AudioClip { get => audioClip; set => audioClip = value; }
	}
}