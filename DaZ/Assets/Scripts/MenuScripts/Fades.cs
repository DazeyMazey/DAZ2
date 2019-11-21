using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fades : MonoBehaviour
{
    // Start is called before the first frame update

    private CanvasGroup CG; 

    void Start()
    {
        CG = this.transform.GetComponent<CanvasGroup>();
        CG.alpha = 1;

        FadeOut();
    }

    // The black box fades in, end of level
    public void FadeIn()
    {
        StartCoroutine(FadeIn_private());
    }

    private IEnumerator FadeIn_private()
    {
        float i = 0.01f;
        while (CG.alpha < 1)
        {
            CG.alpha += i;
            yield return null;
        }
        FindObjectOfType<DoorBehavior>().GoNext();
    }

    // The black box fades out, beginning of level
    public void FadeOut()
    {
        StartCoroutine(FadeOut_private());
    }

    private IEnumerator FadeOut_private()
    {
        float i = -0.05f;
        while (CG.alpha > 0)
        {
            CG.alpha += i;
            yield return null;
        }
    }


    public void FadeInStart()
    {
        StartCoroutine(FadeInStart_private());
    }

    private IEnumerator FadeInStart_private()
    {
        float i = 0.01f;
        while (CG.alpha < 1)
        {
            CG.alpha += i;
            yield return null;
        }
        FindObjectOfType<StartButton>().NextScene();
    }
}
