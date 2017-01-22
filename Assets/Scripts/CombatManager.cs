using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

namespace SJMGame
{
    public class CombatManager : IEventListenerContainer
    {


        BattleField currentBattleField = null;
        List<Character> team1,team2 = null;

        #region ------------ Singleton -------------
        static CombatManager instance = null;
        public static CombatManager CreateInstance()
        {
            if (null == instance)
            {
                instance = new CombatManager();
            }
            return instance;
        }


        private CombatManager()
        {
            currentCombat = new Combat();
            team1 = currentCombat.Team1;
            team2 = currentCombat.Team2;
        }

        #endregion


        Combat currentCombat = null;


        internal Combat CurrentCombat{ get { return currentCombat; }}

        internal void InitEvents()
        {
            EventListener lis_combatStart = new EventListener();

            EventListener lis1 = new EventListener(1,Constant.EventSourceType.System)
            {
                Priority = 1,
                Owner = currentCombat,
                EventHandle = currentCombat.OnTurnStart,
            };
            EventCenter.CreateInstance().RegisterListener(Constant.EventCode_S2C.System_Combat_TurnBegin,lis1);

            
        }

        void Reset()
        {
            currentCombat.Reset();
        }

        internal void CombatStart()
        {
            // currentCombat.Reset();
            //开始计算actionPoint
            // this.CombatCalculateActionPoint();
            
            Debug.Log("ddd");
            //战斗开始
            EventCenter.CreateInstance().InvokeEvent(Constant.EventCode_S2C.System_Combat_Begin,currentCombat,null);
        }


        // private void TurnStart(Character c)
        // {
        //     EventCenter.CreateInstance().InvokeEvent(Constant.EventCode_S2C.System_Combat_TurnBegin,c,null);
        // }

        internal float GetActionPointIncrease(Character target,List<Character> allAliveCharacters)
        {
            float result = 0.2f;
            int totalSpeed = 0;
            foreach(Character c in allAliveCharacters)
            {
                totalSpeed += c.ModelObj.Property_SPD;
            }
            if(totalSpeed >0)
            {
                result = Mathf.Max(0.2f,(float)target.ModelObj.Property_SPD / totalSpeed);
            }
            return result;
        }

        


        /// <summary>
        /// 尝试使用技能
        /// 注意：走到这里只能说明满足行动设定，但不一定能放出来（比如“必定-复活”）
        /// </summary>
        /// <param name="character"></param>
        /// <param name="skill"></param>
        internal void TryToCastSkill(Character character, Skill skill)
        {
            throw new System.NotImplementedException();
		}

		internal int GetSkillIDByActionSettings(Character c)
		{
			int result = -1;
            List<Models.ActionSetting> actionSettings = c.ActionSettings;
			foreach(Models.ActionSetting tempSetting in actionSettings)
			{
				bool checkResult = ConditionValidation.CheckActionSettingCondition(tempSetting,c);
				if(checkResult && tempSetting.SkillID == Constant.SKILLID_CONSIDER)
				{
					continue;
				}
				if(checkResult)
				{
					return tempSetting.SkillID;
				}
			}
			return result;
		}

        
        

        internal BattleField GetCurrentBattleField()
        {
            return currentBattleField;
        }

        internal void RegisterBattleField(BattleField battleField)
        {
            this.currentBattleField = battleField;
        }

        internal void ClearAllTeams()
        {
            team1.Clear();
            team2.Clear();
        }

        internal void AddCharacterToTeam(Character c,int forceID)
        {
            c.ForceID = forceID;
            if(forceID == 1)
            {
                team1.Add(c);
            }
            else
            {
                team2.Add(c);
            }
            this.OnCharacterCreated(c);
        }

        private void OnCharacterCreated(Character c)
        {
            // EventListener event_

            EventListener event_actionFinished = new EventListener()
            {
                Priority = 1,
                Owner = c,
                EventHandle = currentCombat.OnTurnFinished,
            };
            EventCenter.CreateInstance().RegisterListener(Constant.EventCode_S2C.FocusForce_Begin,event_actionFinished);
            EventCenter.CreateInstance().RegisterListener(Constant.EventCode_S2C.ChargingPower_Begin,event_actionFinished);
            EventCenter.CreateInstance().RegisterListener(Constant.EventCode_S2C.Attack,event_actionFinished);
        }

