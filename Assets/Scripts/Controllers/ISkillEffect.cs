using UnityEngine;
using System.Collections;
using LitJson;

namespace SJMGame
{
	abstract class ISkillEffect
	{
		internal int Order {
			get {
				if(data != null && data["order"] != null)
					return Global.INT32(data["order"]);
				return 0;
			}
		}
		protected JsonData data;

		internal void SetData(JsonData jd) { this.data = jd; }

		internal abstract void InvokeEffect(Character caster,Character target);
	}

	class SkillEffect_HPDamage_Normal : ISkillEffect
	{
		internal override void InvokeEffect(Character caster,Character target)
		{
			Constant.EventCode_S2C ec = Constant.EventCode_S2C.InflictDamage_Life;
			int damage = 100;
			target.InflictDamage_HP(damage);
			Debug.Log(string.Format("{0}受到了{1}点伤害",target.ModelObj.Name,damage));
		}

	}
	
}