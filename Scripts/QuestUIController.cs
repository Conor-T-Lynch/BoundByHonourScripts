//@Reference https://learn.microsoft.com/en-us/dotnet/api/system.text.stringbuilder?view=net-9.0
//@Reference https://docs.unity3d.com/ScriptReference/Input.GetKeyDown.html
//@Reference https://docs.unity3d.com/ScriptReference/GameObject.SetActive.html
//@Reference https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
//@Reference https://www.youtube.com/watch?v=7GD5D1viVtc&list=PLj0TSSTwoqAy0abF90Ov3H7SDDj9jcpGD&ab_channel=xOctoManx
//@Reference https://www.youtube.com/watch?v=4oCc0btj_ys&ab_channel=xOctoManx
//@Reference https://www.youtube.com/watch?v=J50L85WUtnw&list=PLj0TSSTwoqAy0abF90Ov3H7SDDj9jcpGD&index=3&ab_channel=xOctoManx
//@Reference https://www.youtube.com/watch?v=JEi3wHZfbNA&list=PLj0TSSTwoqAy0abF90Ov3H7SDDj9jcpGD&index=4&ab_channel=xOctoManx
//@Reference https://www.youtube.com/watch?v=bxCdjDceBbw&list=PLj0TSSTwoqAy0abF90Ov3H7SDDj9jcpGD&index=5&ab_channel=xOctoManx
//@Reference https://www.youtube.com/watch?v=a8y6Ul-nX9o&list=PLj0TSSTwoqAy0abF90Ov3H7SDDj9jcpGD&index=6&ab_channel=xOctoManx
//@Reference https://www.youtube.com/watch?v=-X72WioCsPg&list=PLj0TSSTwoqAy0abF90Ov3H7SDDj9jcpGD&index=7&ab_channel=xOctoManx
//@Reference https://www.geeksforgeeks.org/stringbuilder-in-c-sharp/
//@Reference https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/interpolated
//@Reference https://learn.microsoft.com/en-us/dotnet/csharp/tutorials/string-interpolation

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class QuestUIController : MonoBehaviour
{
    // Reference to the UI text component that displays the quest log
    public TextMeshProUGUI questLogText;

    // Reference to the quest log panel GameObject
    public GameObject questLog;

    public void Update()
    {
        // Toggle the visibility of the quest log when the player presses the Select button (joystick button 6)
        if (Input.GetKeyDown("joystick button 6"))
        {
            // Toggle visibility
            questLog.SetActive(!questLog.activeSelf); 
        }

        // If the QuestManager or UI text is missing, don't continue
        if (QuestManager.Instance == null || questLogText == null) return;

        // Get the current active quest
        var currentQuest = QuestManager.Instance.GetCurrentQuest();

        // Use a StringBuilder for efficient string manipulation
        StringBuilder sb = new StringBuilder();

        // If there's an active quest, display its name, description, and objectives
        if (currentQuest != null)
        {
            sb.AppendLine($"<b>Active Quest:</b> {currentQuest.questName}");
            sb.AppendLine(currentQuest.description);

            // List each objective with completion status
            foreach (var obj in currentQuest.objectives)
            {
                string status = obj.currentAmount >= obj.requiredAmount ? "[X]" : "[ ]";
                sb.AppendLine($"{status} {obj.type} {obj.targetTag} - {obj.currentAmount}/{obj.requiredAmount}");
            }
        }
        else
        {
            // If all quests are complete, show a final congratulatory message
            sb.AppendLine("All quests completed, you have saved your village from the bandits! well done!!.");
        }

        sb.AppendLine();
        sb.AppendLine("Press [Select] to hide/unhide this window.");

        // Set the formatted string as the quest log text
        questLogText.text = sb.ToString();
    }
}
