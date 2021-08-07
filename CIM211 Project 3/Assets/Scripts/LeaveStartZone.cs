using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveStartZone : MonoBehaviour
{
    public GameModeManager gameModeManager;

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerHealth>())
        {
            gameModeManager.ShowObjective(2);
            gameObject.SetActive(false);
        }
    }
}
