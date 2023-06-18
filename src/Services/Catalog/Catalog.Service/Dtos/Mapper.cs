using System.Collections.Generic;
using System.Linq;
using catalog.service.Domain.Entities;

namespace catalog.service.Dtos
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
                    Title = item.Title,
                    Price = item.Price,
                    ParentalCaution = item.ParentalCaution,
                    Upc = item.Upc,
                    AlbumArtUrl = item.AlbumArtUrl,
                    ProductId = item.ProductId,
                    Single = item.Single,
                    ReleaseYear = item.ReleaseYear,
                    HighValueItem = item.HighValueItem,
                    IsActive = item.IsActive,

                    GenreName = item.Genre?.Name,
                    GenreId = item.GenreId,

                    ArtistName = item.Artist?.Name,
                    ArtistId = item.ArtistId,

                    MediumName = item.Medium?.Name,
                    MediumId = item.MediumId,

                    StatusName = item.Status?.Name,
                    StatusId = item.StatusId,

                    ConditionName = item.Condition?.Name,
                    ConditionId = item.ConditionId
                });

            return mappedDtos;
        }

        public static ProductDto MapToMusicDto(Product music)
        {
            return new ProductDto
            {
                Id = music.Id,
                Title = music.Title,
                Price = music.Price,
                ParentalCaution = music.ParentalCaution,
                Upc = music.Upc,
                AlbumArtUrl = music.AlbumArtUrl,
                ProductId = music.ProductId,
                Single = music.Single,
                ReleaseYear = music.ReleaseYear,
                HighValueItem = music.HighValueItem,
                IsActive = music.IsActive,

                GenreName = music.Genre?.Name,
                GenreId = music.GenreId,

                ArtistName = music.Artist?.Name,
                ArtistId = music.ArtistId,

                MediumName = music.Medium?.Name,
                MediumId = music.MediumId,

                StatusName = music.Status?.Name,
                StatusId = music.StatusId,

                ConditionName = music.Condition?.Name,
                ConditionId = music.ConditionId
            };
        }


        public static IEnumerable<GenreDto> MapToGenreDto(IEnumerable<Genre> genres)
        {
            var mappedDtos = new List<GenreDto>();

            foreach (var item in genres)
                mappedDtos.Add(new GenreDto
                {
                    Name = item.Name,
                    GuidId = item.GuidId,
                    Description = item.Name,
                    GenreId = item.GenreId,
                    Albums = item.Products == null || item.Products.Count == 0 ? null : MapToMusicDto(item.Products).ToList()
                });

            return mappedDtos;
        }

        public static GenreDto MapToGenreDto(Genre genre)
        {
            return new GenreDto
            {
                Name = genre.Name,
                GuidId = genre.GuidId,
                Description = genre.Name,
                GenreId = genre.GenreId,
                Albums = genre.Products == null || genre.Products.Count == 0 ? null : MapToMusicDto(genre.Products).ToList()
            };
        }

        public static IEnumerable<ArtistDto> MapToArtistDto(IEnumerable<Artist> artist)
        {
            var mappedDtos = new List<ArtistDto>();

            foreach (var item in artist)
                mappedDtos.Add(new ArtistDto
                {
                    GuidId = item.GuidId,
                    ArtistId = item.ArtistId,
                    Name = item.Name
                });

            return mappedDtos;
        }
    }
}