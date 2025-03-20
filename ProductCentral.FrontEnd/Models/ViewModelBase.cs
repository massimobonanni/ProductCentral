namespace ProductCentral.FrontEnd.Models
{
    public abstract class ViewModelBase
    {
        public bool HasErrror { get; set; } = false;
        public string ErrorMessage { get; set; }
    }
}
