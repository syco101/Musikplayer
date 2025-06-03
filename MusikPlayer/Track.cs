using System.IO;

namespace MusikPlayer
{
    public class Track
    {
        public string FilePath { get; }
        public string Title => Path.GetFileNameWithoutExtension(FilePath);

        public Track(string filePath)
        {
            FilePath = filePath;
        }
    }
}
