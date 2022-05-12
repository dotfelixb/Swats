using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Keis.Data.Repository.EmailManager.Interfaces;
using Keis.Model.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Keis.Web.Controllers
{
    public class EmailController : MethodController
    {
        private readonly IEmailRepository _emailRepository;

        public EmailController(IEmailRepository emailRepository)
        {
            _emailRepository = emailRepository;
        }
        // GET: email.list
        [AllowAnonymous]
        [HttpGet("emails.list")]
        public async Task<ActionResult<IEnumerable<EmailMessage>>> GetMails()
        {
            var emails = await _emailRepository.GetEmails();
            return Ok(emails);
        }

    }
}

