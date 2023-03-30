using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake
{
   
        public class SnowflakeIdGenerator
        {
            public static string GenerateId(SnowflakeEnum artifact)
            {
                var token = new StringBuilder();
                token.Append(artifact);
                token.Append("-");
                token.Append(DateTime.Now.ToShortDateString().Replace("/", "."));
                token.Append("-");
                token.Append(DateTime.Now.ToShortTimeString()).Replace(" ", "");
                token.Append("-");
                //token.Append(new Random().Next(0, 1000000).ToString("D6"));
                token.Append(new Random().Next(0, 1000000000).ToString("D10"));

                return token.ToString();
            }
   }
}
