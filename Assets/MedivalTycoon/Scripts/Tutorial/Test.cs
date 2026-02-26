using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] TutorialSpotlight tutorialSpotlight;
    [SerializeField] RectTransform Player;
    [SerializeField] RectTransform Timer;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            tutorialSpotlight.MoveSpotlight(Player);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            tutorialSpotlight.MoveSpotlight(Timer);
        }
    }
}
