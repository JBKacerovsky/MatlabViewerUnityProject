using UnityEngine;

public class EmissionController : MonoBehaviour
{
    [SerializeField] private bool _emission = false;
    private Material material; 

    void Start()
    {
        // at first I wanted to set these on the prefab but doing it this way
        // ensures that only meshes with the script attached are shootable
        // (so PlayerShoot will not accidentally try to call ToggleEmission() on objects without this script
        // it also ensures that all meshes with this script are in fact shootable (in case I add more mesh types in the future and forget)
        transform.tag = "ShootableMesh";    
        gameObject.layer = LayerMask.NameToLayer("shootable"); 

        material = GetComponent<Renderer>().material;
        EmissionOff();      // Emission needs to be turned on by default so the emission component of the shader is com
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A)&Input.GetKey(KeyCode.E))
        {
            EmissionOff();
        }
    }

    public void ToggleEmission()
    {
        _emission = !_emission;
        material.SetInt("_emissionToggle", _emission ? 1 :0);
    }

    private void EmissionOff()
    {
        _emission = false;
        material.SetInt("_emissionToggle", 0);
    }
}
