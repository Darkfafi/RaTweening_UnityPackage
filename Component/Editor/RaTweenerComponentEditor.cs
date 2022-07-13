using RaTweening.Supyrb;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace RaTweening
{
	[CustomEditor(typeof(RaTweenerComponent))]
	public class RaTweenerComponentEditor : Editor
	{
		private SerializedProperty _tweenElementProperty;
		private Editor _editor;
		private string _name;

		protected void OnEnable()
		{
			_tweenElementProperty = serializedObject.FindProperty("_tweenElement");
		}

		public static SearchWindow CreateTweenSearchWindow(Action<Type> onSelectTween)
		{
			Type[] tweenTypes = GetAllTweenTypes();

			SearchWindow window = null;

			window = SearchWindow.OpenWindow((index) =>
			{
				if(index >= 0)
				{
					Type tweenType = tweenTypes[index];
					onSelectTween?.Invoke(tweenType);
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

		public override void OnInspectorGUI()
		{
			// Auto Fill Tweener
			if(_tweenElementProperty != null && _tweenElementProperty.objectReferenceValue == null)
			{
				var tweens = GetAllTweenTypes();
				if(tweens.Length > 0)
				{
					var tween = tweens.FirstOrDefault(x => TryGetRaTweenerElementAttribute(x, out _, out _));
					if(tween != null)
					{
						SelectTween(tween);
					}
				}
			}

			// Select Tweener Button
			if(GUILayout.Button(string.IsNullOrEmpty(_name) ? "Select Tweener" : _name))
			{
				CreateTweenSearchWindow((tweenType) =>
				{
					SelectTween(tweenType);
				});
			}

			// Draw Editor
			if(_tweenElementProperty != null)
			{
				if(_tweenElementProperty.objectReferenceValue != null)
				{
					if(_editor == null)
					{
						_editor = CreateEditor(_tweenElementProperty.objectReferenceValue);

						RaTweenerElementBase element = _tweenElementProperty.GetValue<RaTweenerElementBase>();

						if(element)
						{
							_name = element.GetElementName();
						}
					}
				}
				else
				{
					_editor = null;
				}

				if(_editor != null)
				{
					_editor.OnInspectorGUI();
					_editor.serializedObject.ApplyModifiedProperties();

					DrawDefaultInspector();
				}
			}
		}

		private void SelectTween(Type tweenType)
		{
			if(_tweenElementProperty != null)
			{
				if(TryGetRaTweenerElementAttribute(tweenType, out RaTweenerElementAttribute attribute, out string error))
				{
					if(serializedObject.targetObject is RaTweenerComponent parent)
					{
						try
						{
							RaTweenerElementBase value = parent.gameObject.AddComponent(attribute.ElementSOType) as RaTweenerElementBase;
							value.hideFlags = HideFlags.HideInInspector;

							RaTweenerElementBase preValue = _tweenElementProperty.GetValue<RaTweenerElementBase>();

							if(preValue != null)
							{
								if(Application.isPlaying)
								{
									Destroy(preValue);
								}
								else
								{
									DestroyImmediate(preValue);
								}
							}

							value.Init(tweenType);
							_tweenElementProperty.SetValue(value);

							EditorUtility.SetDirty(parent);
						}
						catch(Exception e)
						{
							Debug.LogError(e.Message);
						}
						serializedObject.ApplyModifiedProperties();
					}
				}
				else
				{
					EditorUtility.DisplayDialog("Error", error, "Ok");
				}
			}
			serializedObject.Update();
			_editor = null;
		}

		private bool TryGetRaTweenerElementAttribute(Type tweenType, out RaTweenerElementAttribute attribute, out string error)
		{
			if(tweenType.GetCustomAttributes(typeof(RaTweenerElementAttribute), true).FirstOrDefault()
				is RaTweenerElementAttribute extractedAttribute)
			{
				if(typeof(RaTweenerElementBase).IsAssignableFrom(extractedAttribute.ElementSOType))
				{
					if(!extractedAttribute.ElementSOType.IsAbstract)
					{
						attribute = extractedAttribute;
						error = string.Empty;
						return true;
					}
					else
					{
						error = $"Type {extractedAttribute.ElementSOType.Name} defined within {nameof(RaTweenerElementAttribute)}, found above {tweenType.Name}, can't be an Abstract";
					}
				}
				else
				{
					error = $"Type {extractedAttribute.ElementSOType.Name} defined within {nameof(RaTweenerElementAttribute)}, found above {tweenType.Name}, does not derive from {nameof(RaTweenerElementBase)}";
				}
			}
			else
			{
				error = $"No {nameof(RaTweenerElementAttribute)} found above {tweenType.Name}";
			}

			attribute = null;
			return false;
		}
	}
}
