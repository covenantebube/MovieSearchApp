namespace MovieSearchAPI.model.Dto
{
    public class MovieDetailsDTO
    {
        public MovieDetailsModel DetailedInfo { get; set; }
        public string ErrorMessage { get; internal set; }
    }
}
