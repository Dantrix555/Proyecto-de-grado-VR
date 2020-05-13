using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotableComponent : MonoBehaviour
{

    private string _componentFormula = default;

    public string ComponentFormula { get => _componentFormula; set => _componentFormula = value; }

    public void SetVelocity(Vector3 direction, float speed)
    {
        GetComponent<Rigidbody>().velocity = direction * speed;
        Destroy(gameObject, 5);
    }

    //Temporally everything destroys the shot
    private void OnTriggerEnter(Collider other)
    {
        //If the component and the objects are correct set the destroy animation and sound
        if((other.gameObject.tag == "Wall" || other.gameObject.tag == "Water") && _componentFormula == other.gameObject.GetComponent<DestructableObject>().WeaknessComponent.ToString())
        {
            other.gameObject.GetComponent<DestructableObject>().SetDestroyAnimation();
            if(other.gameObject.tag == "Wall")
                SoundManager.LoadSoundEffect(SoundManager.SFX.CanDestroyWall);
            else
                SoundManager.LoadSoundEffect(SoundManager.SFX.CanPurifyWater);
            Destroy(gameObject);
        }
        //Else just set a can't destroy object sound
        else if(other.gameObject.tag == "Wall")
            SoundManager.LoadSoundEffect(SoundManager.SFX.CantDestroyWall);
        else if(other.gameObject.tag == "Water")
            SoundManager.LoadSoundEffect(SoundManager.SFX.CantPurifyWater);

        if (other.gameObject.tag != "Enemy" && other.gameObject.tag != "Player")
        {
            SoundManager.LoadSoundEffect(SoundManager.SFX.ShotHitScenario);
            Destroy(gameObject);
        }
    }
}
