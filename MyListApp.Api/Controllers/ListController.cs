using Microsoft.AspNet.Identity;
using MyListApp.Api.Data.Entities;
using MyListApp.Api.Services;
using System.Web.Http;

namespace MyListApp.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/Lists")]
    public class ListController : ApiController
    {
        private ListRepository _repo { get; set; }
        private ListAuthChecker _auth { get; set; }

        public ListController()
        {
            _repo = new ListRepository(User.Identity);
            _auth = new ListAuthChecker(User.Identity);
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
            if (!_auth.HasListAccessByListId(id))
            {
                return Unauthorized();
            }

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
            // set list ownerId to userId of the current user
            list.OwnerId = User.Identity.GetUserId();

            // verify model state
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // add the list
            ListModel result = _repo.Add(list);

            // return the list to provide the Id of the newly created list
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return InternalServerError();
            }
        }

        // PUT api/<controller>/5
        [Route("{id}")]
        [HttpPut]
        public IHttpActionResult Put(int id, [FromBody]ListModel list)
        {
            // verify authorization to edit record
            if (!_auth.HasListAccessByListId(id))
            {
                return Unauthorized();
            }

            // verify model state
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Add list
            bool result = _repo.Update(id, list);

            // send result
            if (result)
            {
                return Ok();
            }
            else
            {
                return InternalServerError();
            }
        }

        // DELETE api/<controller>/5
        [Route("{id}")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            // verify authorization to delete record
            if (!_auth.HasListAccessByListId(id))
            {
                return Unauthorized();
            }

            // delete list
            bool result = _repo.Delete(id);

            // return result
            // send result
            if (result)
            {
                return Ok();
            }
            else
            {
                return InternalServerError();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repo.Dispose();
                _auth.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}