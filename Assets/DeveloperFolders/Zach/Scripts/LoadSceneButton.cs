using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneButton : ButtonEvents
{
    [SerializeField]
    string sceneName;
   public override void Click(){
        SceneManager.LoadScene(sceneName);
   }
}
