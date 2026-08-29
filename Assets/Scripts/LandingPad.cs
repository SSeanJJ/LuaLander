using UnityEngine;

public class LandingPad : MonoBehaviour {
 
   [SerializeField] private int scoreMultiplier;


    public int GetScoreMultiplier()
    {
        return scoreMultiplier;
    }
}
