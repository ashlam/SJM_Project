using UnityEngine;
using System.Collections;

namespace SJMGame
{
	public class GameDataLoader : MonoBehaviour {

		[SerializeField]
		private TextAsset dataSource_skill;
		[SerializeField]
		private TextAsset dataSource_characterTemplate;

		/// <summary>
		/// Awake is called when the script instance is being loaded.
		/// </summary>
		void Awake()
		{
			if(dataSource_skill != null)
			{
				SkillManager.CreateInstance().InitJsonDataFromDataSource(dataSource_skill.text);
			}
			if(dataSource_characterTemplate != null)
			{
				CharacterManager.CreateInstance().InitCharacterTemplateFromDataSource(dataSource_characterTemplate.text);
			}
		}

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
		
		}
	}
}

