using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageController : MonoBehaviour
{
    public IEnumerator DeactivateImage()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
