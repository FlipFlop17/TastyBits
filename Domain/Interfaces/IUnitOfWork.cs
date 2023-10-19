namespace Domain.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        void Commit();
        IIngredientsRepository IngredientsRepository { get; }
        IMealIngredientsRepository MealIngredientsRepository { get; set; }
        IMealmageRepository MealImageRepository { get; set; }
        IMealsRepository MealsRepository { get; }
    }
}
