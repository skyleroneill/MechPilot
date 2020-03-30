using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitGameButton : ButtonEvents{

   public override void Click(){
        Application.Quit();
   }
}
