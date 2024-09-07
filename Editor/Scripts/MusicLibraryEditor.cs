using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace AudioSystem.Editor
{
	[CustomEditor(typeof(MusicLibrary))]
	public class MusicLibraryEditor : UnityEditor.Editor
	{
		public VisualTreeAsset inspectorXML;
		private MusicLibrary Target => target as MusicLibrary;

		public override VisualElement CreateInspectorGUI()
		{

			// Create a new VisualElement to be the root of our inspector UI
			var root = new VisualElement();

			// Load and clone a visual tree from UXML
			inspectorXML.CloneTree(root);

			var propertyField = root.Q<PropertyField>("entries-property");
			var updateLibraryBtn = root.Q<Button>("update-btn");

			propertyField.BindProperty(serializedObject.FindProperty("musicEntries"));

			if (updateLibraryBtn != null)
				updateLibraryBtn.clicked += Target.UpdateLibrary;

			return root;
		}
	}
}