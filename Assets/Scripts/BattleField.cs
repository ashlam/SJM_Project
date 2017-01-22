using UnityEngine;
using System.Collections;

namespace SJMGame
{
	public class BattleField : MonoBehaviour {

		[SerializeField]
		private PositionData standardPos_front_t1;
		
		[SerializeField]
		private PositionData standardPos_back_t1;
		
		[SerializeField]
		private PositionData standardPos_front_t2;
		
		[SerializeField]
		private PositionData standardPos_back_t2;
		

		// Use this for initialization
		void Start () {
			CombatManager.CreateInstance().RegisterBattleField(this);
			
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		
	}

	
	[System.SerializableAttribute()]
	public class PositionData
	{
		public int ForceID;
		public int PosID;
		public Transform Pos;
	}
}

