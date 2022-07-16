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
		private List<EntryData> _sequenceEntries = new List<EntryData>();

		#endregion

		#region Variables

		private RaTweeningProcessor _processor = new RaTweeningProcessor();
		private int _index;
		private EntryData _headEntry;
		private float _time;

		#endregion

		#region Properties

		public override bool IsValid => _sequenceEntries.Count > 0;

		#endregion

		private RaTweenSequence()
			: this(new RaTweenCore[] { })
		{
		
		}

		private RaTweenSequence(RaTweenCore[] tweens)
			: base(0f)
		{
			tweens = tweens ?? new RaTweenCore[] { };
			for(int i = 0; i < tweens.Length; i++)
			{
				AppendTween(tweens[i]);
			}
		}

		internal RaTweenSequence(IList<EntryData> entries)
			: base(0f)
		{
			AppendTweens(entries ?? new EntryData[] { });
		}

		#region Public Methods

		public static RaTweenSequence Create(RaTweenCore[] tweens)
		{
			RaTweenSequence sequence = new RaTweenSequence(tweens);
			RaTweeningCore.Instance.RegisterTween(sequence);
			return sequence;
		}

		public static RaTweenSequence Create(params EntryData[] tweens)
		{
			RaTweenSequence sequence = new RaTweenSequence(tweens);
			RaTweeningCore.Instance.RegisterTween(sequence);
			return sequence;
		}

		public RaTweenSequence AppendTween(RaTweenCore tween, float stagger = 1f)
		{
			return AppendTween(new EntryData(tween, stagger));
		}

		public RaTweenSequence AppendTween(EntryData entry)
		{
			return AppendTweens(new EntryData[] { entry });
		}

		public RaTweenSequence AppendTweens(IList<EntryData> entries)
		{
			if(CanBeModified())
			{
				for(int i = 0, c = entries.Count; i < c; i++)
				{
					var entry = entries[i];
					if(entry.IsValidForAppend)
					{
						RaTweenCore tween = entry.Tween;

						if(RaTweeningCore.HasInstance)
						{
							RaTweeningCore.Instance.UnregisterTween(tween);
						}

						tween.SetStateInternal(State.Data);
						_sequenceEntries.Add(entry);
					}
				}

				CalculateDuration();
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
			EntryData[] entries = new EntryData[_sequenceEntries.Count];

			for(int i = 0, c = entries.Length; i < c; i++)
			{
				entries[i] = _sequenceEntries[i].Clone();
			}

			return new RaTweenSequence(entries);
		}

		protected override void Dispose()
		{
			_processor.Dispose();
			ClearData();
		}

		protected override void Evaluate(float normalizedValue)
		{
			if(_headEntry == null || _headEntry.ReadyToStartNext())
			{
				_index++;
				if(_index < _sequenceEntries.Count)
				{
					_headEntry = _sequenceEntries[_index].Clone();
					_processor.RegisterTween(_headEntry.Tween);
				}
			}

			float newTime = Time;
			float deltaTime = newTime - _time;
			_time = newTime;

			_processor.Step(deltaTime);
		}

		protected override void PerformEvaluation()
		{
			if(_sequenceEntries.Count > 0)
			{
				Evaluate(Progress);
			}
		}

		protected override void SetDefaultValues()
		{

		}

		#endregion

		#region Private Methods

		private void CalculateDuration()
		{
			if(IsInfinite)
			{
				return;
			}

			float newStartTime = 0f;
			float newDuration = 0f;
			for(int i = 0, c = _sequenceEntries.Count; i < c; i++)
			{
				var entry = _sequenceEntries[i];
				
				if(entry.Tween.IsInfiniteLoop)
				{
					SetInfiniteDuration();
					return;
				}

				float entryTweenDuration = entry.Tween.TotalLoopingDuration;
				newDuration = newStartTime + entryTweenDuration;
				newStartTime += entryTweenDuration * entry.Stagger;
			}

			SetDuration(newDuration);
		}

		private void ClearData()
		{
			_headEntry = null;
			_index = -1;
			_time = 0f;
		}

		#endregion

		#region Nested

		public class EntryData
		{
			public readonly RaTweenCore Tween;
			public readonly float Stagger;

			public bool IsValidForAppend => Tween != null && Tween.CanBeModified();

			public EntryData(RaTweenCore tween, float stagger = 1f)
			{
				Tween = tween;
				Stagger = stagger;
			}

			public EntryData Clone()
			{
				return new EntryData(Tween.Clone(), Stagger);
			}

			public bool ReadyToStartNext()
			{
				if(Tween == null)
				{
					return true;
				}
				return Tween.TotalLoopingProgress >= Stagger;
			}
		}

		#endregion
	}
}