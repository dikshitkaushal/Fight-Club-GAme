using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class enemycontroller : MonoBehaviour
{
    public Transform playertransform;
    private Vector3 direction;
    static Animator anim2;
    public float enemyhealth = 100f;
    public Slider enemyhb;
    public static enemycontroller instance;
    public BoxCollider[] c;
    public AudioClip[] audioclip;
    AudioSource source;
    Vector3 enemypos ;
    // Start is called before the first frame update
    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
        }
    }
    void Start()
    {
        anim2 = GetComponent<Animator>();
        disableboxcollider(false);
        source = GetComponent<AudioSource>();
        enemypos = transform.position;
    }
    public void disableboxcollider(bool state)
    {
        c[0].enabled = state;
        c[1].enabled = state;
    }
    public void playaudio(int clip)
    {
        source.clip = audioclip[clip];
        source.Play();
    }
    // Update is called once per frame
    void Update()
    {
        if(anim2.GetCurrentAnimatorStateInfo(0).IsName("fight_idleCopy"))
        {
            direction=playertransform.position-this.transform.position;
            direction.y = 0;
            this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.3f);
            disableboxcollider(false);
        }
        Debug.Log(direction.magnitude);
        if(direction.magnitude>10f && gamecontroller.iswalking==true)
        {
            anim2.SetTrigger("movefrwd");
            source.Stop();
            disableboxcollider(false);
        }
        else
        {
            anim2.ResetTrigger("movefrwd");
      
        }
        if (direction.magnitude <=10f && direction.magnitude>=7f && gamecontroller.iswalking==true)
        {
            anim2.SetTrigger("kick");
            disableboxcollider(true);
            if (!source.isPlaying && !anim2.GetCurrentAnimatorStateInfo(0).IsName("roundhouse_kick 2"))
            {
                playaudio(2);
            }
        }
        else
        {
            anim2.ResetTrigger("kick");
        }
        if (direction.magnitude <= 7f && gamecontroller.iswalking == true)
        {
            anim2.SetTrigger("punch");
            disableboxcollider(true);
            if (!source.isPlaying && !anim2.GetCurrentAnimatorStateInfo(0).IsName("roundhouse_kick 2"))
            {
                playaudio(1);
            }
        }
        else
        {
            anim2.ResetTrigger("punch");
        }
        if (direction.magnitude >0 && direction.magnitude<5f && gamecontroller.iswalking == true)
        {
            anim2.SetTrigger("moveback");
            source.Stop();
            disableboxcollider(false);
        }
        else
        {
            anim2.ResetTrigger("moveback");
        }
    }
    public void enemyreact()
    {
        enemyhealth -= 10f;
        playaudio(0);
        enemyhb.value = enemyhealth/100f;
        if (enemyhealth < 0)
        {
            enemyknockout();
            playaudio(3);
        }
        else
        {
            anim2.ResetTrigger("idle");
            anim2.SetTrigger("react");
        }
    }

    private void enemyknockout()
    {
        anim2.SetTrigger("knockout");
        gamecontroller.iswalking = false;
        gamecontroller.instance.scoreplayer();
        gamecontroller.instance.onscreenpoints();
        gamecontroller.instance.rounds();
        if(gamecontroller.playerscore==2)
        {
            gamecontroller.instance.doreset();
        }
        else
        {
            StartCoroutine(resetcharacters());
        }
    }
    IEnumerator resetcharacters()
    {
        yield return new WaitForSeconds(4);
        enemyhealth = 100;
        enemyhb.value = enemyhealth / 100f;
        GameObject[] theclone = GameObject.FindGameObjectsWithTag("ee");
        Transform t = theclone[1].GetComponent<Transform>();
        t.position = enemypos;
        t.position = new Vector3(t.position.x, 0, t.position.z);
        gamecontroller.iswalking = true;
    }
}
