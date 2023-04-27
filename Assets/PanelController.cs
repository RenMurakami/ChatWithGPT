using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public GameObject dialoguePanel;
    public Text dialogueText;
    public float triggerDistance = 5.0f;

    private Transform playerTransform;
    private bool isDialogueVisible = false;

    void Start()
    {
        
    }


    void Update()
    {
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if (distance <= triggerDistance && !isDialogueVisible)
        {
            ShowDialogue();
        }
        else if (distance > triggerDistance && isDialogueVisible)
        {
            HideDialogue();
        }
    }

    private void ShowDialogue()
    {
        dialoguePanel.SetActive(true);
        isDialogueVisible = true;
    }

    private void HideDialogue()
    {
        dialoguePanel.SetActive(false);
        isDialogueVisible = false;
    }
}
