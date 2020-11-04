using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : MonoBehaviour
{
    
    public Sprite hurtPicture_01;
    public Sprite hurtPicture_02;
    public Sprite hurtPicture_03;
    public GameObject boom;
    public GameObject score;
    public bool isPig = false;

    public float Defense = 0.1f;
    public float HP = 100;

    private SpriteRenderer render;

    public AudioClip hurtClip;
    public AudioClip dead;
    public AudioClip birdCollision;

    private void Awake()
    {
        render = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "player")
        {
            AudioPlay(birdCollision);
        }

        HP -= collision.relativeVelocity.magnitude/Defense;

        if (HP>50 && HP<=75)
        {
            render.sprite = hurtPicture_01;
        }
        else if(HP>25 && HP<=50)
        {
            render.sprite = hurtPicture_02;
            AudioPlay(hurtClip);
        }
        else if(HP>0 && HP<=25)
        {
            render.sprite = hurtPicture_03;
        }
        else if(HP<=0)
        {
            Destory();
        }
    }

     public void Destory()
    {   if(isPig)
        {
            GameManager._instance.pigs.Remove(this);
        }
        Destroy(gameObject);
        Instantiate(boom, transform.position, Quaternion.identity);
        GameObject go = Instantiate(score, transform.position + new Vector3(0,0.8f,0), Quaternion.identity);
        Destroy(go, 1.5f);
        AudioPlay(dead);
    }

    public void AudioPlay(AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip, transform.position);
    }
}
