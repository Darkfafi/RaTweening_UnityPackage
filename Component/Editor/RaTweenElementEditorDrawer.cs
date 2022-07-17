using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static RaTweening.RaTweenerElementBase;

namespace RaTweening
{
	public class RaTweenElementEditorDrawer
	{
		#region Consts

		private static readonly string[] Events = new string[]
		{
			"_onSetup",
			"_onStart",
			"_onLoop",
			"_onComplete",
			"_onEnd",
		};

		private static readonly string[] Properties = new string[]
		{
			"_delay"
		};

		#endregion

		#region Variables

		private SerializedObject _elementSerializedObject = null;
		private RaTweenerElementBase _element = null;

		private SerializedProperty[] _eventProps = null;
		private bool _foldedOutEventProps = false;

		private SerializedProperty[] _propertiesProps = null;
		private bool _foldedOutPropertiesProps = false;

		private SerializedProperty _loopsProperty = null;

		#endregion

		public RaTweenElementEditorDrawer(SerializedObject elementObject)
		{
			_elementSerializedObject = elementObject;
			_element = elementObject.targetObject as RaTweenerElementBase;

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

			// Loops
			_loopsProperty = elementObject.FindProperty("_loops");
		}

		#region Public Methods

		public void Draw()
		{
			_elementSerializedObject.Update();

			EditorGUILayout.BeginVertical("box");
			{
				EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
				_foldedOutPropertiesProps = DrawFoldout(_foldedOutPropertiesProps, "Properties", _propertiesProps, DrawLoopingProperties);
				_foldedOutEventProps = DrawFoldout(_foldedOutEventProps, "Events", _eventProps, null);
			}
			EditorGUILayout.EndVertical();

			_elementSerializedObject.ApplyModifiedProperties();
		}

		#endregion

		#region Private Methods

		private void DrawLoopingProperties()
		{
			EditorGUILayout.BeginVertical("box");
			{
				EditorGUILayout.LabelField("Looping", EditorStyles.boldLabel);
				switch(_element.GetLoopAllowStage())
				{
					case LoopAllowStage.None:
						EditorGUILayout.LabelField("Disabled", EditorStyles.boldLabel);
						break;
					case LoopAllowStage.ToFinite:
						_loopsProperty.intValue = Mathf.Max(EditorGUILayout.IntField("Loops: ", _loopsProperty.intValue), 0);
						break;
					case LoopAllowStage.ToInfinity:
						bool isInfinite = _loopsProperty.intValue == RaTweenCore.InfiniteLoopingValue;

						if(!isInfinite)
						{
							_loopsProperty.intValue = Mathf.Max(EditorGUILayout.IntField("Loops: ", _loopsProperty.intValue), 0);
						}

						bool isInfiniteNew = EditorGUILayout.Toggle("IsInfiniteLoop: ", isInfinite);

						if(isInfinite != isInfiniteNew)
						{
							if(isInfiniteNew)
							{
								_loopsProperty.intValue = RaTweenCore.InfiniteLoopingValue;
							}
							else
							{
								_loopsProperty.intValue = 0;
							}
						}

						break;
				}
			}
			EditorGUILayout.EndVertical();
		}

		private bool DrawFoldout(bool foldout, string name, IList<SerializedProperty> props, Action customEditorCallback)
		{
			foldout = EditorGUILayout.Foldout(foldout, name);
			if(foldout)
			{
				for(int i = 0; i < props.Count; i++)
				{
					EditorGUILayout.PropertyField(props[i]);
				}

				customEditorCallback?.Invoke();
			}
			return foldout;
		}

		#endregion
	}
}
