using UnityEngine;
using System.Collections;

namespace SJMGame
{

	public class MockCombat : MonoBehaviour {
		
		Character testCharacter;

		public TextAsset dataSource;
		public TextAsset dataSource_skill;

		// Use this for initialization
		void Start () {
		

		}
		
		// Update is called once per frame
		void Update () {
			
		}

		void OnGUI()
		{
			if(GUI.Button(new Rect(4,4,170,39),"CharacterData Test"))
			{
				if(dataSource != null)
				{
					LitJson.JsonData jd = LitJson.JsonMapper.ToObject(dataSource.text);
					if(jd != null)
					{
						testCharacter = new Character();
						Models.CharacterMO cmo = new SJMGame.Models.CharacterMO();
						cmo.SetDataFromJson(jd);
						testCharacter.ModelObj = cmo;
						
						Debug.Log (" aye");

	
						testCharacter.DoActionInTurn();
						
						testCharacter.InflictDamage_HP(200);
						testCharacter.DoActionInTurn();

						testCharacter.InflictDamage_HP(500);
						testCharacter.DoActionInTurn();
					}
				}
			}
		}

	}
}
