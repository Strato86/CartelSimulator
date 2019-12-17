using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeedheadController : MonoBehaviour
{
    public GameObject cigar;

    private void Start()
    {
        cigar.SetActive(false);
    }
    public void TurnOnCigar()
    {
        cigar.SetActive(true);
    }

    IEnumerator CigarTimer()
    {
        yield return new WaitForSeconds(5);
        cigar.SetActive(false);
    }
}
