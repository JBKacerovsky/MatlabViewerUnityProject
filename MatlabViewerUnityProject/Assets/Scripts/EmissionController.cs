using UnityEngine;

public class EmissionController : MonoBehaviour
{
    public bool emission = false;
    private Material material; 
    private bool fire = false; 
    void Start()
    {
        material = GetComponent<Renderer>().material;
        SetEmission(false);      // Emission needs to be turned on by default so the emission component of the shader is compiled
    }
    void Update()
    {
        ConditionalResetAllEmmiters();
        ConditionalExpandShooting();
    }

    public void ToggleEmission()
    {
        emission = !emission;
        material.SetInt("_emissionToggle", emission ? 1 :0);
    }
    public void SetEmission(bool targetState)
    {
        emission = targetState;
        material.SetInt("_emissionToggle", emission ? 1 :0);
    }
    public void SetShootability(int shootablity)
    {
        if (shootablity == 1)
        {
            gameObject.layer = LayerMask.NameToLayer("shootable"); 
            transform.tag = "ShootableMesh";
            return; 
        }
        gameObject.layer = LayerMask.NameToLayer("UnShootable"); 
        transform.tag = "UnShootableMesh";
    }
        private void ConditionalResetAllEmmiters()
    {
        if (Input.GetKey(KeyCode.A) & Input.GetKey(KeyCode.E))
        {
            SetEmission(false);
        }
    }
    private void ConditionalExpandShooting()
    {
        // the double conditional KeyDown and KeyUp seems a bit silly
        // but it ensures that the selection doesn't keep spreading
        // with only the first condition the next selected objects will also sometimes fire and select the next etc
        // presumably if their update is called slightly later in the framee they will already have emission== true and the key press is still active
        // this way only objects that had emission on when the key is pressed will pass their id to KeepShooting once the key is released (and since the emissions are only set after the key releease, the next elements cannot have emission == true and keyDown)
        if (emission == true & Input.GetKeyDown(KeyCode.P))
        {
            fire = true;
        }
        if (fire == true & Input.GetKeyUp(KeyCode.P))
        {
            ShootManagerScript.KeepShooting(transform.name);
            fire = false;
        }
    }
}
