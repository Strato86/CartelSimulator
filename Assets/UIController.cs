using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Image gunImage;
    public Image knifeImage;
    public Image accord;
    public Text moneyAmount;
    public Text seedAmount;
    public Text mariaAmount;


    public void UpdateUI(WorldModel wm)
    {
        moneyAmount.text = wm.money.ToString();
        seedAmount.text = wm.seeds.ToString();
        mariaAmount.text = wm.maria.ToString();

        accord.enabled = wm.hasAgreement;
        knifeImage.enabled = wm.weapon == "knife";
        gunImage.enabled = wm.weapon == "gun";
    }
}
