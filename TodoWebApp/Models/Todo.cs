using System.ComponentModel.DataAnnotations;

namespace TodoWebApp.Models
{
    public class Todo
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public bool Iscompleted { get; set; }

        public override string? ToString()
        {
            return base.ToString();
        }
    }
}
