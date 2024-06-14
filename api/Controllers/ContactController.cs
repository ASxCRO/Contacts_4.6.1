﻿using axians.contacts.services.Models;
using axians.contacts.services.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Web.Http;

namespace api.Controllers
{
    public class ContactController : ApiController
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService ?? throw new ArgumentNullException(nameof(contactService));
        }

        [HttpPost]
        public IHttpActionResult AddContact([FromBody] Contact contact)
        {
            if (contact == null)
            {
                return BadRequest("Contact object is null");
            }

            int contactId = _contactService.AddContact(contact);
            return Ok(contactId);
        }

        [HttpGet]
        [Authorize]
        public IHttpActionResult GetAllContacts([FromUri]GetAllContactsRequest model)
        {
            var data =  _contactService.FindAllContacts(model);
            return Ok(data);
        }

        [HttpGet]
        public IHttpActionResult GetContactById(int id)
        {
            Contact contact = _contactService.FindContactByID(id);
            if (contact == null)
            {
                return NotFound();
            }

            return Ok(contact);
        }

        [HttpPut]
        public IHttpActionResult UpdateContact(int id, [FromBody] Contact contact)
        {
            if (contact == null || id != contact.Id)
            {
                return BadRequest();
            }

            _contactService.UpdateContact(contact);
            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult DeleteContact(int id)
        {
            _contactService.RemoveContact(id);
            return Ok();
        }
    }
}