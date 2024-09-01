using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Project.RuntimeLayer
{
    public static class ModelStateExtensions
    {
        public static Dictionary<string, string[]> GetValidationErrors(this ModelStateDictionary modelState)
        {
            return modelState
                .Where(ms => ms.Value.Errors.Any())
                .ToDictionary(
                    ms => ms.Key,
                    ms => ms.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );
        }
    }
}
