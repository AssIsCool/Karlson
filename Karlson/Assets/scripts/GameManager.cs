using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public string SceneName;
    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }
    private void Start()
    {
        if (player == null) return;
        player = FindObjectOfType<Health>().gameObject;
    }
    private void Update()
    {
        if (player == null) return;
        if (player.GetComponent<Health>().curentHealth <= 0)
        {
            SceneManager.LoadScene(SceneName);

        }        

    }
}
