using System.Collections.Generic;
using Content;
using Runtime;
using UnityEngine;

namespace Generators
{
    public class WorkerGenerator : ScriptableObject
    {
        // Можно создавать генераторы для разных пулов внешностей и черт.
        [SerializeField] private List<WorkerAppearance> appearances;
        [SerializeField] private List<TraitDef> traits;
        
        private readonly int _skillMin = 0;
        private readonly int _skillMax = 5;

        public List<WorkerRuntime> GenerateRandomCandidates(int workerAmount)
        {
            List<WorkerRuntime> workers = new List<WorkerRuntime>();
            
            for (int i = 0; i < workerAmount; i++)
                workers.Add(GenerateRandomWorker());
            
            return workers;
        }

        public WorkerRuntime GenerateRandomWorker()
        {
            WorkerRuntime randomWorker = CreateInstance<WorkerRuntime>();
            
            WorkerAppearance randomAppearance = appearances[Random.Range(0, appearances.Count)];
            TraitDef randomTrait1= traits[Random.Range(0, traits.Count)];
            TraitDef randomTrait2 = traits[Random.Range(0, traits.Count)];

            // Защита от совпадений.
            while (randomTrait1 == randomTrait2)
                randomTrait2 = traits[Random.Range(0, traits.Count)];
            
            int randomPatience = Random.Range(_skillMin, _skillMax);
            int randomSocial = Random.Range(_skillMin, _skillMax);
            int randomIntellectual = Random.Range(_skillMin, _skillMax);
            int basePhysical = Random.Range(_skillMin, _skillMax);
            
            randomWorker.InitializeWorker(randomAppearance, randomPatience, randomSocial, randomIntellectual, basePhysical, randomTrait1, randomTrait2);
            
            return randomWorker;
        }
    }
}