using System;
using UnityEngine;

public class ActivateInteractButton : MonoBehaviour
{
    public GameObject interactButton;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Interacted with " + interactButton.name);
            interactButton.SetActive(true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactButton.SetActive(false);
        }
    }
}
