using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public Image Panel;
    float time = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver(float waitTime, float F_time)
    {
        StartCoroutine(FadeFlow(waitTime,F_time));
    }

    IEnumerator FadeFlow(float waitTime, float F_time)
    {
        yield return new WaitForSeconds(waitTime);
        Panel.gameObject.SetActive(true);
        Color alpha = Panel.color;
        while(alpha.a < 1f)
        {
            time += Time.deltaTime /F_time;
            alpha.a = Mathf.Lerp(0,1,time);
            Panel.color = alpha;
            yield return null;
        }
        time = 0f;
        // yield return new WaitForSeconds(1.0f);
        // while(alpha.a > 0f)
        // {
        //     time += Time.deltaTime /F_time;
        //     alpha.a = Mathf.Lerp(1,0,time);
        //     Panel.color = alpha;
        //     yield return null;
        // }
        // Panel.gameObject.SetActive(false);

        yield return null;
    }

    
}
