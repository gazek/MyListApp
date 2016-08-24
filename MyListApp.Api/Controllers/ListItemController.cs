using Microsoft.AspNet.Identity;
using MyListApp.Api.Data.Entities;
using MyListApp.Api.Services;
using System.Web.Http;

namespace MyListApp.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/Items")]
    public class ListItemController : ApiController
    {
        private ListItemRepository _repo { get; set; }
        private ListAuthChecker _auth { get; set; }

        public ListItemController()
        {
            _repo = new ListItemRepository(User.Identity);
            _auth = new ListAuthChecker(User.Identity);
        }

        // GET api/<controller>/5
        [Route("{id}")]
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            if (!_auth.HasListAccessByItemId(id))
            {
                return Unauthorized();
            }

            ListItemModel result = _repo.Get(id);

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
        public IHttpActionResult Post([FromBody]ListItemModel item)
        {
            // verify user has write access to list
            if (!_auth.HasListAccessByListId(item.ListId))
            {
                return Unauthorized();
            }

            // set creatorId to current userId to prevent user from creating under a different userId
            item.CreatorId = User.Identity.GetUserId();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // add item
            ListItemModel result = _repo.Add(item);

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
        public IHttpActionResult Put(int id, [FromBody]ListItemModel item)
        {
            // verify authorization to edit record
            if (!_auth.HasListAccessByItemId(id))
            {
                return Unauthorized();
            }

            // verify model state
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Add list
            bool result = _repo.Update(id, item);

            // send result
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
            // verify authorization to delete record
            if (!_auth.HasListAccessByItemId(id))
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
                return NotFound();
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