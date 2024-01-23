using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MovieSearchAPI.model.Dto;
using MovieSearchAPI.model.Service;
using MovieSearchAPI.model;
using Newtonsoft.Json;

public class MovieService : IMovieService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly AppSettings _appSettings;
    private static List<string> SearchHistory = new List<string>();
    public MovieService(IHttpClientFactory httpClientFactory, IOptions<AppSettings> appSettings)
    {
        _httpClientFactory = httpClientFactory;
        _appSettings = appSettings.Value;
    }


    public async Task<MovieSearchDTO> SearchMovieAsync(string title)
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetStringAsync($"http://www.omdbapi.com/?apikey={_appSettings.ApiKey}&t={title}");
            var result = JsonConvert.DeserializeObject<dynamic>(response);

            if (result != null && result.Title != null)
            {
                var searchResult = new MovieSearchModel
                {
                    Title = (string)result.Title,
                    Year = (string)result.Year,
                    ImdbID = (string)result.imdbID,
                    // Add other properties as needed
                };

                return new MovieSearchDTO { SearchResult = searchResult, SearchHistory = GetUpdatedSearchHistory(title) };
            }
            else
            {
                return new MovieSearchDTO { ErrorMessage = "No search results found." };
            }
        }
        catch (Exception ex)
        {
            return new MovieSearchDTO { ErrorMessage = ex.Message };
        }
    }

    public async Task<MovieDetailsDTO> GetMovieDetailsAsync(string imdbId)
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetStringAsync($"http://www.omdbapi.com/?apikey={_appSettings.ApiKey}&i={imdbId}");
            var result = JsonConvert.DeserializeObject<dynamic>(response);

            var ratingsArray = (result.Ratings as Newtonsoft.Json.Linq.JArray);
            var ratings = ratingsArray != null
                ? ratingsArray.Select(r => new { RSource = (string)r["Source"], Value = (string)r["Value"] }).ToList<object>()
                : new List<object>();

            var detailedInfo = new MovieDetailsModel
            {
                Title = (string)result.Title,
                Year = (string)result.Year,
                Rated = (string)result.Rated,
                Released = (string)result.Released,
                Runtime = (string)result.Runtime,
                Genre = (string)result.Genre,
                Director = (string)result.Director,
                Writer = (string)result.Writer,
                Actors = (string)result.Actors,
                Plot = (string)result.Plot,
                Language = (string)result.Language,
                Country = (string)result.Country,
                Awards = (string)result.Awards,
                Poster = (string)result.Poster,
                Ratings = ratings,
                Metascore = (string)result.Metascore,
                ImdbRating = (string)result.imdbRating,
                ImdbVotes = (string)result.imdbVotes,
                ImdbID = (string)result.imdbID,
                Type = (string)result.Type,
                DVD = (string)result.DVD,
                BoxOffice = (string)result.BoxOffice,
                Production = (string)result.Production,
                Website = (string)result.Website,
                Response = (string)result.Response
            };

            return new MovieDetailsDTO { DetailedInfo = detailedInfo };
        }
        catch (Exception ex)
        {
            return new MovieDetailsDTO { ErrorMessage = ex.Message };
        }
    }

    private List<SearchHistoryModel> GetUpdatedSearchHistory(string searchTerm)
    {
        SearchHistory.Insert(0, searchTerm);
        return SearchHistory.Take(5).Select(history => new SearchHistoryModel { SearchTerm = history }).ToList();
    }
}
