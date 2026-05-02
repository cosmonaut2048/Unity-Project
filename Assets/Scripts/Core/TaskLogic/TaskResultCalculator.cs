using System.Collections.Generic;
using System.Linq;
using Content;
using Core.WorkerLogic;
using Runtime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.TaskLogic
{
    public class TaskResultCalculator
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
        public TotalTaskResult TotalTaskResult(TaskRuntime task, List<WorkerRuntime> workers)
        {
            TotalTaskResult result =  ScriptableObject.CreateInstance<TotalTaskResult>();
            
            List<WorkerTaskResult> workerTaskResults = new List<WorkerTaskResult>();
            int totalCriticalSuccessAmount = 0;
            int totalCriticalFailureAmount = 0;

            // Применяем пассивные и условные модификаторы к навыкам.
            foreach (var worker in workers)
                workerTaskResults.Add(TotalWorkerTaskResult(task.Task, _workerSkillsCalculator.CalculateModifiedSkillsConditional(worker.Worker, _workerSkillsCalculator.CalculateModifiedSkills(worker.Worker), task)));

            foreach (var workerTaskResult in workerTaskResults)
                totalCriticalSuccessAmount += workerTaskResult.CriticalSuccessAmount;

            foreach (var workerTaskResult in workerTaskResults)
                totalCriticalFailureAmount += workerTaskResult.CriticalFailureAmount;

            if (totalCriticalSuccessAmount > totalCriticalFailureAmount)
                result.SetIsCriticalSuccess(true);
            if (totalCriticalFailureAmount > totalCriticalSuccessAmount)
                result.SetIsCriticalFailure(true);
            
            result.SetIsSuccess(IsTaskSuccessful(workerTaskResults));
            
            // Записываем работников и задание.
            result.SetWorkers(workers);
            result.SetTask(task.Task);
            
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
                IsSkillSuccess(patienceRolls, task.PatienceRequired),
                IsSkillSuccess(socialRolls, task.SocialRequired),
                IsSkillSuccess(intellectualRolls, task.IntellectualRequired),
                IsSkillSuccess(physicalRolls, task.PhysicalRequired)
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
            criticalSuccessAmount += CriticalSuccessInRollCount(patienceRolls, task.PatienceRequired); 
            criticalSuccessAmount += CriticalSuccessInRollCount(socialRolls, task.SocialRequired);
            criticalSuccessAmount += CriticalSuccessInRollCount(intellectualRolls, task.IntellectualRequired);
            criticalSuccessAmount += CriticalSuccessInRollCount(physicalRolls, task.PhysicalRequired);
            
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
            criticalFailureAmount += CriticalFailureInRollCount(patienceRolls, task.PatienceRequired); 
            criticalFailureAmount += CriticalFailureInRollCount(socialRolls, task.SocialRequired);
            criticalFailureAmount += CriticalFailureInRollCount(intellectualRolls, task.IntellectualRequired);
            criticalFailureAmount += CriticalFailureInRollCount(physicalRolls, task.PhysicalRequired);
            
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
    }
}
