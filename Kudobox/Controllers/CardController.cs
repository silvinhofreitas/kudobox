using System;
using System.Threading.Tasks;
using Kudobox.Contexts;
using Kudobox.Dto.Card;
using Kudobox.Dto.Shared;
using Kudobox.Helpers.Constants;
using Kudobox.Services.Card;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Kudobox.Controllers
{
    [ApiController]
    [Route("card")]
    public class CardController : Controller
    {
        private CardService _cardService;
        private IStringLocalizer _translator;

        public CardController(CardContext cardContext, IStringLocalizerFactory factory)
        {
            _cardService = new CardService(cardContext);
            _translator = factory.Create("Card.Card", System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }

        [HttpGet]
        public async Task<ActionResult> GetAllCards(int page = 1, int pageSize = 10)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize;

            var cardList = await _cardService.GetAllCards(page, pageSize);
            return Ok(cardList);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetCardById(Guid id)
        {
            var card = await _cardService.GetCardById(id);
            if (card == null)
                return NotFound(new MessageDto(_translator[CardResourceConstants.CardNotFound]));

            return Ok(card.ToDto());
        }

        [HttpPost]
        public async Task<ActionResult> CreateCard([FromBody] CreateCardDto input)
        {
            var card = await _cardService.CreateNewCard(input);
            if (card == null)
                return BadRequest(new MessageDto(_translator[CardResourceConstants.CardNameAlreadyExists]));
            
            // TODO: Change the URI to the actual URI of the object created
            return Created(card.Id.ToString(), card);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> UpdateCard(Guid id, [FromBody] ManageCardDto input)
        {
            var card = await _cardService.GetCardById(id);
            if (card == null)
                return NotFound(new MessageDto(_translator[CardResourceConstants.CardNotFound]));

            var cardUpdated = await _cardService.UpdateCard(card, input);

            return Ok(cardUpdated);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteCard(Guid id)
        {
            var card = await _cardService.GetCardById(id);
            if (card == null)
                return NotFound(new MessageDto(_translator[CardResourceConstants.CardNotFound]));

            await _cardService.DeleteCard(card);
            
            return Ok(new MessageDto(string.Format(_translator[CardResourceConstants.CardDeleted], card.Name)));
        }
    }
}