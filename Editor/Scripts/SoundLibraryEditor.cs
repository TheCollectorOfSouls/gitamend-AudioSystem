using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace AudioSystem.Editor
{
	[CustomEditor(typeof(SoundLibrary))]
	public class SoundLibraryEditor : UnityEditor.Editor
	{
		public VisualTreeAsset inspectorXML;
		private SoundLibrary Target => target as SoundLibrary;
		
		public override VisualElement CreateInspectorGUI()
		{
			
			// Create a new VisualElement to be the root of our inspector UI
			var root = new VisualElement();
		
			// Load and clone a visual tree from UXML
			inspectorXML.CloneTree(root);
		
			var propertyField = root.Q<PropertyField>("entries-property");
			var updateLibraryBtn = root.Q<Button>("update-btn");
			propertyField.BindProperty(serializedObject.FindProperty("soundEntries"));
			
			if(updateLibraryBtn != null)
				updateLibraryBtn.clicked += Target.UpdateLibrary;
			
			return root;
		}
	}
}