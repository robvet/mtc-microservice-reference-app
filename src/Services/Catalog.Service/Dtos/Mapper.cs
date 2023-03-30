using System.Collections.Generic;
using System.Linq;
using Catalog.API.Domain.Entities;

namespace Catalog.API.Dtos
{
    public class Mapper
    {
        public static IEnumerable<ProductDto> MapToMusicDto(IEnumerable<Product> music)
        {
            var mappedDtos = new List<ProductDto>();

            foreach (var item in music)
                mappedDtos.Add(new ProductDto
                {
                    Id = item.Id,
                    GenreName = item.Genre?.Name,
                    Title = item.Title,
                    ArtistName = item.Artist?.Name,
                    Price = item.Price,
                    ParentalCaution = item.ParentalCaution,
                    Upc = item.Upc,
                    Cutout = item.Cutout,
                    ReleaseDate = item.ReleaseDate
                });

            return mappedDtos;
        }

        public static ProductDto MapToMusicDto(Product music)
        {
            return new ProductDto
            {
                Id = music.Id,
                GenreName = music.Genre?.Name,
                Title = music.Title,
                ArtistName = music.Artist?.Name,
                Price = music.Price,
                ParentalCaution = music.ParentalCaution,
                Upc = music.Upc,
                Cutout = music.Cutout,
                ReleaseDate = music.ReleaseDate,
                ArtistId = music.ArtistId,
                GenreId = music.GenreId
            };
        }


        public static IEnumerable<GenreDto> MapToGenreDto(IEnumerable<Genre> genres)
        {
            var mappedDtos = new List<GenreDto>();

            foreach (var item in genres)
                mappedDtos.Add(new GenreDto
                {
                    Name = item.Name,
                    Description = item.Description,
                    GenreId = item.GenreId,
                    Albums = item.Albums == null || item.Albums.Count == 0 ? null : MapToMusicDto(item.Albums).ToList()
                });

            return mappedDtos;
        }

        public static GenreDto MapToGenreDto(Genre genre)
        {
            return new GenreDto
            {
                Name = genre.Name,
                Description = genre.Description,
                GenreId = genre.GenreId,
                Albums = genre.Albums == null || genre.Albums.Count == 0 ? null : MapToMusicDto(genre.Albums).ToList()
            };
        }

        public static IEnumerable<ArtistDto> MapToArtistDto(IEnumerable<Artist> artist)
        {
            var mappedDtos = new List<ArtistDto>();

            foreach (var item in artist)
                mappedDtos.Add(new ArtistDto
                {
                    ArtistId = item.ArtistId,
                    Name = item.Name
                });

            return mappedDtos;
        }
    }
}