        internal Character GetCharacterById(int characterId)
        {
            Character result = null;
            System.Predicate<Character> matched = (c)=> {return c.ID == characterId;}; 
            result = team1.Find(matched) ?? team2.Find(matched);
            return result;
        }

        
        internal Constant.SkillCastFailedReason GetSkillCastFailedReason(int characterId,int skillId)
        {
            Constant.SkillCastFailedReason reason = Constant.SkillCastFailedReason.None;
            Character caster = GetCharacterById(characterId);
            JsonData jd_skill = SkillManager.CreateInstance().GetSkillJsonDataByID(skillId);
            if(jd_skill == null)
            {
                Debug.Log("skillId = "+ skillId);
            }
            if(!CheckSkillCostCondition(caster,jd_skill["cost"]))
            {
                reason = Constant.SkillCastFailedReason.NotEnoughSP;
            }
            else if(!CheckSkillSelectCondition(caster,jd_skill["selection"]))
            {
                reason = Constant.SkillCastFailedReason.NotAvailableTarget;
            }
            return reason;
        }

        delegate bool SkillValidateHandle(Character caster,JsonData jd);
        private bool CheckSkillCostCondition(Character caster,JsonData jd_cost)
        {
            bool result = true;
            if(jd_cost != null)
            {
                SkillValidateHandle checkCostHandle = null;
                for(int i = 0;i< jd_cost.Count; i++)
                {
                    Constant.SkillCostType costType = (Constant.SkillCostType)Global.INT32(jd_cost[i]["type"]);
                    switch(costType)
                    {
                        case Constant.SkillCostType.SP:
                            checkCostHandle = (c,jd) =>{
                                return c.ModelObj.SP >= Global.INT32(jd["value"]);
                            };
                        break;
                        case Constant.SkillCostType.HP:
                            checkCostHandle = (c,jd) =>{
                                return c.ModelObj.HP >= Global.INT32(jd["value"]);
                            };
                        break;
                    }
                    if(checkCostHandle != null)
                    {
                        result = checkCostHandle(caster,jd_cost[i]);
                        if(!result)
                            return false;
                    }
                    
                }
                
            }
            return result;
        }

        internal Constant.SkillCastStance GetRequiredSkillStance(int skillId)
        {
            Constant.SkillCastStance stance = Constant.SkillCastStance.Immediately;
            JsonData jd_skill = SkillManager.CreateInstance().GetSkillJsonDataByID(skillId);
            if(jd_skill != null && jd_skill["cast"] != null)
            {
                stance = (Constant.SkillCastStance)(Global.INT32(jd_skill["cast"]["type"]));                
            }
            return stance;
        }

        private bool CheckSkillSelectCondition(Character caster,JsonData jd_selection)
        {
            bool result = true;
            SkillValidateHandle checkAlignmentHandle = (c,jd)=>{return true;};
            SkillValidateHandle checkTargetHandle = (c,jd) =>{return true;};
            Constant.SkillSelectionType_Alignment alignmentType = (Constant.SkillSelectionType_Alignment) Global.INT32(jd_selection["alignment"]);
            List<Character> availableTargets = GetCharactersBySelectionType(caster,alignmentType);
            result = availableTargets.Count >0;
            return result;
        }

        private List<Character> GetFriendTeam(Character caster,bool includeSelf = true)
        {
            List<Character> result = new List<Character>();
            result.AddRange(caster.ForceID == 1 ? team1 : team2);
            if(!includeSelf)
            {
                result.Remove(result.Find((c)=>{ return c.ID == caster.ID; }));
            }
            return result;
        }

        private List<Character> GetEnemyTeam(Character caster)
        {
            List<Character> result = new List<Character>();
            result.AddRange(caster.ForceID == 1 ? team2 : team1);
            return result;
        }

        private Character GetRandomCharacterFromList(List<Character> characters)
        {
            Character result = null;
            int rndIndex = Random.Range(0,characters.Count);
            result = characters[rndIndex];
            return result;
        }

        // private List<Character> GetAllAlivedCharacters()
        // {
        //     List<Character> result = new List<Character>();
        //     result.AddRange(team1.FindAll((c)=>{ return c.IsAlive; }));
        //     result.AddRange(team2.FindAll((c)=>{ return c.IsAlive; }));
        //     return result;
        // }

        // private List<Character> GetAllCharacters()
        // {
        //     return GetAllCharacters((c)=>{return true;});
        // }

        // private List<Character> GetAllCharacters(System.Predicate<Character> matched)
        // {
        //     List<Character> result = new List<Character>();
        //     result.AddRange(team1.FindAll(matched));
        //     result.AddRange(team2.FindAll(matched));
        //     return result;
        // }

        internal void DoSkillCost(Character caster,int skillId)
        {
            JsonData jd_skill = SkillManager.CreateInstance().GetSkillJsonDataByID(skillId);
            if(jd_skill != null && jd_skill["cost"] != null)
            {
                for(int i =0;i<jd_skill["cost"].Count;i++)
                {
                    Constant.SkillCostType tempType = (Constant.SkillCostType)Global.INT32(jd_skill["cost"][i]["type"]);
                    int tempValue = Global.INT32(jd_skill["cost"][i]["value"]);
                    this.DoSkillCostByType(ref caster,tempType,tempValue);
                }
            }
        }

