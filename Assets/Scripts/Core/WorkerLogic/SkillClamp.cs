using UnityEngine;
namespace Core.WorkerLogic

{
    public class SkillClamp
    {
        private int _minSkill;
        private int _maxSkill;
        
        public CalculatedSkills ClampSkills(CalculatedSkills skills)
        {
            CalculatedSkills clampedSkills = new CalculatedSkills();
            
            clampedSkills.SkillPatience = Mathf.Clamp(skills.SkillPatience, _minSkill, _maxSkill);
            clampedSkills.SkillSocial = Mathf.Clamp(skills.SkillSocial, _minSkill, _maxSkill);
            clampedSkills.SkillIntellectual = Mathf.Clamp(skills.SkillIntellectual, _minSkill, _maxSkill);
            clampedSkills.SkillPhysical = Mathf.Clamp(skills.SkillPhysical, _minSkill, _maxSkill);
            
            return clampedSkills;
        }
    }
}