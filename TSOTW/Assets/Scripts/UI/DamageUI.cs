using TMPro;
using UnityEngine;

public class DamageUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _text = null;

    public void SetDamage(int current, int total) {
        float percent = ((float)current / (float)total) * 100f;
        _text.text = $"{(int)percent}%";
    }
}
