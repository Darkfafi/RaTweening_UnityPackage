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
		private SerializedProperty _tweenProperty;

		protected void OnEnable()
		{
			_tweenProperty = serializedObject.FindProperty("_raTween");
		}

		public override void OnInspectorGUI()
		{
			string name = GetTweenName();
			if(GUILayout.Button(string.IsNullOrEmpty(name) ? "Select Tweener" : name))
			{
				Type[] tweenTypes = AppDomain.CurrentDomain
					.GetAssemblies()
					.SelectMany(x => x.GetTypes())
					.Where(x => typeof(RaTweenCore).IsAssignableFrom(x) && !x.IsAbstract)
					.ToArray();

				_currentSearchWindow = SearchWindow.OpenWindow((index) =>
				{
					if(index >= 0)
					{
						Type tweenType = tweenTypes[index];
						if(tweenType.GetConstructor(Type.EmptyTypes) != null)
						{
							RaTweenCore value = Activator.CreateInstance(tweenType) as RaTweenCore;
							if(_tweenProperty != null)
							{
								if(value != null)
								{
									value.SetDefaultValuesInternal();
								}

								_tweenProperty.SetValue(value);
								serializedObject.ApplyModifiedProperties();
							}
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

			base.OnInspectorGUI();
		}

		private string GetTweenName()
		{
			if(_tweenProperty != null)
			{
				string name = _tweenProperty.type;
				if(!string.IsNullOrEmpty(name))
				{
					name = name.Replace("managedReference", "");

					if(name.Length > 2)
					{
						name = name.Remove(0, 1);
						name = name.Remove(name.Length - 1, 1);
					}
					return name;
				}
			}

			return string.Empty;
		}
	}
}
