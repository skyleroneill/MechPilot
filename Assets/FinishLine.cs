using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    [SerializeField]
    GameObject winScreen;
    bool triggered;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if(other.gameObject.name == "PrototypeMech")
        {
            triggered = true;
            winScreen.SetActive(true);
            StartCoroutine("WaitThenMenu");
        }
    }

    IEnumerator WaitThenMenu()
    {
        // suspend execution for 5 seconds
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Zachs Menu");
    }

}
