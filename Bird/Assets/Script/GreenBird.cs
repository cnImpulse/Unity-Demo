using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBird : Bird
{
    public override void showSkill()
    {
        base.showSkill();
        Vector3 speed = rg.velocity;
        speed.x *= -1;
        rg.velocity = speed;
    }

}
