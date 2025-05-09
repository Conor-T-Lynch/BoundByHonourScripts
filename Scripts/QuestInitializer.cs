//@Reference https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1?view=net-9.0
//@Reference https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.
//@Reference https://docs.unity3d.com/ScriptReference/MonoBehaviour.html
//@Reference https://docs.unity3d.com/ScriptReference/GameObject.html
//@Reference https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/classes
//@Reference https://www.geeksforgeeks.org/singleton-design-pattern/
//@Reference https://www.youtube.com/watch?v=7GD5D1viVtc&list=PLj0TSSTwoqAy0abF90Ov3H7SDDj9jcpGD&ab_channel=xOctoManx
//@Reference https://www.youtube.com/watch?v=4oCc0btj_ys&ab_channel=xOctoManx
//@Reference https://www.youtube.com/watch?v=J50L85WUtnw&list=PLj0TSSTwoqAy0abF90Ov3H7SDDj9jcpGD&index=3&ab_channel=xOctoManx
//@Reference https://www.youtube.com/watch?v=JEi3wHZfbNA&list=PLj0TSSTwoqAy0abF90Ov3H7SDDj9jcpGD&index=4&ab_channel=xOctoManx
//@Reference https://www.youtube.com/watch?v=bxCdjDceBbw&list=PLj0TSSTwoqAy0abF90Ov3H7SDDj9jcpGD&index=5&ab_channel=xOctoManx
//@Reference https://www.youtube.com/watch?v=a8y6Ul-nX9o&list=PLj0TSSTwoqAy0abF90Ov3H7SDDj9jcpGD&index=6&ab_channel=xOctoManx
//@Reference https://www.youtube.com/watch?v=-X72WioCsPg&list=PLj0TSSTwoqAy0abF90Ov3H7SDDj9jcpGD&index=7&ab_channel=xOctoManx

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestInitializer : MonoBehaviour
{
    public void Start()
    {
        // Quest 1: Defeat village bandits and talk to a villager
        var quest1 = new Quest
        {
            questName = "Return to the Village",
            description = "Defeat bandits in the village and talk to a villager."
        };

        // Objective 1: Kill 3 bandits in the village
        quest1.objectives.Add(new Quest.Objective
        {
            type = Quest.ObjectiveType.Kill,
            targetTag = "VillageBandit",
            requiredAmount = 3
        });

        // Objective 2: Talk to a villager once
        quest1.objectives.Add(new Quest.Objective
        {
            type = Quest.ObjectiveType.Talk,
            targetTag = "Villager",
            requiredAmount = 1
        });

        // Register quest 1 in the quest manager
        QuestManager.Instance.AddQuest(quest1);

        // Quest 2: Track down and kill the traitor
        var quest2 = new Quest
        {
            questName = "Betrayal Uncovered",
            description = "Track down and kill the traitor."
        };

        // Objective: Kill 1 traitor
        quest2.objectives.Add(new Quest.Objective
        {
            type = Quest.ObjectiveType.Kill,
            targetTag = "Traitor",
            requiredAmount = 1
        });

        // Register quest 2
        QuestManager.Instance.AddQuest(quest2);

        // Quest 3: Find the Bandit HQ
        var quest3 = new Quest
        {
            questName = "Find the Bandit HQ",
            description = "Search for the Bandit HQ."
        };

        // Objective: Enter the Bandit HQ area
        quest3.objectives.Add(new Quest.Objective
        {
            type = Quest.ObjectiveType.EnterArea,
            targetTag = "BanditHQZone",
            requiredAmount = 1
        });

        // Register quest 3
        QuestManager.Instance.AddQuest(quest3);

        // Quest 4: Defeat the Bandit Leader
        var quest4 = new Quest
        {
            questName = "Defeat the Bandit Leader",
            description = "Defeat the leader and save your village."
        };

        // Objective: Kill the bandit leader
        quest4.objectives.Add(new Quest.Objective
        {
            type = Quest.ObjectiveType.Kill,
            targetTag = "BanditLeader",
            requiredAmount = 1
        });

        // Register quest 4
        QuestManager.Instance.AddQuest(quest4);
    }
}
