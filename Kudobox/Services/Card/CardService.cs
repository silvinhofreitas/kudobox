using System;
using System.Linq;
using System.Threading.Tasks;
using Kudobox.Contexts;
using Kudobox.Dto.Card;
using Kudobox.Dto.Shared;
using Kudobox.Helpers.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Kudobox.Services.Card
{
    public class CardService
    {
        private readonly CardContext _cardContext;

        public CardService(CardContext cardContext)
        {
            _cardContext = cardContext;
        }

        public async Task<PagedResultDto> GetAllCards(int page, int pageSize)
        {
            var cardList = await _cardContext.Cards.GetPagedAsync(page, pageSize);
            
            return new PagedResultDto
            {
                Page = cardList.CurrentPage,
                PageCount = cardList.PageCount,
                PageSize = cardList.PageSize,
                HasNext = page < cardList.PageCount,
                HasPrevious = page > 1,
                TotalItemsCount = cardList.RowCount,
                Items = cardList.Results.Select(cl => cl.ToDto()).ToList()
            };
        }

        public async Task<Models.Card.Card> GetCardById(Guid id)
        {
            return await _cardContext.Cards.FirstOrDefaultAsync(c => c.Id.Equals(id));
        }

        public async Task<CardDto> CreateNewCard(CreateCardDto input)
        {
            if (await _cardContext.Cards.FirstOrDefaultAsync(c => c.Name.Equals(input.Name)) != null)
                return null;
            
            var card = new Models.Card.Card(input.Name, input.Description, input.Icon, input.Color);
            await _cardContext.AddAsync(card);
            await _cardContext.SaveChangesAsync();

            return card.ToDto();
        }

        public async Task<CardDto> UpdateCard(Models.Card.Card card, ManageCardDto input)
        {
            card.Description = input.Description;
            card.Icon = input.Icon;
            card.Color = input.Color;

            await _cardContext.SaveChangesAsync();

            return card.ToDto();
        }

        public async Task DeleteCard(Models.Card.Card card)
        {
            // TODO: implement validation to see if the card is registered in a Kudo. If positive, the card will be updated to set the active status to false.
            _cardContext.Remove(card);
            await _cardContext.SaveChangesAsync();
        }
    }
}