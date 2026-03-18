using Content;
using Runtime;

namespace Core.WorkerLogic
{
    public class WorkerSkillsCalculator
    {
        /// <summary>
        /// Применение пассивных модификаторов навыков.
        /// </summary>
        /// <param name="worker">Работник.</param>
        /// <returns>Модифицированные навыки в границах от 0 до 5 (определяется в SkillClamp).</returns>
        public CalculatedSkills CalculateModifiedSkills(WorkerDef worker)
        {
            CalculatedSkills skills = new CalculatedSkills();
            SkillClamp skillClamp = new SkillClamp();
            
            int tempPatience = worker.basePatience;
            int tempSocial = worker.baseSocial;
            int tempIntellectual = worker.baseIntellectual;
            int tempPhysical =  worker.basePhysical;

            if (worker.personalityTraits != null)
            {
                foreach (var trait in worker.personalityTraits)
                {
                    tempPatience = trait?.ModifyPatience(tempPatience) ?? tempPatience;
                    tempSocial = trait?.ModifySocial(tempSocial) ?? tempSocial;
                    tempIntellectual = trait?.ModifyIntellectual(tempIntellectual) ?? tempIntellectual;
                    tempPhysical = trait?.ModifyPhysical(tempPhysical) ?? tempPhysical;
                }
            }
            
            skills.SkillPatience = tempPatience;
            skills.SkillSocial = tempSocial;
            skills.SkillIntellectual = tempIntellectual;
            skills.SkillPhysical = tempPhysical;
            
            return skillClamp.ClampSkills(skills);
        }

        /// <summary>
        /// Применение модификаторов навыков по условию.
        /// </summary>
        /// <param name="worker">Работник.</param>
        /// <param name="calculatedSkills">Навыки с применёнными пассивными модификаторами.</param>
        /// <param name="task">Задание.</param>
        /// <returns></returns>
        public CalculatedSkills CalculateModifiedSkillsConditional(WorkerDef worker, CalculatedSkills calculatedSkills, TaskRuntime task)
        {
            CalculatedSkills skills = new CalculatedSkills();
            SkillClamp skillClamp = new SkillClamp();

            int tempPatience = calculatedSkills.SkillPatience;
            int tempSocial = calculatedSkills.SkillSocial;
            int tempIntellectual = calculatedSkills.SkillIntellectual;
            int tempPhysical = calculatedSkills.SkillPhysical;

            if (worker.personalityTraits != null)
            {
                foreach (var trait in worker.personalityTraits)
                {
                    tempPatience = trait?.ModifyPatienceConditional(tempPatience, task) ?? tempPatience;
                    tempSocial = trait?.ModifySocialConditional(tempSocial, task) ?? tempSocial;
                    tempIntellectual = trait?.ModifyIntellectualConditional(tempIntellectual, task) ?? tempIntellectual;
                    tempPhysical = trait?.ModifyPhysicalConditional(tempPhysical, task) ?? tempPhysical;
                }
            }
            
            skills.SkillPatience = tempPatience;
            skills.SkillSocial = tempSocial;
            skills.SkillIntellectual = tempIntellectual;
            skills.SkillPhysical = tempPhysical;
            
            return skillClamp.ClampSkills(skills);
        }
    }
}