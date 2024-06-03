namespace Core.DTOs.LookUps.Category.Request
{
    public class AddCategoryDto
    {
        public string Name { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public bool IsPublished { get; set; }
        public int SortNo { get; set; }
    }
}
