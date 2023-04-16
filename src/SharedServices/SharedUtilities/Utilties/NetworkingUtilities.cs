using System.Text.RegularExpressions;

namespace SharedUtilities.Utilties
{
    public static class NetworkingUtilities
    {
        /// <summary>
        /// Validates IP Address format
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsValidIp(this string ip)
        {
            if (!Regex.IsMatch(ip, "[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}"))
                return false;
            var ips = ip.Split('.');

            if (ips.Length == 4 || ips.Length == 6)
                return int.Parse(ips[0]) < 256 && int.Parse(ips[1]) < 256
                       & int.Parse(ips[2]) < 256 & int.Parse(ips[3]) < 256;

            return false;
        }

        /// <summary>
        /// Validates URL format
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsValidUrl(this string text)
        {
            var rx = new Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
            return rx.IsMatch(text);
        }
    }
}