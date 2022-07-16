using System.Collections.Generic;
using RaTweening.Core;
using UnityEngine;

namespace RaTweening
{
	[RaTweenerElement(typeof(RaTweenerSequenceElement))]
	public sealed class RaTweenSequence : RaTweenCore
	{
		#region Editor Variables

		[SerializeField, SerializeReference]
		private List<RaTweenCore> _tweens = new List<RaTweenCore>();

		#endregion

		#region Variables

		private RaTweeningProcessor _processor = new RaTweeningProcessor();
		private int _index;
		private RaTweenCore _currentTween;
		private float _time;

		#endregion

		#region Properties

		public override bool IsValid => _tweens.Count > 0;

		#endregion

		public RaTweenSequence()
			: this(null)
		{
		
		}

		public RaTweenSequence(RaTweenCore[] tweens)
			: base(0f)
		{
			tweens = tweens ?? new RaTweenCore[] { };
			for(int i = 0; i < tweens.Length; i++)
			{
				AppendTween(tweens[i]);
			}
		}

		#region Public Methods

		public static RaTweenSequence Create(params RaTweenCore[] tweens)
		{
			RaTweenSequence sequence = new RaTweenSequence(tweens);
			RaTweeningCore.Instance.RegisterTween(sequence);
			return sequence;
		}

		public RaTweenSequence AppendTween(RaTweenCore tween)
		{
			if(CanBeModified() && tween.CanBeModified())
			{
				if(RaTweeningCore.HasInstance)
				{
					RaTweeningCore.Instance.UnregisterTween(tween);
				}

				if(tween.IsInfiniteLoop)
				{
					SetInfiniteDuration();
				}

				if(!IsInfinite)
				{
					SetDuration(Duration + tween.TotalLoopingDuration);
				}

				tween.SetStateInternal(State.Data);
				_tweens.Add(tween);
			}
			return this;
		}

		#endregion

		#region Protected Methods

		protected override void OnSetup()
		{
			base.OnSetup();
			ClearData();
		}

		protected override void OnLoop()
		{
			base.OnLoop();
			ClearData();
		}

		protected override RaTweenCore CloneSelf()
		{
			var copyTweens = new RaTweenCore[_tweens.Count];

			for(int i = 0; i < copyTweens.Length; i++)
			{
				copyTweens[i] = _tweens[i].Clone();
			}

			RaTweenSequence sequence = new RaTweenSequence(copyTweens);
			return sequence;
		}

		protected override void Dispose()
		{
			_processor.Dispose();
			ClearData();
		}

		protected override void Evaluate(float normalizedValue)
		{
			if(_currentTween == null || _currentTween.IsCompleted)
			{
				_index++;
				if(_index < _tweens.Count)
				{
					_processor.RegisterTween(_currentTween = _tweens[_index].Clone());
				}
			}

			float newTime = Time;
			float deltaTime = newTime - _time;
			_time = newTime;

			_processor.Step(deltaTime);
		}

		protected override void PerformEvaluation()
		{
			if(_tweens.Count > 0)
			{
				Evaluate(Progress);
			}
		}

		protected override void SetDefaultValues()
		{

		}

		#endregion

		#region Private Methods

		private void ClearData()
		{
			_currentTween = null;
			_index = -1;
			_time = 0f;
		}

		#endregion
	}
}