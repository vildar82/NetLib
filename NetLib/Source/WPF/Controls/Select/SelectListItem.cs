namespace NetLib.WPF.Controls.Select
{
    using JetBrains.Annotations;

    [PublicAPI]
    public class SelectListItem<T> : BaseModel
    {
        public string Name { get; set; }
        public T Object { get; set; }
        public bool IsSelected { get; set; }

        public SelectListItem(string name, T obj)
        {
            Name = name;
            Object = obj;
        }
    }
}