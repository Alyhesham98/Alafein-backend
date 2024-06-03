namespace Core.DTOs.LookUps.Category.Response
{
    public class ListCategoryDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public bool IsPublished { get; set; }
        public int SortNo { get; set; }
    }
}
