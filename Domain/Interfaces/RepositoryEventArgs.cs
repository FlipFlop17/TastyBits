namespace Domain.Interfaces
{
    public class RepositoryEventArgs:EventArgs
    {
        public ChangeType ChangeType { get; set; }
    }

    public enum ChangeType
    {
        Add,
        Update,
        Delete
    }
}
