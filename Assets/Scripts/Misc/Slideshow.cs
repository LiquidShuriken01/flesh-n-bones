using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SlideShowController : MonoBehaviour
{
    public Image[] slides;      // Array to hold all the slides
    public AudioSource[] musicTracks;  // Array to hold background music tracks
    private int currentSlideIndex = 0;
    private int currentTrackIndex = 0;
    public string firstScene;

    void Start()
    {
        // ChatGPT used to generate large parts of this code.
        ShowCurrentSlide();
        PlayCurrentMusicTrack();
    }

    void Update()
    {
        // Check for player input (mouse click)
        if (Input.GetMouseButtonDown(0))  // Change to Input.GetButtonDown("Fire1") for better control
        {
            // Advance to the next slide
            NextSlide();
        }
    }

    void NextSlide()
    {
        // Check if there are more slides
        if (currentSlideIndex < slides.Length - 1)
        {
            currentSlideIndex++;
            ShowCurrentSlide();

            // Change music track every 3 slides
            if (currentSlideIndex % 3 == 0)
            {
                NextMusicTrack();
            }
        }
        else
        {
            // Load a different scene when reaching the final slide
            LoadNextScene();
        }
    }

    void ShowCurrentSlide()
    {
        // Hide all slides
        foreach (Image slide in slides)
        {
            slide.gameObject.SetActive(false);
        }

        // Show the current slide
        slides[currentSlideIndex].gameObject.SetActive(true);
    }

    void NextMusicTrack()
    {
        // Increment the music track index and loop back to the first track if necessary
        currentTrackIndex = (currentTrackIndex + 1) % musicTracks.Length;
        PlayCurrentMusicTrack();
    }

    void PlayCurrentMusicTrack()
    {
        // Stop all music tracks
        foreach (AudioSource track in musicTracks)
        {
            track.Stop();
        }

        // Play the current music track
        musicTracks[currentTrackIndex].Play();
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(firstScene);
    }
}