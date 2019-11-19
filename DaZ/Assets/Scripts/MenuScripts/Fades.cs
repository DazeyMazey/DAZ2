using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fades : MonoBehaviour
{
    // Start is called before the first frame update

    private Image Fader;

    void Start()
    {
        Fader = this.transform.GetChild(0).GetComponent<Image>();

        FadeOut();
    }

    // The black box fades in, end of level
    public void FadeIn()
    {
        float i = 0;
        while (Fader.color.a < 1)
        {
            Fader.color = new Color(255, 255, 255, i);
            i += 0.1f;
        }
        FindObjectOfType<DoorBehavior>().GoNext();
    }

    // The black box fades out, beginning of level
    public void FadeOut()
    {
        float i = 0;
        while (Fader.color.a < 1)
        {
            Fader.color = new Color(255, 255, 255, i);
            i += 0.1f;
        }
    }
}
