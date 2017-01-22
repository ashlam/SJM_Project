using UnityEngine;
using System.Collections;
using LitJson;

namespace SJMGame
{
    public class SkillMO 
    {
        public int ID;

        private JsonData skillData;

        internal virtual void SetDataFromJson(JsonData jd)
        {
            this.ID = Global.INT32(jd["id"]);
            this.skillData = jd;
        }

        internal JsonData GetSkillData()
        {
            return skillData;
        }
    }
}