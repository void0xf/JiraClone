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
    }
}