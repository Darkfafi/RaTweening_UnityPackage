using UnityEngine;

namespace RaTweening.Core
{
	public class RaTweeningCore : MonoBehaviour
	{
		#region Consts

		private const string Name = "<" + nameof(RaTweeningCore) + ">";

		#endregion

		#region Variables

		private readonly RaTweeningProcessor _processor = new RaTweeningProcessor();

		#endregion

		#region Properties

		public static bool HasInstance => _instance != null;

		public static RaTweeningCore Instance
		{
			get
			{
				if(!HasInstance)
				{
					_instance = new GameObject("<"+nameof(RaTweeningCore) +">").AddComponent<RaTweeningCore>();
					DontDestroyOnLoad(_instance);
				}
				return _instance;
			}
		}

		private static RaTweeningCore _instance = null;

		#endregion

		#region Lifecycle

		protected void LateUpdate()
		{
			_processor.Step(Time.deltaTime);
		}

		protected void OnDestroy()
		{
			_processor.Dispose();
		}

		#endregion

		#region Internal Methods

		internal TweenT RegisterTween<TweenT>(TweenT tween)
			where TweenT : RaTweenCore
		{
			return _processor.RegisterTween(tween);
		}

		internal TweenT UnregisterTween<TweenT>(TweenT tween)
			where TweenT : RaTweenCore
		{
			return _processor.UnregisterTween(tween);
		}

		#endregion
	}
}