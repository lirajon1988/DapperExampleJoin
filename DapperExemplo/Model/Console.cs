namespace DapperExemplo.Model
{
    public class Console
    {
        public Console()
        {
            this.ItensConsole = new List<ItemConsole>();
        }
        public int IdConsole { get; set; }
        public string? NomeConsole { get; set; }
        public List<ItemConsole>? ItensConsole { get; set; }
    }
}
