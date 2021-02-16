//-----------------------------------------------------------------------
// <copyright file= "BookPublishedDomainEvent.cs">
//     Copyright (c) Danvic.Wang All rights reserved.
// </copyright>
// Author: Danvic.Wang
// Created DateTime: 2021/2/13 10:33:39
// Modified by:
// Description:
//-----------------------------------------------------------------------

using IngosAbpTemplate.Domain.AggregateModels.BookAggregate;

namespace IngosAbpTemplate.Domain.Events
{
    public class BookPublishedDomainEvent
    {
        public BookPublishedDomainEvent(Book book)
        {
            Book = book;
        }

        public Book Book { get; }
    }
}