using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TouziActionScript : MonoBehaviour {

    // Use this for initialization
    public Image touzi;
    public Image touzi2;
    public Image result;
    public Image result2;
    float time;
    int touziIndex = 1;
    Sprite[] spriteArray = new Sprite[21];
    bool canStart = false;
    int dian = 0;
    int dian2 = 0;
	void Start () {
        //InvokeRepeating("playTouzi", 1, 3);
        playTouzi();
        //Invoke("showResult", 2);
        //SoundCtrl.getInstance().playSoundByActionButton(7);
    }

    private void playTouzi()
    {
        for (int i = 1; i < 21; i++)
        {
            spriteArray[i] = Resources.Load("Touzi/touzi_" + i, typeof(Sprite)) as Sprite;
            //yield return new WaitForSeconds(0.1f);
        }
        canStart = true;
    }

    void OnGUI() {
        time += Time.deltaTime;
        if (time > 0.3f && canStart) {
            touzi.sprite = spriteArray[touziIndex];
            int touziIndex2 = (touziIndex - 5)< 1 ? (20 + touziIndex - 5) : (touziIndex - 5);
            touzi2.sprite = spriteArray[touziIndex2];
            touziIndex++;
            if (touziIndex > 20)
                touziIndex = 1;
            time = 0;
        }
    }

    public void setResult(int r1, int r2) {
        if (result != null)
        {
            result.gameObject.SetActive(false);
            result2.gameObject.SetActive(false);
            touzi.gameObject.SetActive(true);
            touzi2.gameObject.SetActive(true);

            canStart = true;
			Invoke("showResult", 2);
            SoundCtrl.getInstance().playSoundByActionButton(7);
        }
        
        dian = r1;
        dian2 = r2;        
    }

    public void showResult()
    {
        canStart = false;
        touzi.gameObject.SetActive(false);
        touzi2.gameObject.SetActive(false);

        result.sprite = Resources.Load("Touzi/touzi" + dian, typeof(Sprite)) as Sprite;
        result2.sprite = Resources.Load("Touzi/touzi" + dian2, typeof(Sprite)) as Sprite;
        result.gameObject.SetActive(true);
        result2.gameObject.SetActive(true);
        //Invoke("close", 1);
    }

    public void close() {
        Destroy(this);
        Destroy(gameObject);
    }



    // Update is called once per frame
    void Update () {
	
	}
}
