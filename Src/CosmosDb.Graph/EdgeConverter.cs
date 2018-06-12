using System;
using System.Reflection;
using Microsoft.Azure.Graphs.Elements;

using CosmosDb.Graph.Interfaces;

namespace CosmosDb.Graph
{
    public class EdgeConverter : IEdgeConverter
    {
        public T ToObject<T>(Edge edge) where T : EdgeBase, new()
        {
            if (edge == null) throw new ArgumentNullException(nameof(edge));

            var objectType = typeof(T);
            var obj = new T 
            { 
                id = edge.Id.ToString(),
                SourceId = edge.OutVertexId.ToString(),
                TargetId = edge.InVertexId.ToString()
            };

            foreach (var edgeProperty in edge.GetProperties())
            {
                var property = objectType.GetProperty(edgeProperty.Key);
                var value = edgeProperty.Value;
                if ("DateTime".Equals(property.PropertyType.Name)) value = value.ToString().Substring(value.ToString().IndexOf(":") + 1);
                property?.SetValue(obj, Convert.ChangeType(value, property.PropertyType));
            }

            return obj;
        }
    }
}