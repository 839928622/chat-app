namespace Application.Features.Photo.Queries.QueryUserPhotos
{
    public class PhotoDto
    {
        public int Id { get; set; }
        public string Url { get; set; } = null!;
        public bool IsMain { get; set; }
    }
}
