
namespace SJMGame
{
	public class Constant
	{
		public const int SKILLID_CONSIDER = 99;

		public class PromisedWord
		{
			public const string Property_STR = "STR";
			public const string Property_CON = "CON";
			public const string Property_INT = "INT";
			public const string Property_SPD = "SPD";
			public const string KillingPoint = "KP";
			public const string HitPoint = "HP";
			public const string MaxHitPoint = "MHP";
			public const string ForcePoint = "SP";
			public const string MaxForcePoint = "MSP";
		}

	    /// <summary>
	    /// 行动条件key
	    /// 
	    /// 一些后缀缩写说明：
	    /// eq=equal；lt=less than；gt=greater than；le=less or equal；ge=greater or equal
	    /// </summary>
	    public enum ActionCondition
	    { 
	        None = 0,
	        Self_HP_Eq = 101,
	        Self_HP_Le = 102,
	        Self_HP_Ge = 103,
	        Self_HP_Percent_Eq = 104,
	        Self_HP_Percent_Le = 105,
	        Self_HP_Percent_Ge = 106,

			Always = 9999,
	    }

		public enum DamageType
		{
			None = 0,
			Physical = 1,
			Force = 2,
		}

	    public enum SpecialStatus
	    {
	        None = 0,
	        Poison = 1,
	        Burning = 2,
			Spelling = 4,
			Charging = 8,
	    }

		public enum EventCode_S2C
		{
			None = 0,

			//伤害类
			InflictDamage_Life = 101,
			InflictDamage_Force = 102,
		
			Cure_Life = 201,
			Cure_Force = 202,

			//poisoned
			Poisoned = 301,
			PoisonCured = 302,
			InflictPoisonDamage = 303,

			//属性变化
			PropertyChanged_Per = 401,
			PropertyChanged_Num = 402,

			//Positions
			KnockedBack = 501,
			PulledToFront = 502,

			Charge = 503,
			Fallback = 504,


			//Buffs
			GainBuff = 601,
			RemoveBuff = 602,

			//Alive or corposed
			Die_ToCorpse = 701,
			Revived = 702,
			RemoveCorpse = 703,
			Die_Sacrificed = 704,

			//CombatAction
			UseSkill_Begin = 1001,
			ChargingPower_Begin = 1002,
			FocusForce_Begin = 1003,
			ChargingPower_Finished = 1004,
			FocusForce_Finished = 1005,
			UseSkill_Success = 1006,
			UseSkill_Failed = 1007,
			UseSkill_Cost = 1008,
			Dodged = 1009,
			Attack = 1011,
			CountAttack = 1012,

			//Resist
			Resist_Poison = 901,
			Resist_PositionChanged = 902,

			//System:
			///回合开始的事件
			///（使某人行动，或者改变战场环境什么的）
			System_Combat_TurnBegin = 10001,
			///动作开始时的事件（如：聚气、或直接行动）
			System_Combat_ActionBegin = 10002,
			///动作结束时的事件，如聚气完毕或行动完毕
			System_Combat_ActionEnd = 10003,
			///回合结束的事件（回合+1、判定是否结束战斗啥的）
			System_Combat_TurnEnd = 10004,
			///一场战斗开始
			System_Combat_Begin = 10005,
			///一场战斗结束，得到战斗结果
			System_Combat_End = 10006,
			///行动（Action）开始前的结算阶段
			///主要用于结算某些buff效果（如：毒、自动回复、盾等
			System_Combat_CalculateBeforeDoAction =10007,
			///选择行动/付诸行动
			System_Combat_SelectAndDoAction = 10008,
			///等待行动完全结束
			System_Combat_WaitActionFinished = 10009,
			///动作结束后的结算阶段(如：被反击后的存活判定)
			System_Combat_CalculateAfterDoAction = 10010,
			
		}

		public enum SkillSelectionType_Target
		{
			Self = 0,
			Single = 1,
			Multi = 2,
			All = 3,
		}

		public enum SkillSelectionType_Priority
		{
			None = 0,
			Spell = 1,
			Charging = 2,
			Frontline = 3,
			Behindline = 4,
			Poisoned = 5,
		}


		public enum SkillSelectionType_Alignment
		{
			Enemy = 0,
			Self = 1,
			OnlyFriend = 2,
			Friend = 3,
			All = 4,
			Random = 5,
		}

		public enum SkillSelectionType_Limit
		{
			None = 0,
			Alive = 1,
			Corpse = 2,
			
		}


		public enum SkillCastStance
		{
			Immediately = 0,
			Spell,
			Charging,
		}

		public enum SkillCostType
		{
			None = 0,
			SP = 1,
			HP = 2,
			KP = 3,
			MagicCircle = 4,
			Corpse_Friend = 5,
			Corpse_Enemy = 6,
			HP_ByPercent = 7,
			SP_ByPercent = 8,
		}

		public enum SkillEffectType
		{
			None = 0,
			ConditionEffect = 777,
			HPDamage_Normal = 1001,
			HPDamage_IgnoreDefence = 1002,
			HPDamage_ByValue = 1002,
			HPDamage_ByMax = 1003,
			HPDamage_BySelfHP = 1004,

			SPDamage_Normal = 2001,
			SPDamage_IngoreDefence = 2002,

			GainHP = 3001,
			GainHP_ByValue = 3002,
			GainMaxHP_ByPercent = 3101,

			GainSP = 3001,
			GainSP_ByMax = 4002,

			Delay = 5001,
			ReduceRestAP = 5002,

			GainBuff = 6001,
			RemoveBuff = 6002,
			GainMagicCircle = 6101,
			RemoveMagicCircle = 6102,
			StealMagicCircle = 6103,
			
			Revive = 6201,
			Sacrifice = 6202,
			CreateCorpse = 6203,
			RemoveCorpse = 6204,
		}

		public enum EnforcedTargetForEffect
		{
			SameAsSkill = 0,
			Caster = 1,
		}


		//职业：门派+宗派+进化次数+分类
		//比如601（warrior）+01（berserker）+01(lv.1)+01(berserker)=601010101
		//或603（hunter）+00（normal）+00(lv.0)+01(hunter)=603000001
		//或601（warrior）+01（berserker）+02(lv.2)+02(brandWarrior)=601010202
		public enum Faction
		{
			Warrior = 601000001,
			Guard = 601010101,
			Berserker = 601020101,
			MagicWarrior = 603030101,
			RoyalGuardian = 601010201,
			DeathKnight = 601010202,
		}


		public enum Party
		{
			None = 0,
			Shaolin = 1,
			EMei = 2,
			Wudang = 3,
			Dalunsi = 4,
		}

		public enum SkillCastResult
		{
			Successful,
			Charging,
			Spelling,
			Failed,
		}

		public enum SkillCastFailedReason
		{
			None,
			NotEnoughSP,
			NotAvailableTarget,
		}

		public enum EventSourceType
		{
			System = 0,
			Buff = 1,
			Skill = 2,
			Character = 3,
			PassiveSkill = 4,
		}

		public enum CombatResult
		{
			Error = -1,
			None = 0,
			Team1Win = 1,
			Team2Win = 2,
			PlayerWin = 3,
			PlayerLost = 4,
			Draw = 5,
		}

		public enum CombatType
		{
			None,
			PVE,
			PVP,
		}
	}
}