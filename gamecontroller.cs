using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
public class gamecontroller : MonoBehaviour
{
    public static gamecontroller instance;
    public static bool iswalking = false;
    public GameObject playerscoreonscreen;
    public GameObject camerabutton;
    public GameObject enemyscoreonscreen;
    public GameObject backbutton;
    public GameObject fwdbutton;
    public GameObject punchbutton;
    public GameObject kickbutton;
    private bool player321 = false;
    public AudioClip[] clipp;
    AudioSource source;
    public static int playerscore = 0;
    public static int enemyscore = 0;
    public GameObject[] points;
    public static int round = 0;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }
    public void playaudiosource(int clip)
    {
        source.clip = clipp[clip];
        source.Play();
    }
    public void doreset()
    {
        if(playerscore==2)
        {
            playaudiosource(5);
        }
        else
        {
            playaudiosource(6);
        }
        playercontroller.instance.health = 100f;
        playercontroller.instance.playerhb.value = 1;

        enemycontroller.instance.enemyhealth = 100f;
        enemycontroller.instance.enemyhb.value = 1f;

        playerscore = 0;
        enemyscore = 0;
        StartCoroutine(restartgame());
    }
    IEnumerator restartgame()
    {
        yield return new WaitForSeconds(4.5f);
        points[0].SetActive(false);
        points[1].SetActive(false);
        points[2].SetActive(false);
        points[3].SetActive(false);
        iswalking = true;
        StartCoroutine(restartroundaudio());
    }
    IEnumerator restartroundaudio()
    {
        yield return new WaitForSeconds(2);
        playaudiosource(0);
    }
    // Update is called once per frame
    public void scoreplayer()
    {
        playerscore++;
    }
    public void scoreenemy()
    {
        enemyscore++;
    }
    void Update()
    {
        if(player321==false)
        {
            if(DefaultTrackableEventHandler.truefalse==true)
            {
                camerabutton.SetActive(false);
                playerscoreonscreen.SetActive(true);
                enemyscoreonscreen.SetActive(true);
                backbutton.SetActive(true);
                fwdbutton.SetActive(true);
                punchbutton.SetActive(true);
                kickbutton.SetActive(true);
                player321 = true;
                StartCoroutine(round1());
            }
        }
    }

    IEnumerator round1()
    {
        yield return new WaitForSeconds(1.5f);
        playaudiosource(0);
        StartCoroutine(prepareyourself());
    }
    IEnumerator prepareyourself()
    {
        yield return new WaitForSeconds(2f);
        playaudiosource(1);
        StartCoroutine(start321());
    }
    IEnumerator start321()
    {
        yield return new WaitForSeconds(1.5f);
        playaudiosource(2);
        StartCoroutine(allowplayermovement());
    }
    IEnumerator allowplayermovement()
    {
        yield return new WaitForSeconds(5f);
        iswalking = true;
    }
    public void onscreenpoints()
    {
        if(playerscore==1)
        {
            points[0].SetActive(true);
        }
        else if(playerscore==2)
        {
            points[1].SetActive(true);
        }
        if (enemyscore == 1)
        {
            points[2].SetActive(true);
        }
        else if (enemyscore == 3)
        {
            points[3].SetActive(true);
        }
    }
    public void rounds()
    {
        round = playerscore + enemyscore;
        if(round==1)
        {
            playaudiosource(3);
        }
        if(round==2 && playerscore!=2 && enemyscore!=2)
        {
            playaudiosource(4);
        }
    }
}
