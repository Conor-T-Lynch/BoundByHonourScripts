//@Reference https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.LoadScene.html

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class MainMenuController : MonoBehaviour
{
    //Method to start a new game
    public void StartNewGame()
    {
        //Load into the sample scene
        SceneManager.LoadScene("SampleScene");
    }

}