        private void DoSkillCostByType(ref Character caster,Constant.SkillCostType costType,int value)
        {
            switch(costType)
            {
                case Constant.SkillCostType.SP:
                    caster.ModelObj.ChangeSP(-value);
                break;
                case Constant.SkillCostType.KP:
                    // caster.ModelObj. 
                break;
            }
        }

        private List<Character> GetCharactersBySelectionType(Character caster,Constant.SkillSelectionType_Alignment alignment)
        {
            List<Character> result = new List<Character>();
            switch(alignment)
            {
                case Constant.SkillSelectionType_Alignment.All:
                    result.AddRange(team1);
                    result.AddRange(team2);
                break;
                case Constant.SkillSelectionType_Alignment.Friend:
                    result = this.GetFriendTeam(caster);
                break;
                case Constant.SkillSelectionType_Alignment.Enemy:
                    result = this.GetEnemyTeam(caster);
                break;
                case Constant.SkillSelectionType_Alignment.OnlyFriend:
                {
                    result = this.GetFriendTeam(caster,false);
                }
                break;
                case Constant.SkillSelectionType_Alignment.Random:
                {
                    result.AddRange(team1);
                    result.AddRange(team2);
                    Character tmpSelected = GetRandomCharacterFromList(result);
                    result.RemoveAll((c)=>{return c!= tmpSelected;});
                }
                break;
                case Constant.SkillSelectionType_Alignment.Self:
                {
                    result.Add(caster);
                }
                break;
            }
            return result;
        }

        ///
        ///注意这个函数的重点目的是为了“计算”和“得到结果”，不必太纠结于结构什么的
        ///
        internal void SelectTargetAndTakeEffect(Character caster,int skillId)
        {
            JsonData jd_skill = SkillManager.CreateInstance().GetSkillJsonDataByID(skillId);
            if(jd_skill != null)
            {
                JsonData jd_selection = jd_skill["selection"];
                JsonData jd_effects = jd_skill["effects"];
                List<Character> targets = new List<Character>();
                //这边做筛选，确定合适的目标
                if(jd_selection != null && jd_selection["alignment"] != null && jd_selection["type"]!=null)
                {
                    Constant.SkillSelectionType_Alignment alignment = (Constant.SkillSelectionType_Alignment) Global.INT32(jd_selection["alignment"]);
                    List<Character> availableTargets = GetCharactersBySelectionType(caster,alignment);
                    int hitCount = Global.INT32(jd_selection["hitCount"]);
                    Constant.SkillSelectionType_Target selectType = (Constant.SkillSelectionType_Target)Global.INT32(jd_selection["type"]);
                    switch(selectType)
                    {
                        case Constant.SkillSelectionType_Target.Multi:
                        {
                            for(int i = 0 ;i<hitCount;i++)
                            {
                                targets.Add(GetRandomCharacterFromList(availableTargets));
                            }
                            break;
                        }
                        default:
                        {
                            for(int j = 0; j < availableTargets.Count; j++)
                            {
                                for(int i = 0; i < hitCount;i++)
                                {
                                    targets.Add(availableTargets[j]);
                                }
                            }
                            break;
                        }
                    }
                    
                }

                //上面已经计算出哪些目标会受到影响，下面根据这个目标结果施加实际效果
                if(jd_effects != null)
                {
                     List<ISkillEffect> skillEffects = SkillManager.CreateInstance().GetSkillEffects(jd_effects);
                     if(skillEffects != null)
                     {
                         for(int i=0;i<skillEffects.Count;i++)
                         {
                             ISkillEffect tempEffect = skillEffects[i];
                             foreach(Character tar in targets)
                             {
                                 tempEffect.InvokeEffect(caster,tar);
                             }
                         }
                     }
                    //convert in to list 
                    //and sort by order
                    //and get the concrete type
                    //and effect to target

                   

                    Debug.Log("Todo:finish this");
                }
            }
            
            
        }


        internal void OutputAllCharacterStatus()
        {   
            this.currentCombat.OutputCombatStatus();
        }

        internal Constant.CombatResult GetCurrentCombatResult()
        {
            if(currentCombat == null || team1 == null || team2 == null)
                return Constant.CombatResult.None;
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
            else if(currentCombat.CurrentTurn >= currentCombat.MaxTurn)
            {
                result = Constant.CombatResult.Draw;
            }
            return result;
        }
        

    }

	
}
