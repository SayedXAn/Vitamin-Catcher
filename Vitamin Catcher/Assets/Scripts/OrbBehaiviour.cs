using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OrbBehaiviour : MonoBehaviour
{
    public Image orb;
    public Image[] healthBars;
    public Color32[] levelColors;
    private int currentColor = 0;

    void Start()
    {
        DOTween.Init();
        orb.color = levelColors[0];
        for (int i = 0; i < healthBars.Length; i++)
        {
            healthBars[i].color = new Color32(levelColors[i].r, levelColors[i].g, levelColors[i].b, 255);
        }
    }


    public void DestroyOrb()
    {
        orb.DOFade(0f, 0.5f);
    }

    public void ActivateOrb(int level, float opct)
    {
        healthBars[level].DOFillAmount(opct, 0.5f);
        if(currentColor != level)
        {
            currentColor = level;
            Sequence mySequence = DOTween.Sequence();
            
            mySequence.Append(orb.DOFade(0f, 0.5f));
            mySequence.Append(orb.DOColor(levelColors[currentColor], 0.1f));
            mySequence.Append(orb.DOFade(opct, 0.5f));
        }
        else
        {
            orb.DOFade(opct, 0.5f);
        }
        
    }
}
