using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static  GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    public PlayerController[] players;
    public Camera[] cameras;

    int activeCamera;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
        foreach (var c in cameras)
        {
            c.enabled = false;
        }
        cameras[activeCamera].enabled = true;
    }

    public void WinGame(string playerName)
    {
        foreach (var p in players)
        {
            p.Stop();
        }
        Debug.Log(playerName + " has WON!!!");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            activeCamera++;
            if(activeCamera >= cameras.Length)
            {
                activeCamera = 0;
            }
            foreach (var c in cameras)
            {
                c.enabled = false;
            }
            cameras[activeCamera].enabled = true;
        }
    }
}
