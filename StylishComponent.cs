using UnityEngine;

public class StylishComponent : MonoBehaviour
{
#pragma warning disable 169
    public GameObject bar;
    private int chainKillRank;
    private float[] chainRankMultiplier;
    private float chainTime;
    private float duration;
    private Vector3 exitPosition;
    private bool flip;
    private bool hasLostRank;
    public GameObject LabelChain;
    public GameObject labelHits;
    public GameObject labelS;
    public GameObject labelS1;
    public GameObject labelS2;
    public GameObject labelsub;
    public GameObject labelTotal;
    private Vector3 originalPosition;
    private float R;
    private int styleHits;
    private float stylePoints;
    private int styleRank;
    private int[] styleRankDepletions;
    private int[] styleRankPoints;
    private string[,] styleRankText;
    private int styleTotalDamage;
#pragma warning restore 169

    public void Update()
    {
        Destroy(GameObject.Find("Stylish"));
        Destroy(LabelChain);
        Destroy(labelHits);
        Destroy(labelS);
        Destroy(labelS1);
        Destroy(labelS2);
        Destroy(labelsub);
        Destroy(labelTotal);
        Destroy(this);
    }
}

