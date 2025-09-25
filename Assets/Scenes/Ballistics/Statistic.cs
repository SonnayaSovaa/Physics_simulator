using TMPro;
using UnityEngine;

public class Statistic : MonoBehaviour
{
    private int _targetsHit = 0;
    private int _miss = 0;

    [SerializeField] private TMP_Text targetText;
    [SerializeField] private TMP_Text missText;
    

    void ChangeText(TMP_Text text, int value)
    {
        text.text = "" + value;
    }

    public void NewHit()
    {
        _targetsHit += 1;
        ChangeText(targetText, _targetsHit);
    }

    public void NewMiss()
    {
        _miss += 1;
        ChangeText(missText, _miss);
    }


}
