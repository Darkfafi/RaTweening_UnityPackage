using UnityEngine;
using Supyrb;
using System;

using UnityEditor;
using System.Reflection;
using System.Linq;

namespace RaTweening
{
	[CustomEditor(typeof(RaTweenerComponent))]
	public class RaTweenerComponentEditor : Editor
	{
		private SearchWindow _currentSearchWindow = null;

		public override void OnInspectorGUI()
		{
			SerializedProperty tweenProp = serializedObject.FindProperty("_raTween");

			if(GUILayout.Button("Select Tweener"))
			{
				Type[] tweenTypes = AppDomain.CurrentDomain
					.GetAssemblies()
					.SelectMany(x => x.GetTypes())
					.Where(x => typeof(RaTweenBase).IsAssignableFrom(x) && !x.IsAbstract)
					.ToArray();

				_currentSearchWindow = SearchWindow.OpenWindow((index) => 
				{
					if(index >= 0)
					{
						Type tweenType = tweenTypes[index];
						if(tweenType.GetConstructor(Type.EmptyTypes) != null)
						{
							object value = Activator.CreateInstance(tweenType);
							tweenProp.SetValue(value);
						}
						else
						{
							EditorUtility.DisplayDialog("Error", "No Default Constructor for Tweener " + tweenType.ToString() + "! Please add a default constructor", "Ok");
						}
						_currentSearchWindow.Close();
					}
					_currentSearchWindow = null;
				}, tweenTypes);
			}
			serializedObject.Update();

			if(tweenProp != null)
			{
				string name = tweenProp.type;
				if(!string.IsNullOrEmpty(name))
				{
					name = name.Replace("managedReference", "");

					if(name.Length > 2)
					{
						name = name.Remove(0, 1);
						name = name.Remove(name.Length - 1, 1);
					}
				}

				EditorGUILayout.PropertyField(tweenProp, new GUIContent(name), true);
				serializedObject.ApplyModifiedProperties();
			}
		}
	}
}
