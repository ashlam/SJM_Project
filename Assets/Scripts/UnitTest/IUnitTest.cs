using UnityEngine;
using System.Collections;
using SJMGame;

namespace SJMGame.UnitTest
{
	interface IUnitTest
	{
		void Init();
		void DoTest();
	}

	class UnitTest_UseSkill:IUnitTest
	{
		Character c1,c2;
		
		public void Init()
		{
			c1 = new Character();
			c2 = new Character();
			Models.CharacterMO c1mo = CharacterManager.CreateInstance().GetCharacterMOFromTemplate(Constant.Faction.Warrior);
			Models.CharacterMO c2mo = CharacterManager.CreateInstance().GetCharacterMOFromTemplate(Constant.Faction.Warrior);
			
			
			
			c1.ModelObj = c1mo;
			c2.ModelObj = c2mo;
			c1.ID = 1;c2.ID = 2;
			c1.ModelObj.Name = "甲";
			c2.ModelObj.Name = "乙";
			
			CombatManager.CreateInstance().AddCharacterToTeam(c1,1);
			CombatManager.CreateInstance().AddCharacterToTeam(c2,2);

			CombatManager.CreateInstance().InitEvents();

			CombatManager.CreateInstance().CombatStart();
		}

		public void DoTest()
		{
			for(int i = 0;i<10;i++)
			{
				if(c1.ModelObj.HP <= 0 || c2.ModelObj.HP <=0)
				{
					break;
				}
				else
				{
					c1.DoActionInTurn();
				}
			}
		}
	}
	
}