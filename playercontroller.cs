using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playercontroller : MonoBehaviour
{
    public Transform enemy;
    public static bool backward = false;
    public static bool forward = false;
    public static bool isattacking = false;
    public float health = 100;
    private Vector3 direction;
    static Animator anim;
    public Slider playerhb;
    public static playercontroller instance;
    public BoxCollider[] c;
    public AudioClip[] audioclip;
    AudioSource source;
    private Vector3 playerpos;
    public void Awake()
    {
        if(instance==null)
        {
            instance = this;
        }
    }
    public void disableboxcollider(bool state)
    {
        c[0].enabled = state;
        c[1].enabled = state;
    }
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        disableboxcollider(false);
        source = GetComponent<AudioSource>();
        playerpos = transform.position;
    }
    public void playaudio(int clip)
    {
        source.clip = audioclip[clip];
        source.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("fight_idle"))
        {
            direction = enemy.position - this.transform.position;
            direction.y = 0;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.4f);
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("fight_idle"))
        {
            isattacking = false;
            disableboxcollider(false);
        }
        if (gamecontroller.iswalking == true)
        {
            if (isattacking == false)
            {
                forw();
                backw();
            }
            if (isattacking == true)
            {
                disableboxcollider(true);
            }
        }
    }

    private void backw()
    {
        if (backward == true)
        {
            anim.SetTrigger("bckwrd");
            anim.ResetTrigger("idle");
            disableboxcollider(false);
        }
        else if(forward==false)
        {
            anim.SetTrigger("idle");
            anim.ResetTrigger("bckwrd");
        }
    }

    private void forw()
    {
        if (forward == true)
        {
            anim.SetTrigger("frwd");
            anim.ResetTrigger("idle");
            disableboxcollider(false);
        }
        else if(backward==false)
        {
            anim.SetTrigger("idle");
            anim.ResetTrigger("frwd");
        }
    }
    public void kick()
    {
        isattacking = true;
        anim.ResetTrigger("idle");
        anim.SetTrigger("kick");
        playaudio(2);
    }
    public void punch()
    {
        isattacking = true;
        anim.ResetTrigger("idle");
        anim.SetTrigger("punch");
        playaudio(1);
    }
    public void react()
    {
        health -= 10;
        playerhb.value = health/100f;
        if (health < 0)
        {
            knockout();
            playaudio(3);
        }
        else
        {
            isattacking = true;
            anim.ResetTrigger("idle");
            anim.SetTrigger("react");
            playaudio(0);
        }
    }
    public void knockout()
    {
        anim.SetTrigger("knockout");
        gamecontroller.instance.scoreenemy();
        gamecontroller.instance.onscreenpoints();
        gamecontroller.instance.rounds();
        if (gamecontroller.enemyscore == 2)
        {
            gamecontroller.instance.doreset();
            StartCoroutine(resetcharacters());
        }
        else
        {
            StartCoroutine(resetcharacters());
        }
    }
    IEnumerator resetcharacters()
    {
        yield return new WaitForSeconds(4);
        health = 100f;
        playerhb.value = health / 100f;
        GameObject[] theclone = GameObject.FindGameObjectsWithTag("pp");
        Transform t = theclone[1].GetComponent<Transform>();
        anim.SetTrigger("idle");
        anim.ResetTrigger("knockout");
        t.position = playerpos;
        t.position = new Vector3(t.position.x, 0, t.position.z);
        gamecontroller.iswalking = true;
    }
}
