﻿namespace TastyBits.Model.Dto
{
    public class MealDto
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Dictionary<string, string> Ingredients { get; set; }
        public string CookingTime { get; set; }
        public string PrepTime { get; set; }
        public List<byte> Images { get; set; }
    }
}