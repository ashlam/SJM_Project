using UnityEngine;
using System.Collections;

namespace SJMGame
{
	public class ConditionValidation
	{

		delegate bool conditionValidationHandle();
		internal static bool CheckActionSettingCondition(Models.ActionSetting setting,Character c)
		{
			bool result = false;
			int param =  Global.INT32(setting.Param);
			conditionValidationHandle tempHandle = null;
			switch(setting.Condition)
			{
				case Constant.ActionCondition.Always:
				{
					result = true;
					break;
				}
				case Constant.ActionCondition.Self_HP_Ge:
				{
					tempHandle = delegate(){return c.ModelObj.HP >= param; };
					break;
				}
				case Constant.ActionCondition.Self_HP_Eq:
				{
					tempHandle = delegate(){ return c.ModelObj.HP == param;};
					break;
				}
				case Constant.ActionCondition.Self_HP_Le:
				{
					tempHandle = delegate(){ return c.ModelObj.HP <= param;};
					break;
				}
				case Constant.ActionCondition.Self_HP_Percent_Eq:
				{
					tempHandle = delegate(){ 
						float per = (float)c.ModelObj.HP * 100 / c.ModelObj.MaxHP; 
						return Mathf.CeilToInt(per) == param;
					};
					break;
				}
				case Constant.ActionCondition.Self_HP_Percent_Ge:
				{
					tempHandle = delegate(){ 
						float per = (float)c.ModelObj.HP * 100 / c.ModelObj.MaxHP; 
						return Mathf.CeilToInt(per) >= param;
					};
					break;
				}
				case Constant.ActionCondition.Self_HP_Percent_Le:
				{
					tempHandle = delegate(){ 
						float per = (float)c.ModelObj.HP * 100 / c.ModelObj.MaxHP; 
						return Mathf.CeilToInt(per) <= param;
					};
					break;
				}				
			}
			if(null != tempHandle)
			{
				result = tempHandle();
			}
			// Debug.Log (string.Format("condition={0},result={1}",setting.Condition,result));
			return result;
		}
	}
}

