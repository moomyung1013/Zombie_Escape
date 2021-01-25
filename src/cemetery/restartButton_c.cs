using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class restartButton_c : MonoBehaviour
{
    public void ButtonClick()

    {
        SceneManager.LoadScene("CemeteryScene");

    }
}
