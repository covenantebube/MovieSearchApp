namespace MovieSearchAPI.model.Dto
{
    public class MovieSearchDTO
    {
        

        public MovieSearchModel SearchResult { get; set; }
        public List<SearchHistoryModel> SearchHistory { get; set; }
        public string ErrorMessage { get; internal set; }
    }
}
