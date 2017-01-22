using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace SJMGame
{

	public class EventCenter
	{
		static EventCenter instance;
		internal static EventCenter CreateInstance()
		{
			if(instance == null)
			{
				instance = new EventCenter();
			}
			return instance;
		}

		Dictionary<Constant.EventCode_S2C,List<EventListener>> allListeners;
		private EventCenter()
		{
			allListeners = new Dictionary<Constant.EventCode_S2C,List<EventListener>>();
		}

		internal void RegisterListener(Constant.EventCode_S2C code,EventListener listener)
		{
			if(!allListeners.ContainsKey(code))
			{
				/*user story: 
					all character has event of UnderDamage,
					then char1 cast a skill to support an unique full-protected shield to team
					and char 2 cast a same skill        

					...so event must know the source of where itself come from
					
				*/
				allListeners.Add(code, new List<EventListener>() { listener} );
			}
			else
			{
				List<EventListener> existListeners = allListeners[code];
				if(!existListeners.Exists((li) => {
					return li.SameAs(listener);
				}))
				{
					existListeners.Add(listener);
				}
			}
		}

		internal void UnregisterEventListener(Constant.EventCode_S2C code,EventSource source, IEventListenerContainer owner)
		{
			List<EventListener> tempListeners = null;
			if(allListeners.ContainsKey(code))
			{
				tempListeners = allListeners[code];
				tempListeners.RemoveAll((li)=>{ return li.Source.HasSameValue(source) && li.Owner == owner ; });
			}
			if(tempListeners != null && tempListeners.Count == 0)
			{
				allListeners.Remove(code);
			}
		}

		internal void UnregisterEventListenerByOwner(IEventListenerContainer owner)
		{
			foreach(Constant.EventCode_S2C code in allListeners.Keys)
			{
				List<EventListener> tempListeners = allListeners[code];
				tempListeners.RemoveAll((li)=>{ return li.Owner == owner; });
				if(tempListeners.Count == 0)
				{
					allListeners.Remove(code);
				}
			}
		}


		internal EventListener GetEventListener(Constant.EventCode_S2C code,EventSource source,IEventListenerContainer owner)
		{
			EventListener result = null;
			if(allListeners.ContainsKey(code))
			{
				List<EventListener> tempListeners = allListeners[code];
				if(tempListeners != null)
				{
					result = tempListeners.Find((li)=>{ 
						return li.Source.HasSameValue(source);
					});
				}
			}
			return result;
		}


		internal void InvokeEvent(Constant.EventCode_S2C code,IEventListenerContainer eventOwner,object param)
		{
			if(allListeners.ContainsKey(code))
			{
				List<EventListener> tempListeners = allListeners[code].FindAll((li)=>{ return li.Owner == eventOwner; });
				if(tempListeners.Count >0)
				{
					tempListeners.Sort((l1,l2)=>{ return l2.Priority.CompareTo(l1.Priority); });
					foreach(EventListener li in tempListeners)
					{
						li.EventHandle.Invoke(param);
						if(!li.ShouldCountinueEvent)
						{
							break;
						}
					}
				}
			}			
		}
	}


	//监听事件的对象
	//比如一个比较复杂的buff可能提供一个或多个listener
	//addBuff()->registerEvent(code1,buff);registerEvent(code2,buff);
	public class EventListener
	{
		internal int Priority {get;set;}
		internal delegate void InvokeEventHandle(object o);
		internal bool ShouldCountinueEvent {get;set;}
		///这个监听对象所对应的执行函数，在Invoke的时候传参数
		internal InvokeEventHandle EventHandle;
		internal bool IsUnique {get;set;}

		internal EventSource Source {get;set;}
		///这个监听者所属的对象
		internal IEventListenerContainer Owner {get;set;}

		internal bool SameAs(EventListener other)
		{
			return other != null &&
				other.Owner == this.Owner && this.Source.HasSameValue(other.Source);
		}

		public EventListener() {}

		public EventListener(EventSource source) {
			this.Source = source;
		}

		public EventListener(int feature,Constant.EventSourceType type)
		{
			this.Source = new EventSource()
			{
				SourceFeature = feature,
				SourceType = type,
			};
		}
	}

	public struct EventSource
	{
		public Constant.EventSourceType SourceType {get; set;}
		public int SourceFeature {get;set;}

		internal bool HasSameValue(EventSource other)
		{
			return this.SourceFeature == other.SourceFeature && this.SourceType == other.SourceType;
		}
	}


	public interface IEventListenerContainer
	{
	}


}