using UnityEngine;
namespace Core.WorkerLogic

{
    public class SkillClamp
    {
        private const int MinSkill = 0;
        private const int MaxSkill = 5;
        
        public CalculatedSkills ClampSkills(CalculatedSkills skills)
        {
            CalculatedSkills clampedSkills = new CalculatedSkills();
            
            clampedSkills.SkillPatience = Mathf.Clamp(skills.SkillPatience, MinSkill, MaxSkill);
            clampedSkills.SkillSocial = Mathf.Clamp(skills.SkillSocial, MinSkill, MaxSkill);
            clampedSkills.SkillIntellectual = Mathf.Clamp(skills.SkillIntellectual, MinSkill, MaxSkill);
            clampedSkills.SkillPhysical = Mathf.Clamp(skills.SkillPhysical, MinSkill, MaxSkill);
            
            return clampedSkills;
        }
    }
}