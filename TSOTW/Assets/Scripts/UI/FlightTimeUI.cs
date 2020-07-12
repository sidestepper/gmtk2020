using TMPro;
using UnityEngine;

public class FlightTimeUI : MonoBehaviour {
    [SerializeField]
    private TMP_Text _text = null;

    public void SetFlightTime(int secondsRemaining) {
        int minutes = Mathf.FloorToInt(secondsRemaining / 60);
        int seconds = secondsRemaining - (minutes * 60);
        if(seconds < 10) {
            _text.text = $"{minutes}:0{seconds}";
        } else {
            _text.text = $"{minutes}:{seconds}";
        }
    }
}
