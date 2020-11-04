using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBird : Bird
{
    List<Pig> Enmeny = new List<Pig>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Enmeny.Add(collision.gameObject.GetComponent<Pig>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Enmeny.Remove(collision.gameObject.GetComponent<Pig>());
        }
    }

    public override void showSkill()
    {
        base.showSkill();
        if(Enmeny.Count >0 && Enmeny != null)
        {
            for(int i=0;i<Enmeny.Count;i++)
            {
                Enmeny[i].Destory();
            }
        }
    }
}
