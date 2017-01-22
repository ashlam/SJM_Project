using UnityEngine;
using System.Collections;

namespace SJMGame
{
	public class Global 
	{
		public static int INT32(object jd)
		{
			int result = 0;
			try
			{
				result = jd == null ? 0 : System.Convert.ToInt32(jd.ToString());
			}
			catch(System.Exception e)
			{
				Debug.Log(e.Message);
			}
			return result;
		}

		public static string GetString(object jd)
		{
			return jd.ToString();
		}

		public static bool Boolean(object jd)
		{
			return System.Convert.ToBoolean(jd.ToString());
		}
	}
}
