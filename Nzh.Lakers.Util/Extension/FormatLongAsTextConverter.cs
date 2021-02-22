﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using JsonConverter = Newtonsoft.Json.JsonConverter;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace Nzh.Lakers.Util.Extension
{
    public class FormatLongAsTextConverter : JsonConverter
    {
        public override bool CanConvert(Type type)
        {
            return typeof(long).Equals(type);
        }
        public override void WriteJson(
            JsonWriter writer, object value, JsonSerializer serializer)
        {
            long number = (long)value;
            writer.WriteValue(number.ToString(CultureInfo.InvariantCulture));
        }

        public override object ReadJson(
            JsonReader reader, Type type, object existingValue, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }
    }
}
