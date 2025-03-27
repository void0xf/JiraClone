namespace SharedKernel.Utils
{
    public static class ObjectExtensions
    {
        public static void CopyNonNullProperties(object source, object target)
        {
            // Get all properties from the source object
            var properties = source.GetType().GetProperties()
                .Where(prop => prop.CanRead && prop.CanWrite);

            foreach (var prop in properties)
            {
                // Get the value from source
                var value = prop.GetValue(source);
                
                // Only copy if the value is not null
                if (value != null)
                {
                    // Find the corresponding property in target
                    var targetProp = target.GetType().GetProperty(prop.Name);
                    
                    // If matching property exists and can be written to
                    if (targetProp != null && targetProp.CanWrite)
                    {
                        // Set the value to the target
                        targetProp.SetValue(target, value);
                    }
                }
            }
        }
        
        // Improved version that handles collections and explicitly marked properties
        public static void CopyPropertiesForPatch<T>(T source, T target, HashSet<string> explicitlySetProperties = null)
        {
            if (source == null || target == null)
                return;
                
            // Get all properties from the source object
            var properties = typeof(T).GetProperties()
                .Where(prop => prop.CanRead && prop.CanWrite);

            foreach (var prop in properties)
            {
                // If we have a list of explicitly set properties and this property isn't in it, skip it
                if (explicitlySetProperties != null && !explicitlySetProperties.Contains(prop.Name))
                    continue;
                
                // Get the value from source
                var value = prop.GetValue(source);
                
                // Set the value to the target, even if it's null (for PATCH semantics)
                prop.SetValue(target, value);
            }
        }
    }
}