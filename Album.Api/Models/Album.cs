using System.ComponentModel.DataAnnotations;

namespace Album.Api.Models;

public class Album
{
    [Key]
    public Guid Id { get; set; }

    [Required] 
    [MaxLength(200)] 
    public string Name { get; set; } = null!;

    [Required] 
    [MaxLength(200)] 
    public string Artist { get; set; } = null!;

    [Required] 
    [MaxLength(500)] 
    public string ImageUrl { get; set; } = null!;
}
