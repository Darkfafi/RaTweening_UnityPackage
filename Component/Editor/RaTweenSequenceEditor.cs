using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace RaTweening
{
	[CustomEditor(typeof(RaTweenerSequenceElement))]
	public class RaTweenSequenceEditor : Editor
	{
		#region Variables

		private RaTweenElementEditorDrawer _drawer;
		private ReorderableList _orderableList;
		private SerializedProperty _tweensProperty;
		private bool _editMode = true;
		private Dictionary<Object, Editor> _cachedEditors = new Dictionary<Object, Editor>();
		private bool _isGlobalExpanded = false;

		#endregion

		protected void OnEnable()
		{
			try
			{
				_drawer = new RaTweenElementEditorDrawer(serializedObject);
				_tweensProperty = serializedObject.FindProperty("_sequenceElements");
				_orderableList = new ReorderableList(serializedObject, _tweensProperty, true, true, false, false);
				_orderableList.drawElementCallback = OnDrawNestedItem;
				_orderableList.drawHeaderCallback = OnDrawHeader;
			}
			catch { }
		}

		#region Public Methods

		public override void OnInspectorGUI()
		{
			_drawer.Draw();

			RaTweenerSequenceElement self = serializedObject.targetObject as RaTweenerSequenceElement;

			EditorGUILayout.BeginVertical("framebox");
			{
				EditorGUILayout.BeginHorizontal();
				{
					EditorGUILayout.LabelField(self.GetName(), EditorStyles.boldLabel);
					if(_editMode)
					{
						if(IconButton(_isGlobalExpanded ? "scenevis_visible_hover@2x" : "scenevis_visible@2x", 20f))
						{
							_isGlobalExpanded = !_isGlobalExpanded;

							if(_tweensProperty != null && _tweensProperty.isArray)
							{
								for(int i = 0; i < _tweensProperty.arraySize; i++)
								{
									var element = _tweensProperty.GetArrayElementAtIndex(i);
									if(element != null)
									{
										element.isExpanded = _isGlobalExpanded;
									}
								}
							}
						}
					}
					if(IconButton("CollabCreate Icon", 20))
					{
						RaTweenerComponentEditor.CreateTweenSearchWindow(OnTweenSelectedToAdd);
					}
					if(IconButton(_editMode ? "CollabMoved Icon" : "CollabEdit Icon", 20))
					{
						_editMode = !_editMode;
					}
				}
				EditorGUILayout.EndHorizontal();
				GUILayout.Space(5);

				serializedObject.Update();

				if(_editMode)
				{
					if(_tweensProperty != null && _tweensProperty.isArray)
					{
						for(int i = 0; i < _tweensProperty.arraySize; i++)
						{
							DrawElement(_tweensProperty.GetArrayElementAtIndex(i), i);
						}
					}
				}
				else if(_orderableList != null)
				{
					_orderableList.DoLayoutList();
				}
				serializedObject.ApplyModifiedProperties();
			}
			EditorGUILayout.EndVertical();
		}

		#endregion

		#region Private Methods

		private void OnTweenSelectedToAdd(System.Type tweenType)
		{
			if(RaTweenerComponentEditor.TryAddTween(serializedObject, tweenType, out RaTweenerElementBase element))
			{
				RaTweenerSequenceElement self = serializedObject.targetObject as RaTweenerSequenceElement;
				if(self.RegisterTweenElement(element))
				{
					element.SetLoopingAllowStage(RaTweenerElementBase.LoopAllowStage.ToFinite);
					AssetDatabase.SaveAssets();
					EditorUtility.SetDirty(serializedObject.targetObject);
				}
				else
				{
					RaTweenerComponentEditor.TryRemoveTween(element, true);
				}
			}
		}

		private void DrawElement(SerializedProperty serializedProp, int index)
		{
			var item = serializedProp.objectReferenceValue;

			if(item == null)
			{
				EditorGUILayout.LabelField("Item Empty");
				return;
			}

			EditorGUILayout.BeginHorizontal("helpBox");
			{
				if(item is RaTweenerElementBase element)
				{
					string oldName = element.GetName();
					EditorGUILayout.BeginHorizontal();
					{
						EditorGUILayout.LabelField(index + ":", GUILayout.Width(25));
						string newName = EditorGUILayout.TextField(oldName);
						if(oldName != newName)
						{
							element.SetName(newName);
						}
					}
					EditorGUILayout.EndHorizontal();
					if(IconButton(serializedProp.isExpanded ? "scenevis_visible_hover@2x" : "scenevis_visible@2x", 20f))
					{
						serializedProp.isExpanded = !serializedProp.isExpanded;
					}
					if(IconButton("CollabDeleted Icon", 20))
					{
						RaTweenerSequenceElement self = serializedObject.targetObject as RaTweenerSequenceElement;
						if(self.UnregisterTweenElement(element))
						{
							RaTweenerComponentEditor.TryRemoveTween(element, false);
							AssetDatabase.SaveAssets();
							EditorUtility.SetDirty(serializedObject.targetObject);
							return;
						}
					}
				}
			}
			EditorGUILayout.EndHorizontal();
			if(serializedProp.isExpanded)
			{
				if(!_cachedEditors.TryGetValue(item, out Editor editor))
				{
					_cachedEditors[item] = editor = CreateEditor(item);
				}
				EditorGUILayout.BeginVertical("frameBox");
				{
					editor.OnInspectorGUI();
					editor.serializedObject.ApplyModifiedProperties();
				}
				EditorGUILayout.EndVertical();
			}
		}

		private void OnDrawHeader(Rect rect)
		{
			GUILayout.BeginHorizontal();
			{
				GUI.Label(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), string.Empty);
			}
			GUILayout.EndHorizontal();
		}

		private void OnDrawNestedItem(Rect rect, int index, bool isActive, bool isFocused)
		{
			SerializedProperty serializedSequenceItem = _orderableList.serializedProperty.GetArrayElementAtIndex(index);
			RaTweenerElementBase sequenceElement = serializedSequenceItem.objectReferenceValue as RaTweenerElementBase;

			int currentWidth = 0;

			string oldName = sequenceElement.GetName();
			EditorGUI.LabelField(new Rect(rect.x, rect.y, currentWidth += 25, EditorGUIUtility.singleLineHeight), index + ":");
			string newName = EditorGUI.TextField(new Rect(currentWidth + rect.x, rect.y, 275 - currentWidth, EditorGUIUtility.singleLineHeight), oldName);

			if(oldName != newName)
			{
				sequenceElement.SetName(newName);
			}

			int bWidth = 20;
			if(IconButton(new Rect(rect.x + rect.width - bWidth, rect.y, bWidth, EditorGUIUtility.singleLineHeight), "CollabDeleted Icon"))
			{
				RaTweenerSequenceElement self = serializedObject.targetObject as RaTweenerSequenceElement;
				if(self.UnregisterTweenElement(sequenceElement))
				{
					RaTweenerComponentEditor.TryRemoveTween(sequenceElement, false);
					AssetDatabase.SaveAssets();
					EditorUtility.SetDirty(serializedObject.targetObject);
				}
			}
		}

		private bool IconButton(Rect rect, string icon)
		{
			return GUI.Button(rect, EditorGUIUtility.FindTexture(icon), new GUIStyle("label"));
		}

		private bool IconButton(string icon, float size)
		{
			return GUILayout.Button(EditorGUIUtility.FindTexture(icon), new GUIStyle("label"), GUILayout.MaxWidth(size), GUILayout.MaxHeight(size));
		}

		#endregion
	}
}
