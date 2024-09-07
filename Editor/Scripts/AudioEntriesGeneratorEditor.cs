using System;
using System.Linq;
using AudioSystem.Utils;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace AudioSystem.Editor
{
	[CustomEditor(typeof(AudioEntriesGenerator))]
	public class AudioEntriesGeneratorEditor : UnityEditor.Editor
	{
		public VisualTreeAsset inspectorXML;
		
		private DropdownField _entryStringDropdown;
		private PropertyField _soundDataProp;
		private AudioEntriesGenerator Target => target as AudioEntriesGenerator;

		public override VisualElement CreateInspectorGUI()
		{
			// Create a new VisualElement to be the root of our inspector UI
			var root = new VisualElement();

			// Load and clone a visual tree from UXML
			inspectorXML.CloneTree(root);

			var createEntriesBtn = root.Q<Button>("create-entries-btn");
			_entryStringDropdown = root.Q<DropdownField>("entry-type-dropdown");
			var browseSavePathBtn = root.Q<Button>("browse-save-path-btn");
			_soundDataProp = root.Q<PropertyField>("sound-data-prop");

			if (_entryStringDropdown != null)
				DropDownCheck();
			_entryStringDropdown?.RegisterCallback<ChangeEvent<string>>(DropDownCheck);

			if (browseSavePathBtn != null)
				browseSavePathBtn.clicked += BrowseSavePath;

			if (createEntriesBtn != null)
				createEntriesBtn.clicked += CreateEntries;

			return root;
		}

		private void BrowseSavePath()
		{
			var path = EditorUtility.SaveFolderPanel("Select a folder", 
				Application.dataPath + Target.SavePath,"");
			
			Target.SavePath = path.Length > 0 ? path[(Application.dataPath.Length)..] : "";
			EditorUtility.SetDirty(Target);
		}
		
		private void DropDownCheck()
		{
			if(_soundDataProp == null) return;
			_soundDataProp.visible = _entryStringDropdown.value == "Sound";
		}

		private void DropDownCheck(ChangeEvent<string> evt)
		{
			if(_soundDataProp == null) return;
			_soundDataProp.visible = evt.newValue == "Sound";
		}

		private void CreateEntries()
		{
			if(_entryStringDropdown == null) return;
			var entryString = _entryStringDropdown.value;
			switch (entryString)
			{
				case "Sound":
					Target.GenerateSoundEntries();
					break;
				case "Music":
					Target.GenerateMusicEntries();
					break;
				case "Select":
					AudioLogger.LogWarning("Entry type not selected");
					return;
			}
		}
	}
}