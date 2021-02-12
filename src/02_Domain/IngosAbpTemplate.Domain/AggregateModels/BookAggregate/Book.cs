//-----------------------------------------------------------------------
// <copyright file= "Book.cs">
//     Copyright (c) Danvic.Wang All rights reserved.
// </copyright>
// Author: Danvic.Wang
// Created DateTime: 2021/2/12 20:48:43
// Modified by:
// Description: Book aggregate root entity
//-----------------------------------------------------------------------

using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace IngosAbpTemplate.Domain.AggregateModels.BookAggregate
{
    public class Book : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        ///     ctor, just using for orm
        /// </summary>
        protected Book()
        {
        }

        /// <summary>
        ///     ctor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="publishDate"></param>
        /// <param name="bookType"></param>
        /// <param name="price"></param>
        public Book(string name, DateTime publishDate, BookType bookType, float price)
        {
            Name = name;
            PublishDate = publishDate;
            BookType = bookType;
            Price = price;
        }

        public string Name { get; set; }

        public DateTime PublishDate { get; set; }

        public BookType BookType { get; set; }

        public float Price { get; set; }
    }
}