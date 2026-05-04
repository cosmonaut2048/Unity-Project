using System.Linq;
using Runtime;

namespace Core.DialogueLogic
{
    public class DialogueSelector
    {
        public DialogueNode SelectNode(DialogueProfile profile, WorkerRuntime worker, int currentDay,
            DialogueTriggers trigger)
        {
            if (!profile || profile.DialogueNodes() == null || profile.DialogueNodes().Count == 0)
                return null;

            return profile.DialogueNodes()
                .Where(n => IsSuitable(n, worker, currentDay, trigger))
                .OrderByDescending(n => n.Priority)
                .FirstOrDefault();
        }

        private bool IsSuitable(DialogueNode node, WorkerRuntime worker, int currentDay, DialogueTriggers trigger)
        {
            if (node.Trigger != trigger && node.Trigger != DialogueTriggers.Default)
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
    }
}