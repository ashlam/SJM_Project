using UnityEngine;
using System.Collections;

namespace SJMGame.Models
{
    public class ActionSetting 
    {
			public int ID {get;set;}
			public SJMGame.Constant.ActionCondition Condition {get;set;}
			public object Param{get;set;}
			public int SkillID {get;set;}
    }
}