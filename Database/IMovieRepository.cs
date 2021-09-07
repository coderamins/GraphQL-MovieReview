using System;
using System.Threading.Tasks;
using MovieReview.Model;

namespace MovieReviews.Database
{
    public interface IMovieRepository
    {
        Task<Movie> GetMovieByIdAsync(Guid id);
        Task<Movie> AddReviewToMovieAsync(Guid id,Review review);
    }
}