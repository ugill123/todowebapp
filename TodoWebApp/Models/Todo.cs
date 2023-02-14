namespace TodoWebApp.Models
{
    public class Todo
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool Iscompleted { get; set; }

        public override string? ToString()
        {
            return base.ToString();
        }
    }
}
