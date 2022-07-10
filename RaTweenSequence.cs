using System.Collections.Generic;
using RaTweening.Core;
using UnityEngine;

namespace RaTweening
{
	public class RaTweenSequence : RaTweenCore
	{
		#region Variables

		private List<RaTweenCore> _tweens = new List<RaTweenCore>();
		private RaTweeningProcessor _processor = new RaTweeningProcessor();
		private int _index;
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
			: base(0f, 0f)
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
				tween.SetStateInternal(State.Dead);
				_tweens.Add(tween);
				SetDuration(Duration + tween.TotalDuration);
			}
			return this;
		}

		#endregion

		#region Protected Methods

		protected override void OnSetup()
		{
			base.OnSetup();
			_index = -1;
			_time = 0f;
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
			_index = -1;
		}

		protected override void Evaluate(float normalizedValue)
		{
			int index = Mathf.FloorToInt(_tweens.Count * normalizedValue);
			if(_index != index)
			{
				_index = index;
				if(index < _tweens.Count)
				{
					_processor.RegisterTween(_tweens[_index].Clone());
				}
			}

			float newTime = Time;
			float deltaTime = newTime - _time;
			_time = newTime;

			_processor.Step(deltaTime);
		}

		protected override void PerformEvaluation()
		{
			Evaluate(Progress);
		}

		protected override void SetDefaultValues()
		{

		}

		#endregion
	}
}