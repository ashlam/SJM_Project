using UnityEngine;
using System.Collections;
using SJMGame;

namespace SJMGame.UnitTest
{

	public class UnitTestObject : MonoBehaviour {

		IUnitTest myTest = null;
		// Use this for initialization
		void Start () {
			myTest = new UnitTest_UseSkill();
			if(myTest != null)
			{
				myTest.Init();
			}
		}
		
		// Update is called once per frame
		void Update () {
		}

		/// <summary>
		/// OnGUI is called for rendering and handling GUI events.
		/// This function can be called multiple times per frame (one call per event).
		/// </summary>
		void OnGUI()
		{
			if(GUI.Button(new Rect(10,10,130,44),"DoTest"))
			{
				if(null != myTest)
				{
					myTest.DoTest();
				}
			}
		}
	}
}