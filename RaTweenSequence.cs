using System.Collections.Generic;
using RaTweening.Core;
using UnityEngine;
using static RaTweening.RaTweenSequence;

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

		public static RaTweenSequence Create(params EntryData[] tweens)
		{
			RaTweenSequence sequence = new RaTweenSequence(tweens);
			RaTweeningCore.Instance.RegisterTween(sequence);
			return sequence;
		}

		public RaTweenSequence AppendTween(RaTweenCore tween,
			float stagger = 1f,
			StaggerType staggerType = StaggerType.FinalLoopExclDelay)
		{
			return AppendTween(tween.ToSequenceEntry(stagger, staggerType));
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
			if(_sequenceEntries.Count > 0)
			{
				if(_headEntry.IsEmpty || _headEntry.ReadyToStartNext())
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
		}

		protected override float CalculateEvaluation()
		{

			return Progress;
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
				newDuration = Mathf.Max(newStartTime + entryTweenDuration, newDuration);

				switch(entry.StaggerType)
				{
					case StaggerType.Total:
						newStartTime += entryTweenDuration * entry.Stagger;
						break;
					case StaggerType.FinalLoop:
						newStartTime += entryTweenDuration - (entry.Tween.TotalDuration * (1f - entry.Stagger));
						break;
					case StaggerType.FinalLoopExclDelay:
						newStartTime += entryTweenDuration - (entry.Tween.Duration * (1f - entry.Stagger));
						break;
					default:
						throw new System.Exception($"Not Implemented {entry.StaggerType}");
				}
			}

			SetDuration(newDuration);
		}

		private void ClearData()
		{
			_headEntry = default;
			_index = -1;
			_time = 0f;
		}

		#endregion

		#region Nested

		public struct EntryData
		{
			public readonly RaTweenCore Tween;

			public float Stagger
			{
				get; private set;
			}

			public StaggerType StaggerType
			{
				get; private set;
			}

			public bool IsEmpty => Tween == null;

			public bool IsValidForAppend => !IsEmpty && Tween.CanBeModified();

			private EntryData(RaTweenCore tween, float stagger, StaggerType staggerType)
			{
				Tween = tween;
				Stagger = stagger;
				StaggerType = staggerType;
			}

			public static EntryData Create(RaTweenCore tween, float stagger = 1f, StaggerType staggerType = StaggerType.FinalLoopExclDelay)
			{
				return new EntryData(tween, stagger, staggerType);
			}

			public EntryData SetStaggerType(StaggerType staggerType)
			{
				StaggerType = staggerType;
				return this;
			}

			public EntryData SetStagger(float stagger)
			{
				Stagger = Mathf.Clamp01(stagger);
				return this;
			}

			public EntryData Clone()
			{
				return new EntryData(Tween.Clone(), Stagger, StaggerType);
			}

			public bool ReadyToStartNext()
			{
				if(Tween == null)
				{
					return true;
				}

				switch(StaggerType)
				{
					case StaggerType.Total:
						return Tween.TotalLoopingProgress >= Stagger;
					case StaggerType.FinalLoop:
						return Tween.HasReachedLoopEnd && Tween.TotalProgress >= Stagger;
					case StaggerType.FinalLoopExclDelay:
						return Tween.HasReachedLoopEnd && Tween.HasNoDelayRemaining && Tween.Progress >= Stagger;
					default:
						throw new System.Exception($"Not Implemented {StaggerType}");
				}
			}
		}

		public enum StaggerType
		{
			Total,
			FinalLoop,
			FinalLoopExclDelay
		}

		#endregion
	}

	public static class RaTweenSequenceExtensions
	{
		public static EntryData ToSequenceEntry(this RaTweenCore tween, float stagger = 1f, StaggerType staggerType = StaggerType.FinalLoopExclDelay)
		{
			return EntryData.Create(tween, stagger, staggerType);
		}
	}
}