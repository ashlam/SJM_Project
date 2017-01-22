using UnityEngine;
using System.Collections;
using System.Text;

namespace SJMGame
{
	public class LogReporter : MonoBehaviour {

		[SerializeField]
		private UILabel lbLeft;

		[SerializeField]
		private UILabel lbRight;

		// Use this for initialization
		void Start () {
			s_lbLeft = this.lbLeft;
			s_lbRight = this.lbRight;
		}
		
		// // Update is called once per frame
		// void Update () {
		
		// }

		static UILabel s_lbLeft , s_lbRight;
		internal static void Log(string message,int forceId)
		{
			UILabel lbToWrite = null;
			UILabel lbAnother = null;
			switch(forceId)
			{
				case 1:
					lbToWrite = s_lbLeft;
					lbAnother = s_lbRight;
					break;
				case 2:
					lbToWrite = s_lbRight;
					lbAnother = s_lbLeft;
					break;
			}
			
			if(lbToWrite !=null && lbAnother != null)
			{
				StringBuilder sb = new StringBuilder(lbToWrite.text);
				sb.AppendLine(message);
				lbToWrite.text = sb.ToString();

				sb = new StringBuilder(lbAnother.text);
				sb.AppendLine();
				lbAnother.text = sb.ToString();
			}
		}
	}
}
