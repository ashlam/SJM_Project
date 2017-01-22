using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace SJMGame.Models
{
    public class CharacterMO
    {
        internal int ID { get; set; }
        internal string Name { get; set; }
		internal int Property_STR {get;set;}
		internal int Property_INT {get;set;}
		internal int Property_CON {get;set;}
		internal int Property_SPD {get;set;}
		internal int HP { get;set; }
		internal int SP { get;set; }
		internal int MaxHP { get;set; }
		internal int MaxSP { get;set; }

        List<ActionSetting> actionSettings = new List<ActionSetting>();

        internal List<ActionSetting> GetActionSettings()
        {
            //throw new System.NotImplementedException();
            return actionSettings;
        }

		internal void SetDataFromJson(LitJson.JsonData jd)
		{
			this.ID = Global.INT32(jd["id"]); 
			this.Name = Global.GetString(jd["name"]);
			this.Property_STR = Global.INT32(jd["str"]);
			this.Property_CON = Global.INT32(jd["con"]);
			this.Property_INT = Global.INT32(jd["int"]);
			this.Property_SPD = Global.INT32(jd["spd"]);
			this.HP = Global.INT32(jd["hp"]);
			this.MaxHP = Global.INT32(jd["mhp"]);
			this.SP = Global.INT32(jd["sp"]);
			this.MaxSP = Global.INT32(jd["msp"]);
			for(int i =0;i<jd["actionSetting"].Count;i++)
			{
				LitJson.JsonData ad = jd["actionSetting"][i];
                if(Global.INT32(ad["id"]) > 0)
				{
					ActionSetting tempSetting = new ActionSetting();
					tempSetting.ID = Global.INT32(ad["id"]);
					tempSetting.Condition = (Constant.ActionCondition)Global.INT32(ad["condition"]);
					tempSetting.Param = Global.INT32(ad["param"]);
					tempSetting.SkillID = Global.INT32(ad["action"]);
					this.actionSettings.Add(tempSetting);
				}
			}
			actionSettings.Sort((a,b)=>{
				return a.ID.CompareTo(b.ID);
			});
		}

		internal void Init()
		{
			this.HP = this.MaxHP;
		}

		internal void ChangeHP(int value)
		{
			if(value > 0)
			{
				this.HP = Mathf.Min(this.MaxHP, this.HP + value);
			}
			else if(value < 0)
			{
				this.HP = Mathf.Max(this.HP - Mathf.Abs(value),0);
			}

			Debug.Log("CurrentHP = "+this.HP);
		}

		internal void ChangeSP(int value)
		{
			if(value > 0)
			{
				this.SP = Mathf.Min(this.MaxSP, this.SP + value);
			}
			else if(value < 0)
			{
				this.SP = Mathf.Min(this.SP - Mathf.Abs(value),0);
			}
		}
    }
}