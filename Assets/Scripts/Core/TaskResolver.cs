using System.Collections.Generic;
using UnityEngine;
using Content;

namespace Core
{
    [CreateAssetMenu(fileName = "EventResolver", menuName = "Scriptable Objects/Core/EventResolver")]
    public class TaskResolver : ScriptableObject
    {
        private readonly int _criticalFailure = 0;
        private readonly int _criticalSuccess = 5;
        
        public bool IsSuccessful(TaskDef task, params Worker[] workers)
        {
            bool totalResult = false;
            
            foreach (var worker in workers)
            {
                bool workerResult = SkillCheck(DiceRoll(worker.SkillPatience), task.patienceRequired) &&
                                    SkillCheck(DiceRoll(worker.SkillSocial), task.socialRequired) &&
                                    SkillCheck(DiceRoll(worker.SkillIntellectual), task.intellectualRequired) &&
                                    SkillCheck(DiceRoll(worker.SkillPhysical), task.physicalRequired);
                totalResult = totalResult || workerResult;
            }
            return totalResult;
        }
        
        public bool IsCriticalFailure(List<int> rolls, int skillRequired)
        {
            return rolls.Contains(_criticalFailure) && skillRequired != 0;
        }

        public bool IsCriticalSuccess(List<int> rolls, int skillRequired)
        {
            return rolls.Contains(_criticalSuccess) && skillRequired != 0;
        }

        private bool SkillCheck(List<int> rolls, int skillRequired)
        {
            foreach (var roll in rolls)
            {
                if (roll >= skillRequired)
                    return true;
            }
            return false;
        }

        private List<int> DiceRoll(int skillLevel)
        {
            List<int> rolls = new List<int>();
            for (int i = 0; i < skillLevel; i++)
            {
                 rolls.Add(Random.Range(0, skillLevel + 1));
            }
            
            return rolls;
        }
    }
}
