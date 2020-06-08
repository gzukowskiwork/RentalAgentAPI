using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using DinkToPdf;
using DinkToPdf.Contracts;
using Entities.DataTransferObjects.Invoice;
using Entities.Models;
using InvoiceGeneratorService.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleFakeRent.Extensions;
using System.Runtime.InteropServices;
using EmailService;
using System.Collections;
using Microsoft.AspNetCore.Authorization;

namespace SimpleFakeRent.Controllers
{
    [Route("api/invoice")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        private readonly IConverter _converter;
        private readonly IEmailEmmiter _emailEmmiter;

        public InvoiceController(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerManager logger, IConverter converter, IEmailEmmiter emailEmmiter)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
            _converter = converter;
            _emailEmmiter = emailEmmiter;
        }

        //GET api/invoice
        [HttpGet]
        public async Task<IActionResult> GetAllInvoices()
        {
            try
            {
                var invoices = await _repositoryWrapper.Invoice.GetAllInvoices();
                if (invoices == null)
                {
                    _logger.LogInfo($"No 'Invoice' has been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned all 'Invoices' from db, result count: {invoices.Count()}.");
                    var invoicesResult = _mapper.Map<IEnumerable<InvoiceDto>>(invoices);
                    return Ok(invoicesResult);
                }
            }
            catch(Exception e)
            {
                _logger.LogError($"Something went wrong inside GetAllInvoices() action: {e.Message}");
                _logger.LogError(e.ErrorMessageExtension());
                return StatusCode(500, e.Message);
            }
        }

