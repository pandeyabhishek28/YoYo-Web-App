using Newtonsoft.Json;
using Serilog;
using System;
using YoYo_Web_App.Domain.Models;

namespace YoYo_Web_App.Domain.JsonConverters
{
    public class TimeJsonConverter : JsonConverter<Time>
    {
        public override Time ReadJson(JsonReader reader, Type objectType, Time existingValue, bool hasExistingValue, JsonSerializer serializer)
        {

            if (reader.Value != null)
            {
                var value = reader.Value.ToString();
                if (!string.IsNullOrEmpty(value))
                {
                    var values = value.Split(':');
                    try
                    {
                        if (values.Length == 2)
                        {
                            if (int.TryParse(values[0], out int hour) && int.TryParse(values[1], out int minute))
                            {
                                return new Time(hour, minute);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "An error occured while parsing.");
                    }
                }
            }

            return new Time(0, 0);
        }

        public override void WriteJson(JsonWriter writer, Time value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}
