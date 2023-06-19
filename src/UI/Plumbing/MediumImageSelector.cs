using System.Drawing.Text;

namespace MusicStore.Plumbing
{
    public class MediumImageSelector
    {

        private const string DEFAULT_GRAPHIC = "placeholder.png";

        // Set medium graphic
        public static string SetMediumGraphic(string medium, string albumArtUrl)
        {
            // Return graphic to product if present
            if (albumArtUrl != null)
            { 
                return albumArtUrl;
            }
            
            string graphicName;

            switch (medium)
            {
                case "EightTrack":
                    graphicName = "eighttrack.png";
                    break;
                case "CD":
                    graphicName = "cd.jpg";
                    break;
                case "CassetteTape":
                    graphicName = "cassette.jpg";
                    break;
                case "Album":
                    graphicName = "album.png";
                    break;
                default:
                    graphicName = DEFAULT_GRAPHIC;
                    break;
            }
            return graphicName;
        }
    }
}
