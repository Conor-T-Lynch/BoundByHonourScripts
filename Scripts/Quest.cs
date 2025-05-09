//@Reference https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1?view=net-9.0
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

public class Quest
{
    // The name of the quest
    public string questName;

    // A brief description of the quest
    public string description;

    // Flag indicating whether the quest has been completed
    public bool isCompleted = false;

    // Enum to define the type of objective (e.g., kill enemies, talk to NPCs, enter areas)
    public enum ObjectiveType { Kill, Talk, EnterArea }

    // Inner class representing an individual objective within a quest
    public class Objective
    {
        // The type of the objective (Kill, Talk, EnterArea)
        public ObjectiveType type;

        // The tag identifying the target for this objective
        public string targetTag;

        // The number of times the target must be interacted with (e.g., kill count, talk count)
        public int requiredAmount;

        // The current progress made towards completing this objective
        public int currentAmount;

        // Property to check whether this individual objective is complete
        public bool IsComplete => currentAmount >= requiredAmount;
    }

    // A list containing all objectives for this quest
    public List<Objective> objectives = new List<Objective>();

    // Method to check if all objectives are completed and mark the quest as complete
    public bool CheckCompletion()
    {
        // Loop through all objectives
        foreach (var obj in objectives)
        {
            // If any objective is not complete, return false
            if (!obj.IsComplete)
                return false;
        }

        // If all objectives are complete, mark the quest as completed
        isCompleted = true;
        return true;
    }
}
