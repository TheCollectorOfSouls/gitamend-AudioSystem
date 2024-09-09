using UnityEngine;

namespace AudioSystem
{
	/// <summary>
	/// This script is automatically called on startup and if the necessary managers are found in the Resources folder, it instantiates them.
	/// </summary>
	public static class AudioBootstrapper
	{
		private const string SoundManagerPath = "gitamend-MusicManager";
		private const string MusicManagerPath = "gitamend-SoundManager";
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Initialize()
		{
			var soundManager = Resources.Load<GameObject>(SoundManagerPath);
			if(soundManager) Object.Instantiate(soundManager);

			var musicManager = Resources.Load<GameObject>(MusicManagerPath);
			if(musicManager) Object.Instantiate(musicManager);
		}
	}
}