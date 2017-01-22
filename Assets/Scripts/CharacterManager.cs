using UnityEngine;
using System.Collections;
using LitJson;

namespace SJMGame
{
	public class CharacterManager
	{
		JsonData jd_characterTemplate;
		private CharacterManager()
		{

		}


		#region ------------ singleton --------------
		static CharacterManager instance;
		internal static CharacterManager CreateInstance()
		{
			if(instance == null)
			{
				instance = new CharacterManager();
			}
			return instance;
		}
		#endregion

		internal void InitCharacterTemplateFromDataSource(string content)
		{
			jd_characterTemplate = JsonMapper.ToObject(content);

		}

		internal Models.CharacterMO GetCharacterMOFromTemplate(Constant.Faction faction)
		{
		 	Models.CharacterMO result = null;
			// JsonData jd = GameDataLoader.

			for(int i = 0;i<jd_characterTemplate["characterTemplate"].Count;i++)
			{
				JsonData jd = jd_characterTemplate["characterTemplate"][i];
				if(jd["faction"] !=null && (Constant.Faction)Global.INT32(jd["faction"]) == faction)
				{
					result = new Models.CharacterMO();
					result.SetDataFromJson(jd);
					break;
				}
			}
			return result;
		}



	}
}
