using Domain.Models;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.Collections.ObjectModel;

namespace Application.Helpers
{
    public static class TastyFunctions
    {
        /// <summary>
        /// Converts selected chips(meal types) to enum types
        /// </summary>
        /// <param name="selectedChips"></param>
        /// <returns>Enumerable of TimeOfDayMeal</returns>
        public static Collection<TimeOfDayMeal> ConvertMealTypes(MudChip[] selectedChips)
        {
            Collection<TimeOfDayMeal> lista=new Collection<TimeOfDayMeal>();
            if (selectedChips != null) {
                foreach (var item in selectedChips) {
                    lista.Add(
                        (TimeOfDayMeal)Enum.Parse(typeof(TimeOfDayMeal), item.Value.ToString())
                    );
                }
            }
            return lista;

        }
        /// <summary>
        /// Converts uploaded IBrowserfiles to string
        /// </summary>
        /// <returns>List of converted images as string</returns>
        public static async Task<List<string>> ConvertImageToBytesAsync(IEnumerable<IBrowserFile> uploadedFiles)
        {
            List<string> bytes = new List<string>();

            foreach (var imageFile in uploadedFiles) {
                var imageStream = imageFile.OpenReadStream(imageFile.Size);
                byte[] imageBytes;
                using (var memoryStream = new MemoryStream()) {
                    await imageStream.CopyToAsync(memoryStream);
                    imageBytes = memoryStream.ToArray();
                }
                // Convert the byte array to a Base64-encoded string
                string base64Image = Convert.ToBase64String(imageBytes);
                bytes.Add(base64Image);
            }
            return bytes;
        }
    }
}
