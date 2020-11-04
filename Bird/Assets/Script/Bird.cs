using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bird : MonoBehaviour
{
    public Transform Pos;
    public float maxDisance = 1.2f;
    public GameObject boom;
    public float smooth = 3;

    public LineRenderer right;
    public LineRenderer left;
    public Transform rightPos;
    public Transform leftPos;

    [HideInInspector] public SpringJoint2D sp;

    private bool isClick = false;
    
    private TestMyTrail myTrail;
    [HideInInspector]public bool canMove = false;
    [HideInInspector]public bool isFly = false;
    private bool canShowSkill = false;
    protected Rigidbody2D rg;
    

    public AudioClip select;
    public AudioClip fly;

    public Sprite hurt;
    private SpriteRenderer render;

    private void Awake()
    {
        sp = GetComponent<SpringJoint2D>();
        rg = GetComponent<Rigidbody2D>();
        myTrail = GetComponent<TestMyTrail>();
        render = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        if (canMove)
        {
            AudioPlay(select);
            isClick = true;
            rg.isKinematic = true;
        }
    }

    private void OnMouseUp()
    {
        if (canMove)
        {
            isClick = false;
            rg.isKinematic = false;
            Invoke("Fly", 0.1f);
            right.enabled = false;
            left.enabled = false;
            canMove = false;
        }
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (isClick)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0,0, -Camera.main.transform.position.z);
            if(Vector3.Distance(transform.position,Pos.position)>maxDisance)
            {
                Vector3 pos = (transform.position - Pos.position).normalized;//向量单位化
                pos *= maxDisance;
                transform.position = pos + Pos.position;
            }
            Line();
        }

        float posX = transform.position.x;
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position,new Vector3(Mathf.Clamp(posX,0,15),
            Camera.main.transform.position.y, Camera.main.transform.position.z),smooth*Time.deltaTime);

        if (canShowSkill)
        {
            if (Input.GetMouseButtonDown(0))
            {
                showSkill();
                canShowSkill = false;
            }
        }
    }

    void Fly()
    {
        isFly = true;
        canShowSkill = true;
        AudioPlay(fly);
        myTrail.StartTrials();
        sp.enabled = false;
        Invoke("Next", 3);
    }

    void Line()
    {
        right.enabled = true;
        left.enabled = true;

        right.SetPosition(0, rightPos.position);
        right.SetPosition(1, transform.position);

        left.SetPosition(0, leftPos.position);
        left.SetPosition(1, transform.position);
    }

    void Next()
    {
        GameManager._instance.birds.Remove(this);
        Destroy(gameObject);
        Instantiate(boom, transform.position, Quaternion.identity);
        GameManager._instance.NextBird();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isFly == true)
            Hurt();
        canShowSkill = false;
        isFly = false;
        myTrail.ClearTrail();
    }

    public void AudioPlay(AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip,transform.position);
    }

    public virtual void showSkill()
    {
        canShowSkill = false;
    }

    public void Hurt()
    {
        render.sprite = hurt;
    }
}
