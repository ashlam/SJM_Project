using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SJMGame.Models;
using LitJson;

namespace SJMGame
{
    public class Character :IEventListenerContainer
    {
        private CharacterMO model;
		private CharacterMO originModel;
		public int ID{get;set;}
		public int ForceID {get;set;}
		public bool InFront {get;set;}

		public Constant.SkillCastStance CurrentStance{get;set;}

		public bool IsAlive { 
			get { return this.model.HP > 0; }
		}
       
		internal CharacterMO ModelObj
		{
			get{ return this.model; }
			set{ this.model = value; }
		}

		internal List<ActionSetting> ActionSettings
		{
			get { return (model != null) ? model.GetActionSettings() : null; }
		}

		internal float ActionPoint {get;set;}

		internal void SetOriginalObj(CharacterMO mo)
		{
			originModel = mo;
		}

        internal void DoActionInTurn()
        {
            List<ActionSetting> actions = model.GetActionSettings();
			int skillId = CombatManager.CreateInstance().GetSkillIDByActionSettings(this);
			// Debug.Log(this.model.Name + " try to use skillId = " + skillId);
			Color a = new Color(0.3f,1,0.6f);
			// LogReporter.Log(this.model.Name + " try to use skillId = " + skillId , this.ForceID);
			this.UseSkill(skillId);
			
        }

		internal void UseSkill(int skillId)
		{
			Constant.SkillCastFailedReason reason = CombatManager.CreateInstance().GetSkillCastFailedReason(this.ID,skillId);
			if(reason == Constant.SkillCastFailedReason.None)
			{
				Constant.SkillCastStance requiredStance = CombatManager.CreateInstance().GetRequiredSkillStance(skillId);
				if(requiredStance == this.CurrentStance)
				{
					this.CastSkillSuccssful(skillId);
				}
				else
				{
					Debug.Log("Change stance to ：" + requiredStance);
					this.CurrentStance = requiredStance;
				}
			}
			else
			{
				this.CastSkillFailed(reason);
			}
		}

		private void CastSkillSuccssful(int skillId)
		{
			this.CurrentStance = Constant.SkillCastStance.Immediately;
			//cost
			CombatManager.CreateInstance().DoSkillCost(this,skillId);
			//select available targets
			//use skill and take effect
			CombatManager.CreateInstance().SelectTargetAndTakeEffect(this,skillId);
			//+ap

			CombatManager.CreateInstance().OutputAllCharacterStatus();
		}

		private void CastSkillFailed(Constant.SkillCastFailedReason reason)
		{
			this.CurrentStance = Constant.SkillCastStance.Immediately;
			Debug.Log("use skill failed, because of："+reason);
		}

		internal void InflictDamage_HP(int damage,bool isCritical=false)
		{
			this.model.ChangeHP(-damage);
		}

		internal void CureLife(int value)
		{
			this.model.HP = this.model.HP + value;
			this.model.HP = Mathf.Min(this.model.HP,this.model.MaxHP);
		}

		internal void OnActionStart(object arg)
		{
			List<Character> allCharacters = arg as List<Character>;
			Debug.Log(string.Format("{0}开始行动",this.ModelObj.Name));
			// EventCenter.CreateInstance().InvokeEvent(Constant.EventCode_S2C.System_Combat_CalculateBeforeDoAction,this,this);
		}

		private void InvokeBuffBeforeAction()
		{
			Debug.Log(string.Format("检测{0}身上的前置buff",this.ModelObj.Name));
			// foreach
			//结算完毕后把控制权交回Combat，决定是否继续
			EventCenter.CreateInstance().InvokeEvent(Constant.EventCode_S2C.System_Combat_CalculateBeforeDoAction,CombatManager.CreateInstance().CurrentCombat,this);
		}
    }

}