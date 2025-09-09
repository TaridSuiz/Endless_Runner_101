using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

   
    public GameObject gameOverUI;
    public Text scoreUi;
    public Text hiScoreUi;

  
    public AudioSource bgmSource;        // ลาก AudioSource ที่มี "Happy" (Loop, PlayOnAwake) มาวาง
    public AudioClip gameplayBgm;        // ถ้ามีก็ใส่ (ไม่ใส่ก็ใช้ clip ที่อยู่บน bgmSource ได้)
    public AudioClip gameOverBgm;        // เพลงพื้นหลังตอน Game Over
    [Range(0f, 1f)] public float bgmVolume = 1f;
    public float bgmFadeDuration = 0.75f;


    public AudioClip gameOverSfx;        
    private AudioSource sfxSource;       

    int score = 0;
    bool isGameOver;

    private void Awake()
    {
        if (instance == null) instance = this;

        
        if (!bgmSource) bgmSource = GetComponent<AudioSource>();
        if (bgmSource)
        {
            bgmSource.spatialBlend = 0f;     
            bgmSource.loop = true;
            bgmSource.volume = bgmVolume;

            
            if (gameplayBgm) bgmSource.clip = gameplayBgm;
            if (!bgmSource.isPlaying && bgmSource.clip) bgmSource.Play();
        }

        
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;
        sfxSource.spatialBlend = 0f;
        sfxSource.ignoreListenerPause = true; 
    }

    private void Start()
    {
        scoreUi.text = "     Score : " + score;
        int hiScore = PlayerPrefs.GetInt("Highscore");
        hiScoreUi.text = "    High Score : " + hiScore;
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        // 1) แสดง UI และหยุดสกอร์/ฉากเลื่อน
        ObstacleSpawner.instance.isGameOver = true;
        gameOverUI.SetActive(true);
        StopScroll();

        // 2) เล่นเอฟเฟกต์จบเกม
        if (gameOverSfx) sfxSource.PlayOneShot(gameOverSfx);

        // 3) สลับเพลงพื้นหลังเป็นเพลง GameOver (พร้อมเฟด)
        StartCoroutine(SwapBgm(gameOverBgm, bgmFadeDuration));

     
    }

    IEnumerator SwapBgm(AudioClip newClip, float fadeDur)
    {
        if (!bgmSource || !newClip) yield break;

        float startVol = bgmSource.volume;
        float t = 0f;

        // เฟดเอาท์
        while (t < fadeDur)
        {
            t += Time.unscaledDeltaTime;
            bgmSource.volume = Mathf.Lerp(startVol, 0f, t / fadeDur);
            yield return null;
        }

        // เปลี่ยนคลิปแล้วเล่น
        bgmSource.Stop();
        bgmSource.clip = newClip;
        bgmSource.Play();

        // เฟดอิน
        t = 0f;
        while (t < fadeDur)
        {
            t += Time.unscaledDeltaTime;
            bgmSource.volume = Mathf.Lerp(0f, bgmVolume, t / fadeDur);
            yield return null;
        }
        bgmSource.volume = bgmVolume;
    }

    IEnumerator SwapBgmAfterSfx(AudioClip newClip)
    {
        if (gameOverSfx)
            yield return new WaitForSecondsRealtime(gameOverSfx.length);
        yield return StartCoroutine(SwapBgm(newClip, bgmFadeDuration));
    }

    public void StopScroll()
    {
        var scrolls = UnityEngine.Object.FindObjectsByType<TextureScroll>(FindObjectsSortMode.InstanceID);
        foreach (var scroll in scrolls) scroll.isScrolling = false;
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }

    public void IncreaseScore()
    {
        score += 1;
        if (score >= PlayerPrefs.GetInt("Highscore", 0))
            PlayerPrefs.SetInt("Highscore", score);

        scoreUi.text = "     Score : " + score;
    }
}
