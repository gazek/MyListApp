using MyListApp.Api.Data.Entities;
using MyListApp.Api.Services;
using System.Collections.Generic;
using System.Web.Http;

namespace MyListApp.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/Invitations")]
    public class InvitationController : ApiController
    {
        private InvitationRepository _repo { get; set; }
        private ListAuthChecker _auth { get; set; }

        public InvitationController()
        {
            _repo = new InvitationRepository();
            _repo.User = User.Identity;
            _auth = new ListAuthChecker();
            _auth.User = User.Identity;
        }

        // GET api/<controller>/ToMe
        [Route("ToMe")]
        public IHttpActionResult GetToMe()
        {
            IEnumerable<InvitationModel> result = _repo.Get("InviteeId");
            return Ok(result);
        }

        // GET api/<controller>/FromMe
        [Route("FromMe")]
        public IHttpActionResult GetFromMe()
        {
            return Ok(_repo.Get("InvitorId"));
        }

        // GET api/<controller>/5
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            if (!(_auth.IsInvitationReceiverByInvitationId(id) || _auth.IsInvitationSenderByInvitationId(id)))
            {
                return Unauthorized();
            }

            InvitationModel result = _repo.Get(id);

            if (result == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(result);
            }

        }

        // POST api/<controller>
        [Route("")]
        public IHttpActionResult Post([FromBody]InvitationModel item)
        {
            if (!_auth.IsListOwnerByListId(item.ListId))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            InvitationModel result = _repo.Add(item);

            if (item == null)
            {
                return InternalServerError();
            }
            else
            {
                return Ok(result);
            }
        }

        // PUT api/<controller>/5
        [Route("{id}")]
        public IHttpActionResult Put(int id, [FromBody]InvitationModel item)
        {
            if (!_auth.IsInvitationReceiverByInvitationId(id))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_repo.Update(id, item))
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
        public IHttpActionResult Delete(int id)
        {
            if (_auth.IsInvitationSenderByInvitationId(id))
            {
                _repo.Delete(id);
                return Ok();
            }

            return Unauthorized();
        }
    }
}