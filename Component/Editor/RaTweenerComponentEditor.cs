using Supyrb;
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace RaTweening
{
	[CustomEditor(typeof(RaTweenerComponent))]
	public class RaTweenerComponentEditor : Editor
	{
		private SerializedProperty _tweenProperty;

		protected void OnEnable()
		{
			_tweenProperty = serializedObject.FindProperty("_raTween");
		}

		public static SearchWindow CreateTweenSearchWindow(Action<RaTweenCore> onSelectTween)
		{
			Type[] tweenTypes = GetAllTweenTypes();

			SearchWindow window = null;

			window = SearchWindow.OpenWindow((index) =>
			{
				if(index >= 0)
				{
					Type tweenType = tweenTypes[index];
					if(tweenType.GetConstructor(Type.EmptyTypes) != null)
					{
						RaTweenCore value = Activator.CreateInstance(tweenType) as RaTweenCore;
						onSelectTween?.Invoke(value);
					}
					else
					{
						EditorUtility.DisplayDialog("Error", "No Default Constructor for Tweener " + tweenType.ToString() + "! Please add a default constructor", "Ok");
					}
					window.Close();
				}
				window = null;
			}, tweenTypes);
			return window;
		}

		public static Type[] GetAllTweenTypes()
		{
			return AppDomain.CurrentDomain
					.GetAssemblies()
					.SelectMany(x => x.GetTypes())
					.Where(x => typeof(RaTweenCore).IsAssignableFrom(x) && !x.IsAbstract)
					.ToArray();
		}

		public override VisualElement CreateInspectorGUI()
		{
			// Create property container element.
			var container = new VisualElement();

			Button button = null;
			PropertyField tweenerField = new PropertyField();

			button = new Button(() => { OnClickButton(); });

			Refresh();

			// Add fields to the container.
			container.Add(button);
			container.Add(tweenerField);

			return container;

			void OnClickButton()
			{
				CreateTweenSearchWindow((tween) =>
				{
					if(_tweenProperty != null)
					{
						if(tween != null)
						{
							tween.SetDefaultValuesInternal();
						}

						_tweenProperty.SetValue(tween);
						serializedObject.ApplyModifiedProperties();
					}

					Refresh();
				});
			}

			void Refresh()
			{
				serializedObject.Update();
				
				tweenerField.BindProperty(_tweenProperty);

				name = GetTweenName();
				button.text = string.IsNullOrEmpty(name) ? "Select Tweener" : name;
			}
		}

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
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
