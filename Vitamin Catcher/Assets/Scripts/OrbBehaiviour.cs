using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OrbBehaiviour : MonoBehaviour
{
    public Image orb;
    public Color32[] levelColors;
    private int currentColor = 0;

    void Start()
    {
        DOTween.Init();
    }


    public void DestroyOrb()
    {
        orb.DOFade(0f, 0.5f);
    }

    public void ActivateOrb(int level, float opct)
    {
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
