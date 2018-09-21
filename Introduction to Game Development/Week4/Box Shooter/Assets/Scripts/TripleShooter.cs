using UnityEngine;
using System.Collections;

public class TripleShooter : MonoBehaviour {

    // Reference to projectile prefab to shoot
    public GameObject projectile;
    public float power = 10.0f;
    public float shootSpeed = 0.2f;

    // Reference to AudioClip to play
    public AudioClip shootSFX;

    private int isShooting = 0;
    private Object lockShooting = new Object();

    // Update is called once per frame
    void Update() {
        // Detect if fire button is pressed
        if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Jump")) {
            // if projectile is specified
            if (projectile) {
                if (isShooting == 0) {
                    // call shoot and delay for each projectile
                    for (int i = 0; i < 3; i++) {
                        Invoke("Shoot", shootSpeed * i);
                    }
                }
            }
        }
    }

    void Shoot() {
        // Instantiante projectile at the camera + 1 meter forward with camera rotation
        GameObject newProjectile = Instantiate(projectile, transform.position + transform.forward, transform.rotation) as GameObject;

        // if the projectile does not have a rigidbody component, add one
        if (!newProjectile.GetComponent<Rigidbody>()) {
            newProjectile.AddComponent<Rigidbody>();
        }
        // Apply force to the newProjectile's Rigidbody component if it has one
        newProjectile.GetComponent<Rigidbody>().AddForce(transform.forward * power, ForceMode.VelocityChange);

        // play sound effect if set
        if (shootSFX) {
            if (newProjectile.GetComponent<AudioSource>()) {
                // the projectile has an AudioSource component
                // play the sound clip through the AudioSource component on the gameobject.
                // note: The audio will travel with the gameobject.
                newProjectile.GetComponent<AudioSource>().PlayOneShot(shootSFX);
            } else {
                // dynamically create a new gameObject with an AudioSource
                // this automatically destroys itself once the audio is done
                AudioSource.PlayClipAtPoint(shootSFX, newProjectile.transform.position);
            }
        }

        lock (lockShooting) {
            isShooting = (isShooting + 1) % 3;
        }
    }
}
