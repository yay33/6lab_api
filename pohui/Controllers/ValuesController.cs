using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pohui.DB;
using pohui.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace pohui.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly appDBcontext dbcontext;
        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoItem>>> GetTodoItems()
        {
            if (dbcontext.ToDoTable == null)
            {
                return NotFound();
            }
            return await dbcontext.ToDoTable.ToListAsync();
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<ActionResult<ToDoItem>> PostTodoItem(ToDoItem todoItem)
        {
            if (dbcontext.ToDoTable == null)
            {
                return Problem("Entity set 'AppDbContext.TodoItems'  is null.");
            }
            dbcontext.ToDoTable.Add(todoItem);
            await dbcontext.SaveChangesAsync();

            return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
        }

        private bool ToDoItemExists(long id)
        {
            return (dbcontext.ToDoTable?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // PUT api/<ValuesController>/
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, ToDoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            dbcontext.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await dbcontext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToDoItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            if (dbcontext.ToDoTable == null)
            {
                return NotFound();
            }
            var todoItem = await dbcontext.ToDoTable.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            dbcontext.ToDoTable.Remove(todoItem);
            await dbcontext.SaveChangesAsync();

            return NoContent();
        }
        public ValuesController(appDBcontext bcontext)
        {
            dbcontext = bcontext;
        }
    }
}
