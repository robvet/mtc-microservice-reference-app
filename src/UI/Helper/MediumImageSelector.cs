namespace MusicStore.Helper
{
    public class MediumImageSelector
    {
        // Set medium graphic
        public static string SetMediumGraphic(string medium)
        {
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
                    graphicName = "album.jpg";
                    break;
                default:
                    graphicName = "placeholder.png";
                    break;
            }
            return graphicName;
        }
    }
}
