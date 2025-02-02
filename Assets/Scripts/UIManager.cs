using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject objetoPrefab; 
    private GameObject objetoInstanciado; 
    public Transform spawnPoint; 

    public AudioSource audioSource;
    public AudioClip musicaDeFondo; 

    private void Start()
    {
        
        if (audioSource != null && musicaDeFondo != null)
        {
            audioSource.clip = musicaDeFondo;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

   
    public void CrearObjeto()
    {
        
        if (objetoInstanciado != null)
        {
            Debug.Log("Ya existe un objeto creado. No puedes crear otro hasta eliminarlo.");
            return;
        }

        
        objetoInstanciado = Instantiate(objetoPrefab, spawnPoint.position, spawnPoint.rotation);
        objetoInstanciado.tag = "Target"; 
        Debug.Log("Objeto creado.");
    }

   
    public void EliminarObjeto()
    {
        if (objetoInstanciado != null)
        {
            Destroy(objetoInstanciado);
            objetoInstanciado = null; 
            Debug.Log("Objeto eliminado.");
        }
        else
        {
            Debug.Log("No hay ningún objeto que eliminar.");
        }
    }
}
