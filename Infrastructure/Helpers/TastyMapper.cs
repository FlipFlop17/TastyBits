using Domain.Models;
using Infrastructure.Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;

namespace Application.Helpers
{
    public class TastyMapper
    {
        public static MealsDataEntity ConvertUserMealToMealsEntity(UserMeal incomingMeal)
        {
            MealsDataEntity mealDto = new MealsDataEntity();
            mealDto.Id = incomingMeal.MealId;
            mealDto.Name = incomingMeal.Name;
            mealDto.Description = incomingMeal.Description;
            mealDto.CookingTime = incomingMeal.CookingTime;
            mealDto.PrepTime = incomingMeal.PrepTime;
            mealDto.ServingsAmount = incomingMeal.ServingAmount;
            mealDto.Instructions = incomingMeal.Instructions;
            mealDto.UserId = incomingMeal.UserId;

            //map time of day
            foreach (var item in incomingMeal.TimeOfDayMeal) {
                switch (item) {
                    case TimeOfDayMealT.Breakfast:
                        mealDto.IsBreakfast = true;
                        break;
                    case TimeOfDayMealT.Dinner:
                        mealDto.IsDinner = true;
                        break;
                    case TimeOfDayMealT.Lunch:
                        mealDto.IsLunch = true;
                        break;
                    case TimeOfDayMealT.Snack:
                        mealDto.IsSnack = true;
                        break;
                    case TimeOfDayMealT.Dessert:
                        mealDto.IsDesert = true;
                        break;
                    default:
                        break;
                }
            }

            //map images
            mealDto.RecipeImages = new List<MealImageDataEntity>();
            foreach (var item in incomingMeal.Images) {
                var img = ConvertMealImageToImageEntity(item,incomingMeal.MealId);
                mealDto.RecipeImages.Add(img);
            }

            //map ingredients
            mealDto.RecipeIngridients = new List<MealIngredientsDataEntity>();
            foreach(var item in incomingMeal.Ingredients) {
                var ing = ConvertMealIngredientToMealIngEntity(item, incomingMeal.MealId);
                mealDto.RecipeIngridients.Add(ing);
            }

            return mealDto;
        }
        public static MealImageDataEntity ConvertMealImageToImageEntity(MealImage mealImg,int mealId)
        {
            MealImageDataEntity mealImageDataEntity = new MealImageDataEntity();
            mealImageDataEntity.MealId = mealId;
            mealImageDataEntity.ImageData = mealImg.Data;
            mealImageDataEntity.ImageName=mealImg.ImageName;
            mealImageDataEntity.ValidUntil = mealImg.ValidUntil;
            return mealImageDataEntity;
        }
        public static MealIngredientsDataEntity ConvertMealIngredientToMealIngEntity(UserMeal.Ingridient ingredient,int mealId)
        {
            MealIngredientsDataEntity mealIngredient= new MealIngredientsDataEntity();
            mealIngredient.Id = ingredient.Id;
            mealIngredient.IngredientId = ingredient.IngredientId;
            mealIngredient.MealId = mealId;
            mealIngredient.Quantity= ingredient.Quantity;
            mealIngredient.QuantityUnit = Convert.ToString(ingredient.QuantityUnit);
            if(ingredient.IsDeleted)
                mealIngredient.ValidUntil= DateTime.Now;
            return mealIngredient;
        }
        public static IngredientsDataEntity ConvertIngredientsToIngredientsDataEntity(UserMeal.Ingridient ingredient)
        {
            IngredientsDataEntity ingredientEntity = new IngredientsDataEntity();
            ingredientEntity.Id = ingredient.IngredientId;
            ingredientEntity.Name = ingredient.Name;
            ingredientEntity.CaloriesPer100Gram = ingredient.CaloriesPer100g;
            //TODO add other mappings like sugar, proetin etc.
            return ingredientEntity;
        }

        public static UserMeal ConvertUserMealDataEntityToUserMealModel(MealsDataEntity mealData)
        {
            UserMeal userMeal = new UserMeal();
            userMeal.MealId = mealData.Id;
            userMeal.UserId=mealData.UserId;
            userMeal.TimeOfDayMeal = new();
            userMeal.Name = mealData.Name;
            userMeal.Description = mealData.Description;
            userMeal.CookingTime = mealData.CookingTime;
            userMeal.PrepTime = mealData.PrepTime;
            userMeal.ServingAmount = mealData.ServingsAmount;
            userMeal.Instructions = mealData.Instructions;
            if (mealData.IsBreakfast) 
                userMeal.TimeOfDayMeal.Add(TimeOfDayMealT.Breakfast);
            if (mealData.IsDinner)
                userMeal.TimeOfDayMeal.Add(TimeOfDayMealT.Dinner);
            if (mealData.IsLunch)
                userMeal.TimeOfDayMeal.Add(TimeOfDayMealT.Lunch);
            if (mealData.IsSnack)
                userMeal.TimeOfDayMeal.Add(TimeOfDayMealT.Snack);
            if (mealData.IsDesert)
                userMeal.TimeOfDayMeal.Add(TimeOfDayMealT.Dessert);

            foreach (var item in mealData.RecipeImages) {
                userMeal.Images.Add(ConvertMealImageEntityToMealImageUser(item));
            }
            foreach (var item in mealData.RecipeIngridients) {
                userMeal.Ingredients.Add(ConvertMealsIngredientDataEntityToUserIngredient(item));
            }
            return userMeal;
        }

        public static UserMeal.Ingridient ConvertMealsIngredientDataEntityToUserIngredient(MealIngredientsDataEntity ingredientDO)
        {
            UserMeal.Ingridient userIngre=new UserMeal.Ingridient();
            userIngre.Id = ingredientDO.Id;
            userIngre.IngredientId = ingredientDO.IngredientId;
            userIngre.Quantity = ingredientDO.Quantity;
            QuantityUnit res;
            Enum.TryParse(ingredientDO.QuantityUnit,out res);
            userIngre.QuantityUnit = res;
            userIngre.Name = ingredientDO.Ingredients.Name;
            userIngre.CaloriesPer100g = ingredientDO.Ingredients.CaloriesPer100Gram;
            return userIngre;
        }

        public static MealImage ConvertMealImageEntityToMealImageUser(MealImageDataEntity imgDataEntity)
        {
            MealImage userMealImage = new MealImage();
            userMealImage.Data = imgDataEntity.ImageData;
            userMealImage.ImageName = imgDataEntity.ImageName;
            userMealImage.MealId=Convert.ToString(imgDataEntity.MealId);
            userMealImage.Id = Convert.ToString(imgDataEntity.ImageId);
            userMealImage.ValidUntil = imgDataEntity.ValidUntil;
            return userMealImage;
        }

    }
}
