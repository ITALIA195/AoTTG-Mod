using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CharacterCreateAnimationControl : MonoBehaviour
{
    private static Dictionary<string, int> _heroes; // MOD: Original name f__switchSmap0
    private string currentAnimation;
    private float interval = 10f;
    private HERO_SETUP setup;
    private float timeElapsed;

    private void Play(string id)
    {
        this.currentAnimation = id;
        base.animation.Play(id);
    }

    public void PlayAttack(string id)
    {
        string key = id;
        if (key != null)
        {
            if (_heroes == null)
            {
                _heroes = new Dictionary<string, int>(7)
                {
                    {"mikasa", 0},
                    {"levi", 1},
                    {"sasha", 2},
                    {"jean", 3},
                    {"marco", 4},
                    {"armin", 5},
                    {"petra", 6}
                };
            }
            if (_heroes.TryGetValue(key, out int num))
            {
                switch (num)
                {
                    case 0:
                        this.currentAnimation = "attack3_1";
                        break;
                    case 1:
                        this.currentAnimation = "attack5";
                        break;
                    case 2:
                        this.currentAnimation = "special_sasha";
                        break;
                    case 3:
                        this.currentAnimation = "grabbed_jean";
                        break;
                    case 4:
                        this.currentAnimation = "special_marco_0";
                        break;
                    case 5:
                        this.currentAnimation = "special_armin";
                        break;
                    case 6:
                        this.currentAnimation = "special_petra";
                        break;
                    default:
                        throw new IndexOutOfRangeException("@ CharacterCreateAnimationControl");
                }
            }
        }
        base.animation.Play(this.currentAnimation);
    }

    private void Start()
    {
        this.setup = base.gameObject.GetComponent<HERO_SETUP>();
        this.currentAnimation = "stand_levi";
        this.Play(this.currentAnimation);
    }

    public void ToStand()
    {
        if (this.setup.myCostume.Sex == Sex.Female)
        {
            this.currentAnimation = "stand";
        }
        else
        {
            this.currentAnimation = "stand_levi";
        }
        base.animation.CrossFade(this.currentAnimation, 0.1f);
        this.timeElapsed = 0f;
    }

    private void Update()
    {
        if ((this.currentAnimation != "stand") && (this.currentAnimation != "stand_levi"))
        {
            if (base.animation[this.currentAnimation].normalizedTime >= 1f)
            {
                switch (this.currentAnimation)
                {
                    case "attack3_1":
                        this.Play("attack3_2");
                        break;
                    case "special_sasha":
                        this.Play("run_sasha");
                        break;
                    default:
                        this.ToStand();
                        break;
                }
            }
        }
        else
        {
            this.timeElapsed += Time.deltaTime; 
            if (this.timeElapsed > this.interval)
            {
                this.timeElapsed = 0f;
                //MOD: What the actual fuck is this? 
                if (UnityEngine.Random.Range(1, 100) < 35) 
                {
                    this.Play("salute");
                }
                else if (UnityEngine.Random.Range(1, 100) < 35)
                {
                    this.Play("supply");
                }
                else
                {
                    this.Play("dodge");
                }
            }
        }
    }
}

