using UnityEngine;
using UnityEngine.UI;

public class Panel_Controller : MonoBehaviour
{
    public GameObject panel;
    public float distanceThreshold = 10.0f;

    private bool isDialogueVisible = false;

    void Start()
    {
        
    }

    void Update()
    {
        // Your existing Update() code

        bool catInRange = false;
        GameObject[] cats = GameObject.FindGameObjectsWithTag("cat");
        foreach (GameObject cat in cats)
        {
            float distance = Vector3.Distance(transform.position, cat.transform.position);
            if (distance <= distanceThreshold)
            {
                catInRange = true;
                break;
            }
        }

        panel.SetActive(catInRange);
    }


    private void ShowDialogue()
    {
        panel.SetActive(true);
        isDialogueVisible = true;
    }

    private void HideDialogue()
    {
        panel.SetActive(false);
        isDialogueVisible = false;
    }
}
