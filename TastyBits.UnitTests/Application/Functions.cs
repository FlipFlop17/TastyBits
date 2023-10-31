using Application.Helpers;
using Domain.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TastyBits.UnitTests.Application
{
    public class Functions
    {
        //write me a unit test for the above method.just the Fact
        [Fact]
        public async Task ConvertImageToBytesAsync_ShouldReturnListOfMealImages()
        {
            // Arrange
            IEnumerable<IBrowserFile> uploadedFiles = new List<IBrowserFile>
            {
                // Add some IBrowser files as needed
            };

            // Act
            var result = await TastyFunctions.ConvertImageToBytesAsync(uploadedFiles);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<MealImage>>(result);
            // Add more assertions as needed
        }
    }
}
