using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleController : MonoBehaviour
{
    public void SetFade(string fadeState)
    {
        int i;
        for(i = 0; i < transform.childCount; i++)
        {
            if(fadeState == "In")
            {
                StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.In, transform.GetChild(i).gameObject));
            }
            else
            {
                StartCoroutine(InGameManager.SetFade(InGameManager.FadeOperation.Out, transform.GetChild(i).gameObject));
            }
        }
    }
}
