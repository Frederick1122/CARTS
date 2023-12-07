// Description : LoadAScene.cs : various functions to load scene
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadAScene : MonoBehaviour {

// --> Load a scene with its build settings number
	public void LoadASceneWithThisNumber(int value){									
		StartCoroutine(E_Load_LCD_With_Int(value));
	}

// --> Reload the scene
	public void ReloadAScene(){															
		StartCoroutine(E_Load_LCD(SceneManager.GetActiveScene().name));
	}

// --> Load a scene using a string
	private IEnumerator E_Load_LCD(string value){										
		AsyncOperation a = SceneManager.LoadSceneAsync(value, LoadSceneMode.Single);
		a.allowSceneActivation = false;
		while(a.isDone){
			Debug.Log("loading "  + value + " : " + a.progress);
			yield return null;
		}
		a.allowSceneActivation = true;
	}

// --> Load a scene using an integer
	private IEnumerator E_Load_LCD_With_Int(int value){
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        AsyncOperation a = SceneManager.LoadSceneAsync(value, LoadSceneMode.Single);
		a.allowSceneActivation = false;
		while(a.isDone){
			Debug.Log("loading "  + value + " : " + a.progress);
			yield return null;
		}
		a.allowSceneActivation = true;
	}


    public void MCR_LoadAScene(string scenName)
    {
        StartCoroutine(E_Load_LCD(scenName));
    }

    public void MCR_LoadMenuScene(int loadingScreen)
    {
        GameObject tmpMenu = GameObject.Find("Canvas_MainMenu");
        if (tmpMenu)
            tmpMenu.GetComponent<Menu_Manager>().GoToOtherPageWithHisNumber(loadingScreen);                   // Open the Loading menu
        StartCoroutine(E_Load_LCD("Menu"));
    }


}
