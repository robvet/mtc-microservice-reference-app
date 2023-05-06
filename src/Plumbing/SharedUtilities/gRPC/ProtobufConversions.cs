using System;
using System.Collections.Generic;
using System.Text;
using Google.Protobuf.WellKnownTypes;

namespace SharedUtilities.gRPC
{
    public static class ProtobufConversions
    {
        public static Timestamp ConvertDateTime(object dateString)
        {
            // https://lifetime-engineer.com/protocolbuffers-timestamp/
            // https://stackoverflow.com/questions/39348238/google-protobuff-timestamp-proto-in-c-sharp
            var convertDateToString = dateString.ToString();
            var convertToDateTime = DateTime.Parse(convertDateToString);
            var utcTime = TimeZoneInfo.ConvertTimeToUtc(convertToDateTime);
            return Timestamp.FromDateTime(utcTime);
        }
    }
}
