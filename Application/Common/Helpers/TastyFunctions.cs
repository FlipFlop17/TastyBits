using Domain.Models;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.Collections.ObjectModel;

namespace Application.Common.Helpers
{
    public static class TastyFunctions
    {
        /// <summary>
        /// Converts selected chips(meal types) to enum types
        /// </summary>
        /// <param name="selectedChips"></param>
        /// <returns>Enumerable of TimeOfDayMeal</returns>
        public static Collection<TimeOfDayMealT> ConvertMealTypes(MudChip[] selectedChips)
        {
            Collection<TimeOfDayMealT> lista = new Collection<TimeOfDayMealT>();
            if (selectedChips != null)
            {
                foreach (var item in selectedChips)
                {
                    lista.Add(
                        (TimeOfDayMealT)Enum.Parse(typeof(TimeOfDayMealT), item.Value.ToString())
                    );
                }
            }
            return lista;

        }
        /// <summary>
        /// Converts uploaded IBrowserfiles to string
        /// </summary>
        /// <returns>List of converted images as string</returns>
        public static async Task<List<MealImage>> ConvertImageToBytesAsync(IEnumerable<IBrowserFile> uploadedFiles)
        {
            List<MealImage> userImages = new List<MealImage>();
            int maxFileSize = 5000000; // Set your maximum file size limit here

            foreach (var imageFile in uploadedFiles)
            {
                var uImg = new MealImage();
                byte[] imageBytes;
                using (var memoryStream = new MemoryStream())
                {
                    var imageStream = imageFile.OpenReadStream(imageFile.Size);
                    await imageStream.CopyToAsync(memoryStream);
                    imageBytes = memoryStream.ToArray();
                    imageStream.Close();
                }
                // Convert the byte array to a Base64-encoded string
                string base64Image = Convert.ToBase64String(imageBytes);
                uImg.Data = base64Image;
                uImg.ImageName = imageFile.Name;
                userImages.Add(uImg);

            }
            return userImages;
        }
    }
}
