using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderServices.Dtos
{
public class ItemResponse
{
    public int ItemId { get; set; }
    public string ItemName { get; set; }
    public string ItemDescription { get; set; }
    public decimal OriginalPrice { get; set; }
    public int CategoryId { get; set; }
    public CategoryResponse Category { get; set; }
    public List<ImageResponse> Images { get; set; }
    public bool IsCharged { get; set; }
    public bool IsLocked { get; set; }
    public List<IngredientResponse> IngredientList { get; set; }
}

public class CategoryResponse
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
}

public class ImageResponse
{
    public int ImageId { get; set; }
    public string ImageUrl { get; set; }
}

public class IngredientResponse
{
    public int IngredientId { get; set; }
    public string IngredientName { get; set; }
    public int MaxQuantity { get; set; }
    public int MinQuantity { get; set; }
}
}