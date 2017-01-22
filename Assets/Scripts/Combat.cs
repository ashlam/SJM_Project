using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace SJMGame
{
	public class Combat : IEventListenerContainer
	{
		public int CurrentTurn;
		public int MaxTurn = 150;
		List<Character> team1,team2;

		internal List<Character> Team1 { get {return team1;} }
		internal List<Character> Team2 { get {return team2;} }
		internal Constant.CombatType CombatType {get;set;}

		internal Constant.CombatResult CombatResult {get;set;}

		
		int currentSkillID = -1;
		public Combat()
		{
			team1 = new List<Character>();
			team2 = new List<Character>();
		}

		internal void OnCombatBegin(object args)
		{
			CurrentTurn = 0;
			// currentActor = null;
			// currentSkillID = -1;
			Debug.Log("- New Combat Begin!");
			
			// EventCenter.CreateInstance().InvokeEvent(Constant.EventCode_S2C.System_Combat_TurnBegin,this,currentActor);
			//回合开始
			EventCenter.CreateInstance().InvokeEvent(Constant.EventCode_S2C.System_Combat_TurnBegin,this,null);
		}

		internal void OnTurnStart(object args)
		{
			/*
			1.打雷下雨之类
			2.得到当前行动对象
			*/

			Debug.Log("dong!");
			Character currentActor = this.GetCharacterCouldDoAction();
			EventCenter.CreateInstance().InvokeEvent(Constant.EventCode_S2C.System_Combat_CalculateBeforeDoAction,currentActor,this.GetAllCharacters());
			EventCenter.CreateInstance().InvokeEvent(Constant.EventCode_S2C.System_Combat_ActionBegin,currentActor,this);
		}

		internal void OnActionStart(object args)
		{
			Character currentActor = args as Character;
			if(currentActor == null) 
			{ 
				return; 
			}
			
			if(currentActor.IsAlive)
			{
				Constant.CombatResult combatResult = GetCurrentCombatResult();

			}
		}

		internal void OnTurnFinished(object args)
		{
			Character currentActor = args as Character;
			CurrentTurn ++;
			Debug.Log("turn fininshed:" + CurrentTurn);
			this.CombatResult = this.GetCurrentCombatResult();
			if(this.CombatResult == Constant.CombatResult.None)
			{
				if(currentActor != null)
				{
					if(currentActor.IsAlive)
					{
						EventCenter.CreateInstance().InvokeEvent(Constant.EventCode_S2C.System_Combat_CalculateAfterDoAction,currentActor,null);
					}
					else
					{
						// EventCenter.CreateInstance().
					}
				}
			}
			else
			{
				EventCenter.CreateInstance().InvokeEvent(Constant.EventCode_S2C.System_Combat_End,this,this);
			}
			
		}

		internal void OnCalculateBuffBefore(object args)
		{
			Character currentActor = args as Character;
			this.CombatResult = this.GetCurrentCombatResult();
			if(this.CombatResult != Constant.CombatResult.None)
			{
				//Combat over
				EventCenter.CreateInstance().InvokeEvent(Constant.EventCode_S2C.System_Combat_End,this,this);
			}
			else if(!currentActor.IsAlive)
			{
				EventCenter.CreateInstance().InvokeEvent(Constant.EventCode_S2C.System_Combat_TurnEnd,this,this);
			}
			else
			{

			}
		}

		internal void OnCombatFinished(object args)
		{
			// Constant.EventCode_S2C.turn
		}


		private Constant.CombatResult GetCurrentCombatResult()
        {
            // if(currentCombat == null || team1 == null || team2 == null)
            //     return Constant.CombatResult.None;
            Constant.CombatResult result = Constant.CombatResult.None;
            System.Predicate<Character> matched = (c)=>{ return c.IsAlive; };
            if(team1.FindAll(matched).Count + team2.FindAll(matched).Count == 0)
            {
                result = Constant.CombatResult.Draw;
            }
            else if(team1.FindAll(matched).Count == 0)
            {
                result = Constant.CombatResult.Team2Win;
            }
            else if(team2.FindAll(matched).Count == 0)
            {
                result = Constant.CombatResult.Team1Win;
            }
            else if(this.CurrentTurn >= this.MaxTurn)
            {
                result = Constant.CombatResult.Draw;
            }
            return result;
        }

		private Character GetCharacterCouldDoAction()
		{
			List<Character> allAlivedCharacters = this.GetAllAlivedCharacters();
			if(allAlivedCharacters ==null || allAlivedCharacters.Count == 0)
				return null;
			allAlivedCharacters.Sort((c1,c2)=>{ return c2.ModelObj.Property_SPD.CompareTo(c1.ModelObj.Property_SPD); });
			int minTimes = 0;
			foreach(Character c in allAlivedCharacters)
			{
				int tempTotal = Mathf.CeilToInt((100-c.ActionPoint)/CombatManager.CreateInstance().GetActionPointIncrease(c,allAlivedCharacters));
				minTimes = minTimes == 0 ? tempTotal : Mathf.Min(minTimes, tempTotal);
			}
			foreach(Character c in allAlivedCharacters)
			{
				c.ActionPoint = Mathf.Min(100, c.ActionPoint + minTimes * CombatManager.CreateInstance().GetActionPointIncrease(c,allAlivedCharacters));
			}
			foreach(Character c in allAlivedCharacters)
			{
				c.ActionPoint = Mathf.Min(100, c.ActionPoint +  CombatManager.CreateInstance().GetActionPointIncrease(c,allAlivedCharacters));
				if(c.ActionPoint >= 100)
				{
					return c;
				}
			}
			return null;
		}


        private List<Character> GetAllAlivedCharacters()
        {
            List<Character> result = new List<Character>();
            result.AddRange(team1.FindAll((c)=>{ return c.IsAlive; }));
            result.AddRange(team2.FindAll((c)=>{ return c.IsAlive; }));
            return result;
        }

        private List<Character> GetAllCharacters()
        {
            return GetAllCharacters((c)=>{return true;});
        }

        private List<Character> GetAllCharacters(System.Predicate<Character> matched)
        {
            List<Character> result = new List<Character>();
            result.AddRange(team1.FindAll(matched));
            result.AddRange(team2.FindAll(matched));
            return result;
        }

		internal void OutputCombatStatus()
		{
			List<Character> allCharacters = GetAllCharacters();
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			foreach(Character c in allCharacters)
			{
				sb.Append(string.Format("[{0}]HP:{1}/{2},SP:{3},{4}",
					c.ModelObj.Name,c.ModelObj.HP,c.ModelObj.MaxHP,c.ModelObj.SP,c.ModelObj.MaxSP));
			}
			Debug.Log(string.Format("Turn {0}:\n {1}",this.CurrentTurn,sb.ToString()));
		}

		internal void Reset()
        {
            foreach(Character c in GetAllCharacters())
            {
                EventListener listener1 = new EventListener()
                {
                    EventHandle = c.OnActionStart,
                    Owner = c,
                    Priority = 99,
                    IsUnique = true,
                };
                EventCenter.CreateInstance().RegisterListener(Constant.EventCode_S2C.System_Combat_ActionBegin,listener1);
            }
        }

	}

	/*
		turn begin..
			confirm currentActor
			calculate buff before action
			if die then { die and action end } else { select action }
			select action
				if (action failed) then {action end} or {action start}
			action start
			if (spell or charging) then {turn end} else {do action}
			action end 
			calculate buff after action
				if die then { die }
			calculate combat result
				if finished then { finish combat } else { turn end }
		turn end
		next turn

	 */
}