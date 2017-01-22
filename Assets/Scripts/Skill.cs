using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

namespace SJMGame
{
    public class Skill 
    {
        public int SkillID { get; set; }
        private JsonData skillData = null;
        private List<ISkillEffect> myEffects;

        public Skill()
        {
            myEffects = new List<ISkillEffect>();
        }

        internal void TakeEffect(Character caster)
        {
            //throw new System.NotImplementedException();
        }

        // internal void SetData(JsonData data)
        // {
        //     this.skillData = data;
        //     myEffects.Clear();
        //     for(int i=0;i<data["effects"].Count;i++)
        //     {
        //         JsonData effectData = data["effects"][i];
        //         ISkillEffect tempEffect = SkillManager.CreateInstance().GetSkillEffectByJsonData(effectData);
        //         tempEffect.SetData(effectData);
        //         myEffects.Add(tempEffect);
        //     }
        //     myEffects.Sort((e1,e2)=>{
        //             return e1.Order.CompareTo(e2.Order);
        //         }
        //     );
        // }
    }
}