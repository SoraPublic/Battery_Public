using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoder
{
    private string titleScene = "TitleScne";
    private string gameScene = "TestScene";

    public void LoadScene(Scene scene)
    {
        switch (scene)
        {
            case Scene.Title:
                SceneManager.LoadScene(titleScene);
                break;

            case Scene.Game:
                SceneManager.LoadScene(gameScene);
                break;
        }
    }

    public void UnLoadScene(Scene scene)
    {
        switch (scene)
        {
            case Scene.Title:
                SceneManager.UnloadSceneAsync(titleScene);
                break;

            case Scene.Game:
                SceneManager.UnloadSceneAsync(gameScene);
                break;
        }
    }
}

public enum Scene
{
    Title, 
    Game,
}
