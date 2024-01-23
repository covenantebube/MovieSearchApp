using MovieSearchAPI.model.Dto;

namespace MovieSearchAPI.model.Service
{
    public interface IMovieService
    {
        Task<MovieSearchDTO> SearchMovieAsync(string title);
        Task<MovieDetailsDTO> GetMovieDetailsAsync(string imdbId);
    }

}