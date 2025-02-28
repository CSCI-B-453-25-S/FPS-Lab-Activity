using System.Collections;
using UnityEngine;

public class PressurePlate : MonoBehaviour, ITriggerable
{
    [SerializeField] GameObject doorlight;
    [SerializeField] float cooldown = 1f;

    private bool canActivate = true;
    public void Trigger()
    {
        MyEvents.someEvent.Invoke(5);
        if (!canActivate) return;

        StartCoroutine(Cooldown());
        if (doorlight.activeSelf)
        {
            doorlight.SetActive(false);
        }
        else
        {
            doorlight.SetActive(true);
        }
    }

    private void Awake()
    {
        doorlight = transform.GetChild(0).gameObject;
        doorlight.SetActive(false);
    }
    private IEnumerator Cooldown()
    {
        canActivate = false;
        yield return new WaitForSeconds(cooldown);
        canActivate = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Trigger();
        }
    }
}
