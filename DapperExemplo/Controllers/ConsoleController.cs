using Dapper;
using Microsoft.AspNetCore.Mvc;
using Slapper;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DapperExemplo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsoleController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ConsoleController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        
        [HttpGet("{id}")]
        public ActionResult<Model.Console> Get(int id)
        {
            var sqlScript = string.Format(
                              @"SELECT a.[IdConsole]
                                     , a.[NomeConsole]
                                     , b.[IdItem]
                                    ,  b.[IdConsole]
                                    ,  b.[DescricaoItem]
                                    ,  b.[TipoItem]
                                    
                              FROM [Dapper].[dbo].[Console] a
                              inner join[Dapper].[dbo].[ItemConsole] b on b.IdConsole = a.IdConsole
                            where a.idConsole = {0}", id);

            using IDbConnection db = new SqlConnection(_configuration.GetConnectionString("Dapper"));


            var teste = new Dictionary<int, Model.Console>();
            var a = db.Query<Model.Console, Model.ItemConsole, Model.Console>
                (
                    sql: sqlScript, 
                    map: (con, it) => 
                    {
                        Model.Console c;
                        if (!teste.TryGetValue(con.IdConsole, out c))
                        {
                            teste.Add(con.IdConsole,c = con);
                        }

                        if (c.ItensConsole == null)
                            c.ItensConsole = new List<Model.ItemConsole>();

                        c.ItensConsole.Add(it);
                        return con;
                    },
                    splitOn:"IdItem"
                 ).FirstOrDefault();

            return a;

        }

        // POST api/<PedidoController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<PedidoController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PedidoController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
