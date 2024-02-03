
    using System;
    using System.Linq;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class ConvertJObject<T>
    {
        public T ConvertJObjectToConditionData(JObject jObject)
        {
            var typeName = jObject["type"].ToObject<string>();
            var conditionType = typeof(T).Assembly.GetTypes().FirstOrDefault(t => t.Name == typeName);
     
            if (conditionType == null || !typeof(T).IsAssignableFrom(conditionType))
            {
                throw new NotSupportedException($"{typeof(T)} type '{typeName}' is not supported.");
            }
     
            var condition = (T)Activator.CreateInstance(conditionType);
             
            // Use the JsonReader directly
            using (var jObjectReader = jObject.CreateReader())
            {
                // Populate the object
                JsonSerializer.CreateDefault().Populate(jObjectReader, condition);
            }
     
            return condition;
        }
    
    }
