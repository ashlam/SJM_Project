using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;

namespace SJMGame
{
	public class SkillManager 
	{
		Dictionary<int,Skill> allOriginSkills;
		private SkillManager()
		{
			allOriginSkills = new Dictionary<int,Skill>();
			this.InitializeAllSkillEffects();
		}

		#region ------------- singleton ------------
		private static SkillManager instance;
		internal static SkillManager CreateInstance()
		{
			if(null == instance)
				instance = new SkillManager();
			return instance;
		}
		#endregion


		JsonData jd_allSkillDatas = null;
		internal void InitJsonDataFromDataSource(string jsonContent)
		{
			jd_allSkillDatas = JsonMapper.ToObject(jsonContent);
		}

		internal JsonData GetSkillJsonDataByID(int skillId)
		{
			JsonData result = null;
			if(jd_allSkillDatas != null && jd_allSkillDatas["allSkillDatas"] != null)
			{
				for(int i = 0;i< jd_allSkillDatas["allSkillDatas"].Count; i++)
				{
					JsonData tempData = jd_allSkillDatas["allSkillDatas"][i]; 
					if(Global.INT32(tempData["id"]) == skillId)
					{
						result = tempData;
						break;
					}
				}
			}
			return result;
		}

		
		internal void InitializeAllSkills()
		{
			allOriginSkills.Clear();
			for(int i = 0; i < jd_allSkillDatas.Count; i++)
			{
				int tempId = Global.INT32(jd_allSkillDatas["allSkillDatas"][i]);
				Skill tempSkill = new Skill();
				tempSkill.SkillID = tempId;
				// tempSkill.SetData()
				JsonData jd = GetSkillJsonDataByID(tempId);
				// tempSkill.SetData(jd);
			} 
		}

		internal Skill GetSkillProtoByID(int skillId)
		{
			if(allOriginSkills.ContainsKey(skillId))
				return allOriginSkills[skillId];
			return null;
		}

		private Constant.SkillEffectType GetSkillEffectTypeByJson(JsonData jd)
		{
			Constant.SkillEffectType result = Constant.SkillEffectType.None;
			if(jd["type"] != null)
			{
				result = (Constant.SkillEffectType) System.Enum.Parse(typeof(Constant.SkillEffectType),Global.GetString(jd["type"]));
			}
			return result;
		}

		private Dictionary<Constant.SkillEffectType,System.Type> allSkillEffectFactory = new Dictionary<Constant.SkillEffectType, System.Type>();

		void InitializeAllSkillEffects()
		{
			allSkillEffectFactory.Clear();
			allSkillEffectFactory.Add(Constant.SkillEffectType.HPDamage_Normal,typeof(SkillEffect_HPDamage_Normal));
		}

		ISkillEffect CreateSkillEffectInstance<T>(Constant.SkillEffectType effectType) where T:ISkillEffect
		{
			T result = null;
			if(allSkillEffectFactory.ContainsKey(effectType))
			{
				System.Type t = allSkillEffectFactory[effectType];
				if( t.IsSubclassOf(typeof(ISkillEffect)))
				{
					result = System.Activator.CreateInstance(t) as T;
				}
				
			}
			return result;
		}

		internal ISkillEffect GetSkillEffectByJsonData(JsonData jd_effect)
		{
			ISkillEffect result = null;
			Constant.SkillEffectType effectType = (Constant.SkillEffectType)Global.INT32(jd_effect["type"]);
			//to be design..

			result = CreateSkillEffectInstance<ISkillEffect>(effectType);
			return result;
		}

		internal List<ISkillEffect> GetSkillEffects(JsonData jd_effects)
		{
			List<ISkillEffect> result = null;
			if(jd_effects != null)
			{
				result = new List<ISkillEffect>();
				for(int i=0;i<jd_effects.Count;i++)
				{
					JsonData ed = jd_effects[i];
					ISkillEffect tempEffect = this.GetSkillEffectByJsonData(ed);
					if(tempEffect != null)
						result.Add(tempEffect);
				}
				result.Sort((e1,e2)=>{ 
					return e1.Order.CompareTo(e2.Order);
				});
			}
			return result;
		}
	}
}