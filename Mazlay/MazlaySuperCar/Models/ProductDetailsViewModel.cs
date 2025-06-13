using Application.DTOs;         
using Domain.Entities;          

namespace MazlaySuperCar.Models;

public class ProductDetailsViewModel
{
    public Product Product { get; init; } = null!;  // сам товар
    public int?    PrevId  { get; init; }           // <- предыдущий
    public int?    NextId  { get; init; }           // следующий ->
}