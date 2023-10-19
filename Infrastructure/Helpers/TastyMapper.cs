using Domain.Models;
using Infrastructure.Data.Context;

namespace Application.Helpers
{
    public class TastyMapper
    {
        public static MealsDataEntity ConvertUserMealToMealsEntity(UserMeal incomingMeal)
        {
            MealsDataEntity mealDto = new MealsDataEntity();
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
                    case TimeOfDayMeal.Breakfast:
                        mealDto.IsBreakfast = true;
                        break;
                    case TimeOfDayMeal.Dinner:
                        mealDto.IsDinner = true;
                        break;
                    case TimeOfDayMeal.Lunch:
                        mealDto.IsLunch = true;
                        break;
                    case TimeOfDayMeal.Snack:
                        mealDto.IsSnack = true;
                        break;
                    case TimeOfDayMeal.Dessert:
                        mealDto.IsDesert = true;
                        break;
                    default:
                        break;
                }
            }

            return mealDto;
        }
        public static MealImageDataEntity ConvertMealImageToImageEntity(string imgBytes,int mealId)
        {
            MealImageDataEntity mealImageDataEntity = new MealImageDataEntity();
            mealImageDataEntity.MealId = mealId;
            mealImageDataEntity.ImageData = imgBytes;
            return mealImageDataEntity;
        }
        public static MealIngredientsDataEntity ConvertMealIngredientToMealIngEntity(UserMeal.Ingridient ingredient,int mealId)
        {
            MealIngredientsDataEntity mealIngredient= new MealIngredientsDataEntity();
            mealIngredient.IngredientId = ingredient.Id;
            mealIngredient.MealId = mealId;
            mealIngredient.Quantity= ingredient.Quantity;
            mealIngredient.QuantityUnit = Convert.ToString(ingredient.QuantityUnit);
            return mealIngredient;
        }
        public static IngredientsDataEntity ConvertIngredientsToIngredientsDataEntity(UserMeal.Ingridient ingredient)
        {
            IngredientsDataEntity ingredientEntity = new IngredientsDataEntity();
            ingredientEntity.Name = ingredient.Name;
            ingredientEntity.CaloriesPer100Gram = ingredient.CaloriesPer100g;
            //TODO add other mappings like sugar, proetin etc.
            return ingredientEntity;
        }

        public static UserMeal ConvertUserMealDataEntityToUserMealModel(MealsDataEntity mealData)
        {
            UserMeal userMeal = new UserMeal();
            userMeal.TimeOfDayMeal = new();
            userMeal.Name = mealData.Name;
            userMeal.Description = mealData.Description;
            userMeal.CookingTime = mealData.CookingTime;
            userMeal.PrepTime = mealData.PrepTime;
            userMeal.ServingAmount = mealData.ServingsAmount;
            userMeal.Instructions = mealData.Instructions;
            if (mealData.IsBreakfast) 
                userMeal.TimeOfDayMeal.Add(TimeOfDayMeal.Breakfast);
            if (mealData.IsDinner)
                userMeal.TimeOfDayMeal.Add(TimeOfDayMeal.Dinner);
            if (mealData.IsLunch)
                userMeal.TimeOfDayMeal.Add(TimeOfDayMeal.Lunch);
            if (mealData.IsSnack)
                userMeal.TimeOfDayMeal.Add(TimeOfDayMeal.Snack);
            if (mealData.IsDesert)
                userMeal.TimeOfDayMeal.Add(TimeOfDayMeal.Dessert);

            foreach (var item in mealData.RecipeImages) {
                userMeal.Images.Add(item.ImageData);
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
            userIngre.Quantity = ingredientDO.Quantity;
            QuantityUnit res;
            Enum.TryParse(ingredientDO.QuantityUnit,out res);
            userIngre.QuantityUnit = res;
            userIngre.Name = ingredientDO.Ingredients.Name;
            userIngre.CaloriesPer100g = ingredientDO.Ingredients.CaloriesPer100Gram;
            return userIngre;
        }

    }
}
