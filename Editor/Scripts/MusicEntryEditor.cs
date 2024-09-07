using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace AudioSystem.Editor
{
	[CustomEditor(typeof(MusicEntry))]
	public class MusicEntryEditor : UnityEditor.Editor
	{
		public VisualTreeAsset inspectorXML;
		private MusicEntry Target => target as MusicEntry;

		public override VisualElement CreateInspectorGUI()
		{
			// Create a new VisualElement to be the root of our inspector UI
			var root = new VisualElement();

			// Load and clone a visual tree from UXML
			inspectorXML.CloneTree(root);

			var filenameToTagBtn = root.Q<Button>("file-to-tag-btn");
			var tagToFilenameBtn = root.Q<Button>("tag-to-file-btn");
			var soundProperty = root.Q<PropertyField>("sound-property");

			soundProperty?.BindProperty(new SerializedObject(Target).FindProperty("audioClip"));

			if (tagToFilenameBtn != null)
				tagToFilenameBtn.clicked += SetTagAsFilename;

			if (filenameToTagBtn != null)
				filenameToTagBtn.clicked += () => Target.NameTag = Target.name;

			return root;
		}

		private void SetTagAsFilename()
		{
			if (string.IsNullOrWhiteSpace(Target.NameTag)) return;
			var path = AssetDatabase.GetAssetPath(Target);
			AssetDatabase.RenameAsset(path, Target.NameTag);
		}
	}
}