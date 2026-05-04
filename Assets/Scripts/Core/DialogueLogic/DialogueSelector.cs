using System.Linq;
using Runtime;

namespace Core.DialogueLogic
{
    public class DialogueSelector
    {
        public DialogueNode SelectNode(DialogueProfile profile, WorkerRuntime worker, int currentDay,
            DialogueConditions condition)
        {
            if (!profile || profile.DialogueNodes() == null || profile.DialogueNodes().Count == 0)
                return null;
            
            return profile.DialogueNodes()
                .Where(n => IsSuitable(n, worker, currentDay, condition))
                .OrderByDescending(n => n.Priority)
                .FirstOrDefault();
        }
        
        public DialogueNode SelectNodeWorkerDef(DialogueProfile profile, int currentDay,
            DialogueConditions condition)
        {
            if (!profile || profile.DialogueNodes() == null || profile.DialogueNodes().Count == 0)
                return null;
            
            return profile.DialogueNodes()
                .Where(n => IsSuitableWorkerDef(n, currentDay, condition))
                .OrderByDescending(n => n.Priority)
                .FirstOrDefault();
        }

        private bool IsSuitable(DialogueNode node, WorkerRuntime worker, int currentDay, DialogueConditions condition)
        {
            if (node.Condition != condition && node.Condition != DialogueConditions.Default)
                return false;
            
            if (currentDay < node.MinDay || currentDay > node.MaxDay)
                return false;
            
            if (worker.Loyalty < node.MinLoyalty || worker.Loyalty > node.MaxLoyalty)
                return false;
            
            if (worker.Productivity < node.MinProductivity || worker.Productivity > node.MaxProductivity)
                return false;
            
            if (node.PlayOnlyOnce && node.WasPlayed)
                return false;
            
            return true;
        }
        
        private bool IsSuitableWorkerDef(DialogueNode node, int currentDay, DialogueConditions condition)
        {
            if (node.Condition != condition && node.Condition != DialogueConditions.Default)
                return false;
            
            if (currentDay < node.MinDay || currentDay > node.MaxDay)
                return false;
            
            if (node.PlayOnlyOnce && node.WasPlayed)
                return false;
            
            return true;
        }

        private bool IsDefault(DialogueNode node)
        {
            return node.Condition == DialogueConditions.Default;
        }
    }
}