using Microsoft.AspNet.Identity;
using MyListApp.Api.Data.Entities;
using MyListApp.Api.Services;
using System.Security.Principal;
using System.Web.Http;

namespace MyListApp.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/Lists")]
    public class ListsController : ApiController
    {
        private ListRepository _repo { get; set; }

        public ListsController()
        {
            _repo = new ListRepository(User.Identity);
        }

        // GET api/<controller>
        [Route("")]
        [HttpGet]
        public IHttpActionResult Get()
        {

            return Ok(_repo.Get());
        }

        // GET api/<controller>/5
        [Route("{id}")]
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            ListModel result = _repo.Get(id);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

        // POST api/<controller>
        [Route("")]
        [HttpPost]
        public IHttpActionResult Post([FromBody]ListModel list)
        {
            list.OwnerId = User.Identity.GetUserId();

            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid list model.");
            }

            ListModel result = _repo.Add(list);
            return Ok(result);
        }

        // PUT api/<controller>/5
        [Route("{id}")]
        [HttpPut]
        public IHttpActionResult Put(int id, [FromBody]ListModel list)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid list model.");
            }
            bool result = _repo.Update(id, list);
            if (result)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        // DELETE api/<controller>/5
        [Route("{id}")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            bool result = _repo.Delete(id);
            if (result)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}