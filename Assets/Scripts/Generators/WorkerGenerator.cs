using System.Collections.Generic;
using Content;
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

        public List<WorkerDef> GenerateRandomCandidates(int workerAmount)
        {
            List<WorkerDef> workers = new List<WorkerDef>();
            
            for (int i = 0; i < workerAmount; i++)
                workers.Add(GenerateRandomWorker());
            
            return workers;
        }

        private WorkerDef GenerateRandomWorker()
        {
            WorkerDef randomWorker = CreateInstance<WorkerDef>();
            
            WorkerAppearance randomAppearance = appearances[Random.Range(0, appearances.Count)];
            TraitDef randomTrait1= traits[Random.Range(0, traits.Count)];
            TraitDef randomTrait2 = traits[Random.Range(0, traits.Count)];

            // Защита от совпадений.
            while (randomTrait1 == randomTrait2 || !AreTraitsCompatible(randomTrait1, randomTrait2))
                randomTrait2 = traits[Random.Range(0, traits.Count)];
            
            int randomPatience = Random.Range(_skillMin, _skillMax);
            int randomSocial = Random.Range(_skillMin, _skillMax);
            int randomIntellectual = Random.Range(_skillMin, _skillMax);
            int randomPhysical = Random.Range(_skillMin, _skillMax);
            List<TraitDef> randomTraits = new List<TraitDef>
            {
                randomTrait1,
                randomTrait2
            };

            randomWorker.InitializeWorkerDef(randomAppearance, randomPatience, randomSocial, randomIntellectual, randomPhysical, randomTraits);
            
            return randomWorker;
        }
        
        private bool AreTraitsCompatible(TraitDef traitA, TraitDef traitB)
        {
            var incompatibilitiesA = traitA.GetType().GetCustomAttributes(typeof(IncompatibleWithAttribute), true);
        
            foreach (IncompatibleWithAttribute attr in incompatibilitiesA)
            {
                foreach (var incompatibleType in attr.Types)
                {
                    if (incompatibleType == traitB.GetType())
                        return false;
                }
            }
            
            var incompatibilitiesB = traitB.GetType().GetCustomAttributes(typeof(IncompatibleWithAttribute), true);
        
            foreach (IncompatibleWithAttribute attr in incompatibilitiesB)
            {
                foreach (var incompatibleType in attr.Types)
                {
                    if (incompatibleType == traitA.GetType())
                        return false;
                }
            }
        
            return true;
        }
    }
}