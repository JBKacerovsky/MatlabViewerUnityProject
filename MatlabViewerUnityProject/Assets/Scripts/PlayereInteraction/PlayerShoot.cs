using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
	private const string _shootableMesh = "ShootableMesh";
	private Camera cam;
	[SerializeField] public LayerMask mask;
	[SerializeField] private int range = 50000;
	void Start()
	{
		cam = Camera.main;
	}
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space)||Input.GetKeyDown(KeyCode.Mouse1))
		{
			Shoot();
		}
	}
	void Shoot()
	{
		//Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

    	RaycastHit _hit;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out _hit, range, mask))
        {
            if (_hit.collider.tag == _shootableMesh)
            {
				_hit.transform.GetComponent<EmissionController>().ToggleEmission();
				ShootManager.ShootConnections(_hit.collider.name); 
            }
        }
    }
}
