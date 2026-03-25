using System;
using System.Collections.Generic;
using System.Linq;
using Content;
using Core.WorkerLogic;
using Runtime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.TaskLogic
{
    [CreateAssetMenu(fileName = "TaskResultCalculator", menuName = "Scriptable Objects/Core/TaskLogic/TaskResultCalculator")]
    public class TaskResultCalculator : ScriptableObject
    {
        private readonly int _criticalFailure = 0; // Минимальное возможное значение навыка.
        private readonly int _criticalSuccess = 5; // Максимальное возможное значение навыка.
        private readonly int _skillsAmount = 4; // Количество навыков.
        
        private readonly WorkerSkillsCalculator _workerSkillsCalculator =  new WorkerSkillsCalculator();
        
        /// <summary>
        /// Результат выполнения задания для всех работников.
        /// </summary>
        /// <param name="task">Задание.</param>
        /// <param name="workers">Список работников.</param>
        /// <returns>Результат выполнения задания: успех/провал, критический успех, критический провал.</returns>
        public TotalTaskResult TotalTaskResult(TaskRuntime task, params WorkerDef[] workers)
        {
            TotalTaskResult result =  new TotalTaskResult();
            
            List<WorkerTaskResult> workerTaskResults = new List<WorkerTaskResult>();
            int totalCriticalSuccessAmount = 0;
            int totalCriticalFailureAmount = 0;

            // Применяем пассивные и условные модификаторы к навыкам.
            foreach (var worker in workers)
                workerTaskResults.Add(TotalWorkerTaskResult(task, _workerSkillsCalculator.CalculateModifiedSkillsConditional(worker, _workerSkillsCalculator.CalculateModifiedSkills(worker), task)));

            foreach (var workerTaskResult in workerTaskResults)
                totalCriticalSuccessAmount += workerTaskResult.CriticalSuccessAmount;

            foreach (var workerTaskResult in workerTaskResults)
                totalCriticalFailureAmount += workerTaskResult.CriticalFailureAmount;
            
            if (totalCriticalSuccessAmount > totalCriticalFailureAmount)
                result.IsCriticalSuccess = true;
            if  (totalCriticalFailureAmount > totalCriticalSuccessAmount)
                result.IsCriticalFailure = true;
            
            result.IsSuccess = IsTaskSuccessful(workerTaskResults);
            
            return result;
        }
        
        /// <summary>
        /// Определяет успех прохождения испытания для всех работников.
        /// </summary>
        /// <param name="workerTaskResults">Результаты выполнения задания работников.</param>
        /// <returns>Успех прохождения испытания.</returns>
        private bool IsTaskSuccessful(List<WorkerTaskResult> workerTaskResults)
        {
            List<bool> totalSkillCheckSum = new List<bool> { false, false, false, false };
            
            foreach (var result in workerTaskResults)
            {
                for (int i = 0; i < _skillsAmount; i++)
                    totalSkillCheckSum[i] = totalSkillCheckSum[i] || result.Success[i];
            }
            
            return !totalSkillCheckSum.Contains(false);
        }

        /// <summary>
        /// Результат выполнения задания для одного работника.
        /// </summary>
        /// <param name="task">Задание.</param>
        /// <param name="skills">Навыки.</param>
        /// <returns>Результат выполнения задания для одного работника:
        /// Список успехов и неудач, количество критических успехов и провалов.</returns>
        private WorkerTaskResult TotalWorkerTaskResult(TaskDef task, CalculatedSkills skills)
        {
            WorkerTaskResult workerTaskResult = new WorkerTaskResult();

            // Результаты бросков кубика по всем навыкам.
            var patienceRolls = DiceRoll(skills.SkillPatience);
            var socialRolls = DiceRoll(skills.SkillSocial);
            var intellectualRolls = DiceRoll(skills.SkillIntellectual);
            var physicalRolls = DiceRoll(skills.SkillPhysical);
            
            workerTaskResult.Success = WorkerTaskSuccess(patienceRolls, socialRolls, intellectualRolls, physicalRolls, task);
            workerTaskResult.CriticalSuccessAmount = CriticalSuccessAmount(patienceRolls, socialRolls, intellectualRolls, physicalRolls, task);
            workerTaskResult.CriticalFailureAmount = CriticalFailureAmount(patienceRolls, socialRolls, intellectualRolls, physicalRolls, task);
            
            return workerTaskResult;
        }

        /// <summary>
        /// Фиксирует успехи и неудачи задания по всем навыкам для одного работника.
        /// </summary>
        /// <param name="patienceRolls">Результаты бросков на навык терпения.</param>
        /// <param name="socialRolls">Результаты бросков на социальный навык.</param>
        /// <param name="intellectualRolls">Результаты бросков на интеллектуальный навык.</param>
        /// <param name="physicalRolls">Результаты бросков на физический навык</param>
        /// <param name="task">Задание.</param>
        /// <returns>Список успехов и неудач по всем навыкам для одного работника.</returns>
        private List<bool> WorkerTaskSuccess(List<int> patienceRolls, List<int> socialRolls,
            List<int> intellectualRolls, List<int> physicalRolls, TaskDef task)
        {
            // Фиксируем результаты проверки успеха.
            List<bool> result = new List<bool>
            {
                IsSkillSuccess(patienceRolls, task.patienceRequired),
                IsSkillSuccess(socialRolls, task.socialRequired),
                IsSkillSuccess(intellectualRolls, task.intellectualRequired),
                IsSkillSuccess(physicalRolls, task.physicalRequired)
            };

            return result;
        }
        
        /// <summary>
        /// Проверка успеха для одного навыка.
        /// Если хоть один бросок из списка больше порога или равен ему -- проверка пройдена.
        /// </summary>
        /// <param name="rolls">Список результатов бросков кубика.</param>
        /// <param name="skillRequired">Порог проверки.</param>
        /// <returns>Пройдена ли проверка одного навыка.</returns>
        private bool IsSkillSuccess(List<int> rolls, int skillRequired)
        {
            if (skillRequired == 0)
                return true;
            foreach (var roll in rolls)
            {
                if (roll >= skillRequired)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Бросок кубика.
        /// </summary>
        /// <param name="skillLevel">Уровень навыка сотрудника.</param>
        /// <returns>Результаты всех бросков (выпавшие числа).</returns>
        private List<int> DiceRoll(int skillLevel)
        {
            List<int> rolls = new List<int>();
            
            for (int i = 0; i < skillLevel; i++)
                rolls.Add(Random.Range(0, skillLevel + 1));
            
            return rolls;
        }
        
        /// <summary>
        /// Подсчёт критических успехов на задании для одного работника.
        /// </summary>
        /// <param name="patienceRolls">Результаты бросков на навык терпения.</param>
        /// <param name="socialRolls">Результаты бросков на социальный навык.</param>
        /// <param name="intellectualRolls">Результаты бросков на интеллектуальный навык</param>
        /// <param name="physicalRolls">Результаты бросков на физический навык</param>
        /// <param name="task">Задание.</param>
        /// <returns>Количество критических успехов.</returns>
        private int CriticalSuccessAmount(List<int> patienceRolls, List<int> socialRolls,
            List<int> intellectualRolls, List<int> physicalRolls, TaskDef task)
        {
            int criticalSuccessAmount = 0;
            
            // Фиксируем количество критических успехов.
            criticalSuccessAmount += CriticalSuccessInRollCount(patienceRolls, task.patienceRequired); 
            criticalSuccessAmount += CriticalSuccessInRollCount(socialRolls, task.socialRequired);
            criticalSuccessAmount += CriticalSuccessInRollCount(intellectualRolls, task.intellectualRequired);
            criticalSuccessAmount += CriticalSuccessInRollCount(physicalRolls, task.physicalRequired);
            
            return criticalSuccessAmount;
        }
        
        /// <summary>
        /// Подсчёт критических провалов на задании для одного работника.
        /// </summary>
        /// <param name="patienceRolls">Результаты бросков на навык терпения.</param>
        /// <param name="socialRolls">Результаты бросков на социальный навык.</param>
        /// <param name="intellectualRolls">Результаты бросков на интеллектуальный навык</param>
        /// <param name="physicalRolls">Результаты бросков на физический навык</param>
        /// <param name="task">Задание.</param>
        /// <returns>Количество критических провалов.</returns>
        private int CriticalFailureAmount(List<int> patienceRolls, List<int> socialRolls,
            List<int> intellectualRolls, List<int> physicalRolls, TaskDef task)
        {
            int criticalFailureAmount = 0;
            
            // Фиксируем количество критических провалов.
            criticalFailureAmount += CriticalFailureInRollCount(patienceRolls, task.patienceRequired); 
            criticalFailureAmount += CriticalFailureInRollCount(socialRolls, task.socialRequired);
            criticalFailureAmount += CriticalFailureInRollCount(intellectualRolls, task.intellectualRequired);
            criticalFailureAmount += CriticalFailureInRollCount(physicalRolls, task.physicalRequired);
            
            return criticalFailureAmount;
        }
        
        /// <summary>
        /// Считает количество критических успехов на одном броске.
        /// </summary>
        /// <param name="rolls">Список результатов бросков кубика.</param>
        /// <param name="skillRequired">Порог проверки.</param>
        /// <returns></returns>
        private int CriticalSuccessInRollCount(List<int> rolls, int skillRequired)
        {
            int result = 0;
            
            if (skillRequired != 0)
                result = rolls.Count(x => x.Equals(_criticalSuccess));
            
            return result;
        }

        /// <summary>
        /// Считает количество критических провалов на одном броске.
        /// </summary>
        /// <param name="rolls">Список результатов бросков кубика.</param>
        /// <param name="skillRequired">Порог проверки.</param>
        /// <returns></returns>
        private int CriticalFailureInRollCount(List<int> rolls, int skillRequired)
        {
            int result = 0;
            
            if (skillRequired != 0)
                result = rolls.Count(x => x.Equals(_criticalFailure));
            
            return result;
        }
        
        /// <summary>
        /// Фиксирует критический провал.
        /// Если порог проверки == 0, провал не учитывается.
        /// </summary>
        /// <param name="rolls">Список результатов бросков кубика.</param>
        /// <param name="skillRequired">Порог проверки.</param>
        /// <returns>Есть ли критический провал.</returns>
        [Obsolete("IsCriticalFailure больше не используется -- заменён на CriticalFailureInRollCount.")]
        private bool IsCriticalFailure(List<int> rolls, int skillRequired)
        {
            return rolls.Contains(_criticalFailure) && skillRequired != 0;
        }

        /// <summary>
        /// Фиксирует критический успех.
        /// Если порог проверки == 0, успех не учитывается.
        /// </summary>
        /// <param name="rolls">Список результатов бросков кубика.</param>
        /// <param name="skillRequired">Порог проверки.</param>
        /// <returns>Есть ли критический успех.</returns>
        [Obsolete("IsCriticalSuccess больше не используется -- заменён на CriticalSuccessInRollCount.")]
        private bool IsCriticalSuccess(List<int> rolls, int skillRequired)
        {
            return rolls.Contains(_criticalSuccess) && skillRequired != 0;
        }
    }
}
