using System.ComponentModel.DataAnnotations;

namespace CVhantering.ValidateFolder
{
    public class ValidateObjekts
    {

        public static List<string> ValidateObject<T>(T obj) // T is a generic type
        {
            var context = new ValidationContext(obj, null, null);
            var results = new List<ValidationResult>();
            var errors = new List<string>();

            // The object is validated
            Validator.TryValidateObject(obj, context, results, true);

            // All errors are added to the list
            foreach (var result in results)
            {
                errors.Add(result.ErrorMessage);
            }

            return errors;
        }

    }
}
