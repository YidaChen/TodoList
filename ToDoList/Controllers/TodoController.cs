using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    [Produces("application/json")]
    [Route("api/Todo")]
    [Authorize]
    public class TodoController : Controller
    {
        private readonly todolistContext _context;

        public TodoController(todolistContext context)
        {
            _context = context;
        }

        // GET: api/Todo
        [HttpGet]
        public IEnumerable<Todo> GetAllByLoginUserId()
        {
            int userId = Int32.Parse(User.FindFirst("Id").Value);
            
            return _context.Todo.Where(m => m.UserId == userId).ToList();
        }

        // GET: api/Todo/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodo([FromRoute] uint id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var todo = await _context.Todo.SingleOrDefaultAsync(m => m.Id == id);

            if (todo == null)
            {
                return NotFound();
            }

            return Ok(todo);
        }

        // PUT: api/Todo/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodo([FromRoute] uint id, [FromBody] Todo todo)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != todo.Id)
                return BadRequest();

            _context.Entry(todo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoExists(id))
                    return NotFound();
                else
                    throw;
            }
            return NoContent();
        }

        // POST: api/Todo
        [HttpPost]
        public async Task<IActionResult> PostTodo([FromBody] Todo todo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            todo.UserId = Convert.ToUInt32(User.FindFirst("Id").Value);
            _context.Todo.Add(todo);
            await _context.SaveChangesAsync();

            return Ok(todo);
        }

        // DELETE: api/Todo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo([FromRoute] uint id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var todo = await _context.Todo.SingleOrDefaultAsync(m => m.Id == id);
            if (todo == null)
                return NotFound();

            _context.Todo.Remove(todo);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool TodoExists(uint id)
        {
            return _context.Todo.Any(e => e.Id == id);
        }
    }
}