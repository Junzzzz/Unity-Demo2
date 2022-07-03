using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterHouseEvent : DialogTriggerEvent
{
    protected override void DoEvent()
    {
        if (Input.GetKey(KeyCode.E))
        {
            SceneManager.LoadScene("Scenes/Level02");
        }
    }
}