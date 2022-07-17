using UnityEditor;

namespace RaTweening
{
	[CustomEditor(typeof(RaTweenerSerializableElement))]
	public class RaTweenerSerializableEditor : Editor
	{
		#region Variables

		private RaTweenElementEditorDrawer _drawer;

		#endregion

		#region Lifecycle

		protected void OnEnable()
		{
			try
			{
				_drawer = new RaTweenElementEditorDrawer(serializedObject);
			}
			catch { }
		}

		#endregion

		#region Public Methods

		public override void OnInspectorGUI()
		{
			RaTweenerComponentEditor.DrawDefaultInspectorWithoutScript(serializedObject);
			_drawer.Draw();
		}

		#endregion
	}
}
