using AutoMapper;
using BookStore_API.Data;
using BookStore_API.DTOs;
using BookStore_API.Models;

namespace BookStore_API.Mappings
{
	public class Maps : Profile
	{
		public Maps()
		{
			CreateMap<Book, BookDto>().ReverseMap();
			CreateMap<Rental, RentalDto>().ReverseMap();
			CreateMap<Rental, RentalCreateDto>().ReverseMap();
			CreateMap<Rental, RentalUpdateDto>().ReverseMap();
		}
	}
}
