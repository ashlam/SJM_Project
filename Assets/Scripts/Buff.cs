using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace SJMGame
{
	public abstract class Buff
	{
		protected IEventListenerContainer owner;
		internal abstract void OnGained(IEventListenerContainer owner);
	}

	public class Buff_FullProtectShield:Buff
	{
		internal override void OnGained(IEventListenerContainer owner)
		{
			this.owner = owner;
			EventListener listener1 = new EventListener()
			{
				Priority = 5,
				EventHandle = this.ShowDisplayInfo_OnTrigger,
				Source = new EventSource()
				{
					SourceType = Constant.EventSourceType.Buff,
					SourceFeature = 103,
				},
				Owner = this.owner,
				ShouldCountinueEvent = false,
				IsUnique = true,
			};
			EventCenter.CreateInstance().RegisterListener(Constant.EventCode_S2C.InflictDamage_Life,listener1);

			EventListener listener2 = new EventListener()
			{
				Priority = 1,
				EventHandle = this.OnTurnBegining,
				Source = new EventSource()
				{
					SourceType = Constant.EventSourceType.Buff,
					SourceFeature = 103,
				},
				Owner = this.owner,
				ShouldCountinueEvent = true,
				IsUnique = true,
			};
			EventCenter.CreateInstance().RegisterListener(Constant.EventCode_S2C.System_Combat_TurnBegin,listener2);
		}

		private void ShowDisplayInfo_OnTrigger(object arg)
		{

			Debug.Log("The shield block this damage.");
			EventCenter.CreateInstance().UnregisterEventListener(Constant.EventCode_S2C.InflictDamage_Life,
				new EventSource(){ SourceFeature = 103, SourceType = Constant.EventSourceType.Buff }, this.owner);
		}

		private void OnTurnBegining(object arg)
		{
			Debug.Log("The shield regenesised");
			EventListener listener1 = new EventListener()
			{
				Priority = 1,
				EventHandle = this.ShowDisplayInfo_OnTrigger,
				Source = new EventSource()
				{
					SourceType = Constant.EventSourceType.Buff,
					SourceFeature = 103,
				},
				Owner = this.owner,
				ShouldCountinueEvent = false,
				IsUnique = true,
			};
			EventCenter.CreateInstance().RegisterListener(Constant.EventCode_S2C.InflictDamage_Life,listener1);
		}
	}
}