        [Authorize(Policy = "Landlord,Tenant")]
        //GET api/invoice/{invoice_id}
        [HttpGet("{id}", Name ="InvoiceById")]
        public async Task<IActionResult>GetInvoiceByInvoiceId(int id)
        {
            try
            {
                var invoice = await _repositoryWrapper.Invoice.GetInvoiceById(id);

                if (invoice == null)
                {
                    _logger.LogError($"Invoice with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Inovice by invoice_id: {id}");
                    var invoiceResult = _mapper.Map<InvoiceDto>(invoice);
                    return Ok(invoiceResult);
                }
            }
            catch(Exception e)
            {
                _logger.LogError($"Something went wrong inside GetInvoiceById(invoice_id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/invoice/state/{state_id}
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("state/{id}", Name = "InvoiceByStateId")]
        public async Task<IActionResult> GetInvoiceByStateId(int id)
        {
            try
            {
                var invoice = await _repositoryWrapper.Invoice.GetInvoiceByStateId(id);

                if (invoice == null)
                {
                    _logger.LogError($"Invoice with StateId: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Inovice by state_id: {id}");
                    var invoiceResult = _mapper.Map<InvoiceDto>(invoice);
                    return Ok(invoiceResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetInvoiceByStateId(state_id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/invoice/rent/{id}
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("rent/{id}")]
        public async Task<IActionResult> GetInvoicesByRentId(int id)
        {
            try
            {
                var invoices = await _repositoryWrapper.Invoice.GetInvoicesByRentId(id);

                if (invoices == null)
                {
                    _logger.LogError($"Invoices with rent_id: {id}, haven't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Invoices with rent_id: {id}, result count: {invoices.Count()}.");
                    var invoicesResult = _mapper.Map<IEnumerable<InvoiceDto>>(invoices);
                    return Ok(invoicesResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetInvoicesByRentId(rent_id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/invoice/rent/{id}
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("rent/{id}/state")]
        public async Task<IActionResult> GetInvoicesWithStateByRentId(int id)
        {
            try
            {
                var invoices = await _repositoryWrapper.Invoice.GetInvoicesWithStateByRentId(id);

                if (invoices == null)
                {
                    _logger.LogError($"Invoices with rent_id: {id}, haven't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Invoices by rent_id: {id} with related state, result count: {invoices.Count()}.");
                    var invoicesResult = _mapper.Map<IEnumerable<InvoiceWithStateDto>>(invoices);
                    return Ok(invoicesResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetInvoicesByRentId(rent_id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/invoice/{id}/rent
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("{id}/rent", Name = "InvoiceWithRentByInvoiceId")]
        public async Task<IActionResult> GetInvoiceWithRentByInvoiceId(int id)
        {
            try
            {
                var invoice = await _repositoryWrapper.Invoice.GetInvoiceWithRentByInvoiceId(id);

                if (invoice == null)
                {
                    _logger.LogError($"Invoice with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Invoice with invoice_id: {id} and related Rent.");
                    var invoiceResult = _mapper.Map<InvoiceWithRentDto>(invoice);
                    return Ok(invoiceResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetInvoiceWithRentByInvoiceId(invoice_Id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/invoice/{id}/state/rent/property/address
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("{id}/state/rent/property/address", Name = "InvoiceWithStateRentAndPropertyAndAddressByInvoiceId")]
        public async Task<IActionResult> GetWithStateRentAndPropertyAndAddressByInvoiceId(int id)
        {
            try
            {
                var invoice = await _repositoryWrapper.Invoice.GetInvoiceWithStateRentAndPropertyAndAddressByInvoiceId(id);

                if (invoice == null)
                {
                    _logger.LogError($"Invoice with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Invoice with invoice_id: {id} and related Rent.");
                    var invoiceResult = _mapper.Map<InvoiceWithRentStateTenantPropertyWithAddressLimitedDto>(invoice);
                    return Ok(invoiceResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetInvoiceWithRentByInvoiceId(invoice_Id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/invoice/{id}/state
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("{id}/state", Name = "InvoiceWithStateByInvoiceId")]
        public async Task<IActionResult> GetInvoiceWithStateByInvoiceId(int id)
        {
            try
            {
                var invoice = await _repositoryWrapper.Invoice.GetInvoiceWithStateByInvoiceId(id);

                if (invoice == null)
                {
                    _logger.LogError($"Invoice with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Invoice with id: {id} and related State.");
                    var invoiceResult = _mapper.Map<InvoiceWithStateDto>(invoice);
                    return Ok(invoiceResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetInvoiceWithStateByInvoiceId(invoice_Id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //POST api/invoice
        [HttpPost]
        public async Task<IActionResult> CreateInvoice([FromBody] InvoiceForCreationDto invoice)
        {
            try
            {
                if (invoice == null)
                {
                    _logger.LogError("Invoice received is a Null Object.");
                    return BadRequest("Invoice object is null. Please send full request.");
                }
                else if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid Invoice object sent from client.");
                    return BadRequest("Invoice object is not Valid");
                }
                
                Boolean rentExist = await _repositoryWrapper.Rent.CheckIfRentExistByRentId(invoice.RentId);
                if (!rentExist)
                {
                    _logger.LogError($"Invoice cannot be created, because Rent with id:{invoice.RentId} does not exist in DB.");
                    return BadRequest($"Not created. Rent wiht id: {invoice.RentId} does not exist.");
                }

                Boolean stateHasInvoice = await _repositoryWrapper.Invoice.CheckIfInvoiceExistByStateId(invoice.StateId);
                if (stateHasInvoice)
                {
                    _logger.LogError($"Invoice cannot be created, because State with id:{invoice.StateId} already exist.");
                    return BadRequest($"Not created. Invoice for this State_id: {invoice.StateId} already exist");
                }

                var invoiceEntity = _mapper.Map<Invoice>(invoice);

                _repositoryWrapper.Invoice.CreateInvoice(invoiceEntity);
                await _repositoryWrapper.Save();

                var createdInvoice = _mapper.Map<InvoiceDto>(invoiceEntity);

                /* Get generatedInvoice and update previously created invoice in this process. */
                var pdfFileToInclude = await GetGeneratedPDF(createdInvoice.Id);
                createdInvoice.InvoiceDocument = pdfFileToInclude.getInvoiceBytes();
                createdInvoice.FileName = pdfFileToInclude.getInvoiceFileName();
                /* map created invoice to InvoiceForUpdateDto */
                invoiceEntity.InvoiceDocument = pdfFileToInclude.getInvoiceBytes();
                invoiceEntity.FileName = pdfFileToInclude.getInvoiceFileName();
                _repositoryWrapper.Invoice.UpdateInvoice(invoiceEntity);
                await _repositoryWrapper.Save();
                /* Modofication end 03.06. */

                return CreatedAtRoute("InvoiceById", new { id = createdInvoice.Id }, createdInvoice);
            }
            catch(Exception e)
            {
                _logger.LogError($"Something went wrong inside CreateInvoice() action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //PUT api/invoice
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInvoice(int id, [FromBody]InvoiceForUpdateDto invoice)
        {
            try
            {
                if (invoice == null)
                {
                    _logger.LogError("Invoice received is a Null Object.");
                    return BadRequest("Invoice object is null. Please send full request.");
                }
                else if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid Invoice object sent from client.");
                    return BadRequest("Invoice object is not Valid");
                }

                Boolean rentExist = await _repositoryWrapper.Rent.CheckIfRentExistByRentId(invoice.RentId);
                if (!rentExist)
                {
                    _logger.LogError($"Invoice cannot be updated, because Rent with id:{invoice.RentId} does not exist in DB.");
                    return BadRequest($"Not updated. Rent wiht id: {invoice.RentId} does not exist.");
                }

                Boolean stateHasInvoice = await _repositoryWrapper.Invoice.CheckIfInvoiceExistByStateId(invoice.StateId);
                if (!stateHasInvoice)
                {
                    _logger.LogError($"Invoice with id: {id}, hasn't been found in db.");
                    return NotFound($"Not updated.Invoice with id: {id} not found in DB.");
                }

                var invoiceEntity = await _repositoryWrapper.Invoice.GetInvoiceById(id);
                if (invoiceEntity is null)
                {
                    return NotFound($"Not updated.Invoice with id: {id} not found in DB.");
                }

                _mapper.Map(invoice, invoiceEntity);
                _repositoryWrapper.Invoice.UpdateInvoice(invoiceEntity);

                await _repositoryWrapper.Save();

                /* Invoice document update in updated invoice */
                var pdfFileToInclude = await GetGeneratedPDF(invoiceEntity.Id);
                invoiceEntity.InvoiceDocument = pdfFileToInclude.getInvoiceBytes();
                invoiceEntity.FileName = pdfFileToInclude.getInvoiceFileName();
                _repositoryWrapper.Invoice.UpdateInvoice(invoiceEntity);
                await _repositoryWrapper.Save();
                /* modification end 03.06.2020 */

                return NoContent();
            }
            catch(Exception e)
            {
                _logger.LogError($"Something went wrong inside UpdateInvoice(id, invoiceForUpdateDto) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //DELETE api/invoice
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            try
            {
                var invoice = await _repositoryWrapper.Invoice.GetInvoiceById(id);
                if (invoice == null)
                {
                    _logger.LogError("Invoice Id not found, cannot delete.");
                    return NotFound();
                }

                _repositoryWrapper.Invoice.DeleteInvoice(invoice);
                await _repositoryWrapper.Save();

                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside DeleteInvoice(id) action: {e.Message}");
                return StatusCode(500, e.ToString());
            }
        }

        /*
         *  PDF operations
         */

        //GET api/invoice/{invoice_id}/invoice
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("{id}/invoice")]
        public async Task<IActionResult> GetInvoiceDocumentStoredByInvoiceId(int id)
        {
            try
            {
                var invoice = await _repositoryWrapper.Invoice.GetInvoiceById(id);
                if (invoice == null)
                {
                    _logger.LogError($"Invoice with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned row Invoice, Invoice Document, invoice and tenant id, with id: {id}");
                    var invoiceResult = _mapper.Map<InvoiceDocumentDto>(invoice);
                    return Ok(invoiceResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetInvoiceDocumentStoredByInvoiceId(invoice_id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }

        }


        // View pdf on web, no save a copy in location SimpleFakeRent/Invoices/
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("{id}/view")]
        public async Task<IActionResult> ViewOnWebPDF(int id)
        {

            try
            {

                var invoiceWithAll = await _repositoryWrapper.Invoice.GetInvoiceWihtAllDetailsForInvoiceGeneration(id);
                // Add Application user to get access for email data
                ApplicationUser landlordAspUser = invoiceWithAll.Rent.Landlord.ApplicationUser;
                ApplicationUser tenantAspUser = invoiceWithAll.Rent.Tenant.ApplicationUser;
                Invoice invoice = _mapper.Map<Invoice>(invoiceWithAll);
                Rent rent = _mapper.Map<Rent>(invoiceWithAll.Rent);
                Tenant tenant = _mapper.Map<Tenant>(invoiceWithAll.Rent.Tenant);
                Property property = _mapper.Map<Property>(invoiceWithAll.Rent.Property);
                Landlord landlord = _mapper.Map<Landlord>(invoiceWithAll.Rent.Landlord);
                Photo photo = _mapper.Map<Photo>(invoiceWithAll.State.Photo);
                Rate rate = _mapper.Map<Rate>(invoiceWithAll.Rent.Property.Rate);

                var globalSettings = new GlobalSettings
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    Margins = new MarginSettings { Top = 10 },
                    DocumentTitle = "Invoice title",
                };

                InvoiceFile invoiceGenerated = TemplateGenerator.GetInvoiceHTMLString(landlordAspUser, tenantAspUser, invoice, tenant, property, landlord, rent);

                var objectSettings = new ObjectSettings
                {
                    PagesCount = true,
                    HtmlContent = invoiceGenerated.getInvoiceString(),
                    WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = SystemRecognizer.GetCssFileLocation() },
                    HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                    FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
                };

                var pdf = new HtmlToPdfDocument()
                {
                    GlobalSettings = globalSettings,
                    Objects = { objectSettings }
                };

                //_converter.Convert(pdf);
                var file = _converter.Convert(pdf);

                //return Ok("Successfully created PDF document.");
                return File(file, "application/pdf", invoiceGenerated.getInvoiceFileName());
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside ViewOnWebPDF(id) action: {e.ToString()}");
                return StatusCode(500, e.Message);
            }
        }

        /* new action to be deleted if not working VV */
        protected async Task<InvoiceFile> GetGeneratedPDF(int id)
        {

            try
            {
                var invoiceWithAll = await _repositoryWrapper.Invoice.GetInvoiceWihtAllDetailsForInvoiceGeneration(id);
                // Add Application user to get access for email data
                ApplicationUser landlordAspUser = invoiceWithAll.Rent.Landlord.ApplicationUser;
                ApplicationUser tenantAspUser = invoiceWithAll.Rent.Tenant.ApplicationUser;
                Invoice invoice = _mapper.Map<Invoice>(invoiceWithAll);
                Rent rent = _mapper.Map<Rent>(invoiceWithAll.Rent);
                Tenant tenant = _mapper.Map<Tenant>(invoiceWithAll.Rent.Tenant);
                Property property = _mapper.Map<Property>(invoiceWithAll.Rent.Property);
                Landlord landlord = _mapper.Map<Landlord>(invoiceWithAll.Rent.Landlord);
                Photo photo = _mapper.Map<Photo>(invoiceWithAll.State.Photo);
                Rate rate = _mapper.Map<Rate>(invoiceWithAll.Rent.Property.Rate);

                var globalSettings = new GlobalSettings
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    Margins = new MarginSettings { Top = 10 },
                    DocumentTitle = "Invoice title",
                };

                InvoiceFile invoiceGenerated = TemplateGenerator.GetInvoiceHTMLString(landlordAspUser, tenantAspUser, invoice, tenant, property, landlord, rent);

                var objectSettings = new ObjectSettings
                {
                    PagesCount = true,
                    HtmlContent = invoiceGenerated.getInvoiceString(),
                    WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = SystemRecognizer.GetCssFileLocation() },
                    HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                    FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
                };

                var pdf = new HtmlToPdfDocument()
                {
                    GlobalSettings = globalSettings,
                    Objects = { objectSettings }
                };

                byte[] file = _converter.Convert(pdf);

                invoiceGenerated.setInvoiceBytes(file);

                return invoiceGenerated;
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside ViewOnWebPDF(id) action: {e.ToString()}");
                return null;
            }
        }
        /* new action to be deleted if not working ^^*/


        // Only download, no save in location SimpleaFakeRent/Invoices/
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("{id}/download")]
        public async Task<IActionResult> DownloadPDF(int id)
        {
            try
            {
                var invoiceWithAll = await _repositoryWrapper.Invoice.GetInvoiceWihtAllDetailsForInvoiceGeneration(id);
                // Add Application user to get access for email data
                ApplicationUser landlordAspUser = invoiceWithAll.Rent.Landlord.ApplicationUser;
                ApplicationUser tenantAspUser = invoiceWithAll.Rent.Tenant.ApplicationUser;
                Invoice invoice = _mapper.Map<Invoice>(invoiceWithAll);
                Rent rent = _mapper.Map<Rent>(invoiceWithAll.Rent);
                Tenant tenant = _mapper.Map<Tenant>(invoiceWithAll.Rent.Tenant);
                Property property = _mapper.Map<Property>(invoiceWithAll.Rent.Property);
                Landlord landlord = _mapper.Map<Landlord>(invoiceWithAll.Rent.Landlord);
                Photo photo = _mapper.Map<Photo>(invoiceWithAll.State.Photo);
                Rate rate = _mapper.Map<Rate>(invoiceWithAll.Rent.Property.Rate);

                var globalSettings = new GlobalSettings
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    Margins = new MarginSettings { Top = 10 },
                    DocumentTitle = "Invoice title",
                };

                InvoiceFile invoiceGenerated = TemplateGenerator.GetInvoiceHTMLString(landlordAspUser, tenantAspUser, invoice, tenant, property, landlord, rent);

                var objectSettings = new ObjectSettings
                {
                    PagesCount = true,
                    HtmlContent = invoiceGenerated.getInvoiceString(),
                    WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = SystemRecognizer.GetCssFileLocation() },
                    HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                    FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
                };

                var pdf = new HtmlToPdfDocument()
                {
                    GlobalSettings = globalSettings,
                    Objects = { objectSettings }
                };
 
                var file = _converter.Convert(pdf);

                return File(file, "application/pdf", invoiceGenerated.getInvoiceFileName());
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside DownloadPDF(id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }

        }

        // Send mail to tenant with PDF createt in location SimpleFakeRent/Invoices/Invoice.pdf, Returns OK!
        [Authorize(Policy = "Landlord")]
        [HttpGet("{id}/send/email")]
        public async Task<IActionResult> SendEmailToTenantWithInvoice(int id)
        {
            try
            {
                // Fetch data for email body.
                var invoiceWithAll = await _repositoryWrapper.Invoice.GetInvoiceWihtAllDetailsForInvoiceGeneration(id);

                if (invoiceWithAll == null)
                {
                    return NotFound("No such invoice");
                }

                // Add Application user to get access for email data
                ApplicationUser landlordAspUser = invoiceWithAll.Rent.Landlord.ApplicationUser;
                ApplicationUser tenantAspUser = invoiceWithAll.Rent.Tenant.ApplicationUser;
                State state = _mapper.Map<State>(invoiceWithAll.State);
                Invoice invoice = _mapper.Map<Invoice>(invoiceWithAll);
                Rent rent = _mapper.Map<Rent>(invoiceWithAll.Rent);
                Tenant tenant = _mapper.Map<Tenant>(invoiceWithAll.Rent.Tenant);
                Property property = _mapper.Map<Property>(invoiceWithAll.Rent.Property);
                Landlord landlord = _mapper.Map<Landlord>(invoiceWithAll.Rent.Landlord);
                Photo photo = _mapper.Map<Photo>(invoiceWithAll.State.Photo);
                Rate rate = _mapper.Map<Rate>(invoiceWithAll.Rent.Property.Rate);

                if (state.IsConfirmed == false)
                {
                    return BadRequest("State not confirmed. Please confirm state");
                }

                if (rate == null)
                {
                    return BadRequest("Property has no rates. Please add rates for property first.");
                }

                var message = new Email(
                    new string[] { /*tenant.Email,*/ "ravczar@gmail.com" /*, "grzegorz.zukowski.gda@gmail.com",*/  },
                    $"Nowa Faktura od: {landlord.CompanyName}",
                    $"Nowa faktura do zaplaty za mieszkanie {property.FlatLabel} " +
                    $"{property.Address.City} " +
                    $"{property.Address.Street} " +
                    $"{property.Address.BuildingNumber}/{property.Address.FlatNumber}. Prosimy o terminową wpłatę na konto: {landlord.BankAccount}. ",
                    new List<byte[]> { invoice.InvoiceDocument },
                    new List<string> { invoice.FileName.Replace('/', '-') }
                    );
                await _emailEmmiter.SendMailAsync(message);

                /* After email is sent update invoice status to IsDistributed */
                invoice.IsDistributed = true;
                _repositoryWrapper.Invoice.UpdateInvoice(invoice);
                await _repositoryWrapper.Save();
                /* invoice updated */

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside SendEmailToTenantWithInvoice(id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        // Send mail to tenant with PDF createt in location SimpleFakeRent/Invoices/Invoice.pdf, Returns OK!
        [Authorize(Policy = "Landlord")]
        [HttpGet("{id}/send")]
        public async Task<IActionResult> SendMailToTenantWithPDF(int id)
        {
            try
            {
                var invoiceWithAll = await _repositoryWrapper.Invoice.GetInvoiceWihtAllDetailsForInvoiceGeneration(id);
                // Add Application user to get access for email data
                ApplicationUser landlordAspUser = invoiceWithAll.Rent.Landlord.ApplicationUser;
                ApplicationUser tenantAspUser = invoiceWithAll.Rent.Tenant.ApplicationUser;
                Invoice invoice = _mapper.Map<Invoice>(invoiceWithAll);
                Rent rent = _mapper.Map<Rent>(invoiceWithAll.Rent);
                Tenant tenant = _mapper.Map<Tenant>(invoiceWithAll.Rent.Tenant);
                Property property = _mapper.Map<Property>(invoiceWithAll.Rent.Property);
                Landlord landlord = _mapper.Map<Landlord>(invoiceWithAll.Rent.Landlord);
                Photo photo = _mapper.Map<Photo>(invoiceWithAll.State.Photo);
                Rate rate = _mapper.Map<Rate>(invoiceWithAll.Rent.Property.Rate);

                var globalSettings = new GlobalSettings
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    Margins = new MarginSettings { Top = 10 },
                    DocumentTitle = "Invoice title",
                    //Out = SystemRecognizer.GetGeneratedInvoiceFileLocation() // this line will generate an invoice file on in InvoiceGeneratior/invoices folder but wont let generate proper attachment!!
                };

                InvoiceFile invoiceGenerated = TemplateGenerator.GetInvoiceHTMLString(landlordAspUser, tenantAspUser, invoice, tenant, property, landlord, rent);

                var objectSettings = new ObjectSettings
                {
                    PagesCount = true,
                    HtmlContent = invoiceGenerated.getInvoiceString(),
                    WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = SystemRecognizer.GetCssFileLocation() },
                    HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                    FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
                };

                var pdf = new HtmlToPdfDocument()
                {
                    GlobalSettings = globalSettings,
                    Objects = { objectSettings }
                };

                var invoiceFile = _converter.Convert(pdf);

                var message = new Email(
                    new string[] { /*tenant.Email,*/ "ravczar@gmail.com" /*, "grzegorz.zukowski.gda@gmail.com",*/  },
                    $"Nowa Faktura od: {landlord.CompanyName}",
                    $"Nowa faktura do zaplaty za mieszkanie {property.FlatLabel} " +
                    $"{property.Address.City} " +
                    $"{property.Address.Street} " +
                    $"{property.Address.BuildingNumber}/{property.Address.FlatNumber}. Prosimy o terminową wpłatę na konto: {landlord.BankAccount}. ",
                    new List<byte[]> { invoiceFile },
                    new List<string> { invoiceGenerated.getInvoiceFileName().Replace('/', '-') }
                    );
                await _emailEmmiter.SendMailAsync(message);       

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside SendMailToTenantWithPDF(id) action: {e.ToString()}");
                return StatusCode(500, e.Message);
            }
        }


    }
}