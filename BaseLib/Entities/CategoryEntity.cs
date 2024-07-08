using BlazexpIO.Utils.Enums;

namespace BaseLib.Entities
{
    public class CategoryEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public CategoryType CategoryType { get; set; }
        public bool IsExpense { get; set; }
    }
}
