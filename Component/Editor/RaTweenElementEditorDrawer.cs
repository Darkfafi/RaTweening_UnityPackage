using System.Collections.Generic;
using UnityEditor;

namespace RaTweening
{
	public class RaTweenElementEditorDrawer
	{
		#region Consts

		private static readonly string[] Events = new string[]
		{
			"_onSetup",
			"_onStart",
			"_onComplete",
			"_onEnd",
		};

		private static readonly string[] Properties = new string[]
		{
			"_delay",
		};

		#endregion

		#region Variables

		private SerializedObject _elementObject = null;

		private SerializedProperty[] _eventProps = null;
		private bool _foldedOutEventProps = false;
		
		private SerializedProperty[] _propertiesProps = null;
		private bool _foldedOutPropertiesProps = false;

		#endregion

		public RaTweenElementEditorDrawer(SerializedObject elementObject)
		{
			_elementObject = elementObject;

			// Events
			_eventProps = new SerializedProperty[Events.Length];
			for(int i = 0; i < Events.Length; i++)
			{
				_eventProps[i] = elementObject.FindProperty(Events[i]);
			}

			// Properties
			_propertiesProps = new SerializedProperty[Properties.Length];
			for(int i = 0; i < Properties.Length; i++)
			{
				_propertiesProps[i] = elementObject.FindProperty(Properties[i]);
			}
		}

		#region Public Methods

		public void Draw()
		{
			EditorGUILayout.BeginVertical("box");
			{
				_elementObject.Update();
				EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
				_foldedOutEventProps = DrawFoldout(_foldedOutEventProps, "Events", _eventProps);
				_foldedOutPropertiesProps = DrawFoldout(_foldedOutPropertiesProps, "Properties", _propertiesProps);
				_elementObject.ApplyModifiedProperties();
			}
			EditorGUILayout.EndVertical();
		}

		#endregion

		#region Private Methods

		private bool DrawFoldout(bool foldout, string name, IList<SerializedProperty> props)
		{
			foldout = EditorGUILayout.Foldout(foldout, name);
			if(foldout)
			{
				for(int i = 0; i < props.Count; i++)
				{
					EditorGUILayout.PropertyField(props[i]);
				}
			}
			return foldout;
		}

		private void DrawLabel(string label, string label2)
		{
			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField(label);
				EditorGUILayout.LabelField(label2);
			}
			EditorGUILayout.EndHorizontal();
		}

		#endregion
	}
}
