using System.Text.Json.Serialization;

namespace GAME.PhotoRanking.Models.File
{
    public class ShortFileInfo 
    {
        public string Id { get; set; }
        public string FileName { get; set; }
        public long Size { get; set; }

        [JsonPropertyName("md5")]
        public string MD5 { get; set; }
        public string ContentType { get; set; }
        public DateTime UploadDate { get; set; }
    }
}
