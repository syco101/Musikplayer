using NAudio.Wave;
using MusikPlayer; 


public class MusicPlayer
{
    private WaveOutEvent outputDevice;
    private AudioFileReader audioFile;

    public void Play(Track track)
    {
        Stop();
        audioFile = new AudioFileReader(track.FilePath);
        outputDevice = new WaveOutEvent();
        outputDevice.Init(audioFile);
        outputDevice.Play();
    }

    public void Stop()
    {
        outputDevice?.Stop();
        outputDevice?.Dispose();
        audioFile?.Dispose();
        outputDevice = null;
        audioFile = null;
    }

    public void Pause() => outputDevice?.Pause();
    public void Resume() => outputDevice?.Play();
    public void SetVolume(float volume)
    {
        if (outputDevice != null)
            outputDevice.Volume = volume;
    }

    public TimeSpan GetTotalTime() => audioFile?.TotalTime ?? TimeSpan.Zero;
    public TimeSpan? GetCurrentTime() => audioFile?.CurrentTime;
}
